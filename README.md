# Chibi-Robo Randomizer

Chibi-Robo Randomizer is a Windows application that randomizes the locations of equipment, quest items, trash, and more.

Chibi Robo Unplug Emo Tracker Pack: https://github.com/EverydaySimpleDev/EmoTrackerPacks

![Screencap of Randomizer GUI](/Randomizer/interfaceIMG.PNG)

## How to Use

- Open ISO
  - Click this button to select a NTSC-USA copy of the Chibi-Robo ISO file. (Do not ask for a download link for this, you must source it yourself!)
- Browse
  - The randomizer will create a copy of the ISO file before applying the edits to the game files. Use the Browse button to select where this file will be saved.
- Open AP
  - This is a json file that the archipelago system will generate. This is required if you plan on connecting your game with the Archipelago multiworld rando https://github.com/EverydaySimpleDev/Archipelago.
- Mode / Logic
  - Select a game mode / logic setting from the dropdown. Currently, only Glitchless is supported, but make sure at least something is selected in this dropdown.
- Seed
  - The seed is a series of alphanumeric characters that correspond to how the items are randomized into the game. Given the same seed, the randomizer will put the items in the same locations. This way, two people can generate the same version of a randomized game for races / co-op runs. This will be filled in automatically by default
- Free PJs
  - Checking this box will add the Pajamas as a bonus item to the Chibi-Shop for 10 Moolah, which can allow for more convenient access to areas / locations blocked by day/night-specific events.
- Open Upsatirs
  - Checking this box will insert a stack of books onto the 2nd stair in the Foyer, allowing immediate access upstairs without the need for the ladder. This will impact item shuffling logic.
- Charged Battery
  - Checking this will shuffle a fully-charged battery as a bonus item into the game (in addition to the default uncharged battery, which is needed to unlock scrap)
- Open Downstairs
  - (NOT FULLY IMPLEMENTED) Checking this will enable the entrances to the Foyer and Kitchen immediately from the start of the game without having to move Sophie from the Kitchen door. This will impact item shuffling logic. (For now, this will just add the Drake Redcrest Suit to the shop for 10 moolah)
- Chibi-Vision Off
  - Checking this will remove the item tag for key items that can be seen while Chibi-Vision is active, making it more difficult to sweep for key items immediately after visiting a room for the first time
- Randomize Password
  - Checking this will randomize the password required to turn on Giga-Robo to finish the game. This will require finding the engagement ring and reading the newly generated password from its item description, which will also be updated with the new random password.
- Randomize
  - Click this button once finished with the above controls and the randomizer will create a new ISO at the file path you have specified with the Browse button!

## KNOWN ISSUES

- The program will crash if any part of the file path where the program is extracted contains a space
- Randomization for the Copter, Blaster, and Radar are planned but not supported
- Duplicate items cannot be randomized to different slots in the shop (meaning frog rings cannot appear in the shop)
- If a shop location doesn't have an item randomized to it, it will not appear as an option in the shop (this is a temporary fix for the above)
- AP Worlds do not have the shop items included in the rando

# Enable Debug Menu

I would recoumend using the below Gecko code in your dolphin so you can open the custom debug menu.
This is a temp fix for the blaster / copter / radar items just in case you have a hard time with collecting them.
It allows you to press X + Start to open the debug menu created by the randomizer

C200FF60 00000003
801C19BC 5400056B
41820008 80648584
81830148 00000000
