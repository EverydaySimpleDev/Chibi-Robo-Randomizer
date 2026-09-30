using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp1
{
    // GBA link cable option: adds the Chibi-Robo! GBA Link code to main.dol so a GBA plugged into
    // controller port 2-4 (no cartridge) gets a multiboot program showing the house map, battery,
    // moolah, happy points and popup messages (var(1880) / var(1881), see Resources\stage05.us).
    //
    // C# port of chibi-mini-gba\tools\chibi_link_patch.py (iso mode). The code blob and its symbol
    // list are built in chibi-mini-gba (build.bat) and copied to Resources\chibi_link.bin/.sym.
    // Patches the ISO in place, so call it after every unplug command has finished.
    public static class GbaLinkPatcher
    {
        const uint SectionAddr = 0x80672300;   // retail __ArenaLo
        const uint MailboxAddr = 0x80672300;   // cl_mailbox
        const uint HookSite = 0x800156C8;      // bl VIWaitForRetrace in main()
        const uint HookOrig = 0x4815BF69;
        const uint PadSite = 0x801CC118;       // the game's only bl PADRead
        const uint PadOrig = 0x4BFA7881;

        // (lis addr, addi addr, original lis, original addi) in OSInit
        static readonly uint[][] ArenaSites =
        {
            new uint[] { 0x801615F4, 0x801615F8, 0x3C608067, 0x38632300 }, // ArenaLo = __ArenaLo
            new uint[] { 0x8016162C, 0x80161630, 0x3C608067, 0x386302F0 }, // debugger path
        };

        public static string PatchIso(string isoPath, string binPath, string symPath)
        {
            byte[] blob = File.ReadAllBytes(binPath);
            Dictionary<string, uint> syms = ReadSymbols(symPath);

            using (var fs = new FileStream(isoPath, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] header = ReadAt(fs, 0, 0x440);
                string gameId = System.Text.Encoding.ASCII.GetString(header, 0, 6);
                if (gameId != "GGTE01")
                    throw new Exception("GBA link: ISO is " + gameId + ", only GGTE01 is supported");

                uint dolOff = BE32(header, 0x420);
                uint fstOff = BE32(header, 0x424);
                uint fstSize = BE32(header, 0x428);

                byte[] dolHeader = ReadAt(fs, dolOff, 0x100);
                byte[] dol = ReadAt(fs, dolOff, (int)DolFileEnd(dolHeader));
                byte[] fst = ReadAt(fs, fstOff, (int)fstSize);

                // every file's (offset, size) on the disc
                uint count = BE32(fst, 8);
                var files = new List<KeyValuePair<long, long>>();
                for (uint i = 1; i < count; i++)
                    if (fst[i * 12] == 0)
                        files.Add(new KeyValuePair<long, long>(BE32(fst, (int)(i * 12 + 4)), BE32(fst, (int)(i * 12 + 8))));

                byte[] newDol = PatchDol(dol, blob, syms);
                byte[] ptr = new byte[4];
                string where;

                // 1) vanilla layout: the FST directly follows main.dol with room to grow - write the
                //    DOL in place and move the FST up behind it.
                long firstAfterFst = fs.Length;
                foreach (var f in files)
                    if (f.Key >= fstOff + fstSize)
                        firstAfterFst = Math.Min(firstAfterFst, f.Key);
                uint newFstOff = Align((uint)(dolOff + newDol.Length), 0x100);

                if (fstOff >= dolOff + dol.Length && newFstOff + fstSize <= firstAfterFst)
                {
                    WriteAt(fs, dolOff, new byte[newFstOff + fstSize - dolOff]);
                    WriteAt(fs, dolOff, newDol);
                    WriteAt(fs, newFstOff, fst);
                    PutBE32(ptr, 0, newFstOff);
                    WriteAt(fs, 0x424, ptr);
                    where = string.Format("FST moved {0:X8} -> {1:X8}", fstOff, newFstOff);
                }
                else
                {
                    // 2) no room (after unplug's imports, qp.bin is written right after the FST):
                    //    put the DOL in the first free gap on the disc and point the header's DOL
                    //    offset (0x420) at it. The FST and all files stay where they are.
                    var used = new List<KeyValuePair<long, long>>(files)
                    {
                        new KeyValuePair<long, long>(0, dolOff),
                        new KeyValuePair<long, long>(dolOff, dol.Length),
                        new KeyValuePair<long, long>(fstOff, fstSize),
                        new KeyValuePair<long, long>(fs.Length, 0),
                    };
                    used.Sort((a, b) => a.Key.CompareTo(b.Key));
                    long end = 0, newDolOff = -1;
                    foreach (var u in used)
                    {
                        long gap = (end + 0x7FFF) & ~0x7FFFL;
                        if (gap + newDol.Length <= u.Key) { newDolOff = gap; break; }
                        end = Math.Max(end, u.Key + u.Value);
                    }
                    if (newDolOff < 0)
                        throw new Exception("GBA link: no free space on this disc for the patched main.dol");

                    WriteAt(fs, newDolOff, newDol);
                    PutBE32(ptr, 0, (uint)newDolOff);
                    WriteAt(fs, 0x420, ptr);
                    where = string.Format("main.dol moved {0:X8} -> {1:X8}", dolOff, newDolOff);
                }

                return string.Format("GBA link: added {0} bytes at {1:X8}, mailbox at {2:X8} ({3})",
                                     blob.Length, SectionAddr, MailboxAddr, where);
            }
        }

        static byte[] PatchDol(byte[] dolIn, byte[] blob, Dictionary<string, uint> syms)
        {
            var dol = new Dol(dolIn);

            if (dol.Read32(HookSite) != HookOrig)
                throw new Exception("GBA link: main.dol is already patched or is not GGTE01 rev 0");
            if (dol.Read32(PadSite) != PadOrig)
                throw new Exception("GBA link: unexpected code at the PADRead call site");
            foreach (uint[] a in ArenaSites)
                if (dol.Read32(a[0]) != a[2] || dol.Read32(a[1]) != a[3])
                    throw new Exception("GBA link: unexpected OSInit code at " + a[0].ToString("X8"));

            if (Sym(syms, "__cl_start") != SectionAddr || Sym(syms, "cl_mailbox") != MailboxAddr)
                throw new Exception("GBA link: chibi_link.bin was not linked at " + SectionAddr.ToString("X8"));
            uint end = Align(Sym(syms, "__cl_end"), 0x20);
            if (SectionAddr + blob.Length > end)
                throw new Exception("GBA link: chibi_link.bin does not match chibi_link.sym");

            dol.AddTextSection(SectionAddr, blob);
            dol.Write32(HookSite, BranchLink(HookSite, Sym(syms, "cl_frame_hook")));
            dol.Write32(PadSite, BranchLink(PadSite, Sym(syms, "cl_pad_read")));

            uint hi = (end + 0x8000) >> 16, lo = end & 0xFFFF;
            foreach (uint[] a in ArenaSites)
            {
                dol.Write32(a[0], 0x3C600000 | hi);   // lis  r3, hi
                dol.Write32(a[1], 0x38630000 | lo);   // addi r3, r3, lo
            }
            return dol.Data.ToArray();
        }

        class Dol
        {
            public List<byte> Data;
            uint[] off = new uint[18], addr = new uint[18], size = new uint[18];
            uint bssAddr, bssSize;

            public Dol(byte[] d)
            {
                Data = new List<byte>(d);
                for (int i = 0; i < 18; i++)
                {
                    off[i] = BE32(d, i * 4);
                    addr[i] = BE32(d, 0x48 + i * 4);
                    size[i] = BE32(d, 0x90 + i * 4);
                }
                bssAddr = BE32(d, 0xD8);
                bssSize = BE32(d, 0xDC);
            }

            int OffsetOf(uint a)
            {
                for (int i = 0; i < 18; i++)
                    if (size[i] != 0 && a >= addr[i] && a < addr[i] + size[i])
                        return (int)(off[i] + a - addr[i]);
                throw new Exception("GBA link: address " + a.ToString("X8") + " is not in main.dol");
            }

            public uint Read32(uint a)
            {
                int o = OffsetOf(a);
                return (uint)(Data[o] << 24 | Data[o + 1] << 16 | Data[o + 2] << 8 | Data[o + 3]);
            }

            public void Write32(uint a, uint v)
            {
                int o = OffsetOf(a);
                Data[o] = (byte)(v >> 24); Data[o + 1] = (byte)(v >> 16);
                Data[o + 2] = (byte)(v >> 8); Data[o + 3] = (byte)v;
            }

            void SetHeader(int field, int slot, uint v)
            {
                int o = field + slot * 4;
                Data[o] = (byte)(v >> 24); Data[o + 1] = (byte)(v >> 16);
                Data[o + 2] = (byte)(v >> 8); Data[o + 3] = (byte)v;
            }

            public void AddTextSection(uint a, byte[] blob)
            {
                int slot = -1;
                for (int i = 0; i < 7 && slot < 0; i++)
                    if (size[i] == 0)
                        slot = i;
                if (slot < 0)
                    throw new Exception("GBA link: no free text section in main.dol");
                uint blobEnd = a + (uint)blob.Length;
                for (int i = 0; i < 18; i++)
                    if (size[i] != 0 && addr[i] < blobEnd && a < addr[i] + size[i])
                        throw new Exception("GBA link: new section overlaps an existing one");
                if (bssAddr < blobEnd && a < bssAddr + bssSize)
                    throw new Exception("GBA link: new section overlaps .bss");

                while (Data.Count % 0x20 != 0) Data.Add(0);
                uint o = (uint)Data.Count;
                Data.AddRange(blob);
                while (Data.Count % 0x20 != 0) Data.Add(0);

                off[slot] = o; addr[slot] = a; size[slot] = Align((uint)blob.Length, 0x20);
                SetHeader(0x00, slot, off[slot]);
                SetHeader(0x48, slot, addr[slot]);
                SetHeader(0x90, slot, size[slot]);
            }
        }

        static uint DolFileEnd(byte[] h)
        {
            uint end = 0;
            for (int i = 0; i < 18; i++)
            {
                uint s = BE32(h, 0x90 + i * 4);
                if (s != 0)
                    end = Math.Max(end, BE32(h, i * 4) + s);
            }
            return end;
        }

        static Dictionary<string, uint> ReadSymbols(string path)
        {
            var syms = new Dictionary<string, uint>();
            foreach (string line in File.ReadAllLines(path))
            {
                string[] p = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length == 3)
                    syms[p[2]] = Convert.ToUInt32(p[0], 16);
            }
            return syms;
        }

        static uint Sym(Dictionary<string, uint> syms, string name)
        {
            uint v;
            if (!syms.TryGetValue(name, out v))
                throw new Exception("GBA link: symbol " + name + " missing from chibi_link.sym");
            return v;
        }

        static uint BranchLink(uint site, uint target)
        {
            int rel = (int)(target - site);
            if (rel < -0x2000000 || rel >= 0x2000000)
                throw new Exception("GBA link: branch out of range");
            return 0x48000001 | ((uint)rel & 0x03FFFFFC);
        }

        static uint Align(uint v, uint a) { return (v + a - 1) & ~(a - 1); }

        static uint BE32(byte[] b, int o)
        {
            return (uint)(b[o] << 24 | b[o + 1] << 16 | b[o + 2] << 8 | b[o + 3]);
        }

        static void PutBE32(byte[] b, int o, uint v)
        {
            b[o] = (byte)(v >> 24); b[o + 1] = (byte)(v >> 16); b[o + 2] = (byte)(v >> 8); b[o + 3] = (byte)v;
        }

        static byte[] ReadAt(FileStream fs, long pos, int len)
        {
            var buf = new byte[len];
            fs.Seek(pos, SeekOrigin.Begin);
            int got = 0;
            while (got < len)
            {
                int n = fs.Read(buf, got, len - got);
                if (n <= 0) throw new EndOfStreamException("GBA link: unexpected end of ISO");
                got += n;
            }
            return buf;
        }

        static void WriteAt(FileStream fs, long pos, byte[] data)
        {
            fs.Seek(pos, SeekOrigin.Begin);
            fs.Write(data, 0, data.Length);
        }
    }
}
