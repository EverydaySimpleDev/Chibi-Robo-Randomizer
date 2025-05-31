using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public class ItemPool
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
    public class Item
    {
        [JsonProperty("objectName")]
        public string objectName { get; set; }
        [JsonProperty("itemName")]
        public string itemName { get; set; }
        [JsonProperty("numChecks")]
        public int numChecks { get; set; }
        [JsonProperty("flags")]
        public string[] flags { get; set; }

        // Flags For Items:

        //spawn	The object spawns when the stage loads.

        //opaque	The object can obscure the player without showing any silhouette.

        //blastthru	The object allows blaster projectiles to pass through it.

        //radar	The radar will point to the object if it is nearby.

        //intangible	The object is not solid and other objects can pass through it.

        //invisible	The object is drawn fully transparent. This isn't the same as the object not rendering at all because it may still obscure some objects and shadows.

        //toon	The object is lit using a toon effect.

        //flash	The object flashes like an item.

        //unlit	It's unclear exactly what this does. It's some sort of weird rendering effect. The unlit name is tentative.

        //botcam	The object always shows in the utilibot camera window.

        //explode	The object can be destroyed with the blaster.

        //pushthru	The object allows other objects to be pushed through it.

        //lowpri	The object will not be prioritized in interactions. If this is not set and the player presses A close to the object, they will automatically walk up and interact with it.

        //reflect	The object shows in the floor reflection.

        //pushblock	The object blocks other objects from being pushed through it.

        //cull	The object is culled when not being looked at (this is mainly a performance optimization).

        //lift	The player can lift the object up.

        //climb	The player can climb on the object.

        //clamber	The player can clamber up to surfaces on the object.

        //ladder	The player can climb up the object as a ladder.

        //rope	The player can climb up the object as a rope.

        //stairs	The object is a staircase (i.e. it has internal ledges). The object's data value indicates the height of each step.

        //fall	The object will fall if it is pushed off a ledge.

        //grab	The player can grab the object and push/pull it.

        //interact	The object can be interacted with by walking up to it and pressing A.

        //touch	The object responds to being touched by the player.

        //atc	The object responds to attachments.

        //projectile	The object responds to projectiles.

        //unk28	???

        //mirror	The object shows in mirrors.

        //unk30	???

        //disabled	The object is disabled and cannot be spawned.
        // Note that this does not fully work with all object types.
    }

    //Handles check locations
    public class RootObject
    {
        [JsonProperty("rooms")]
        public List<StageObject> rooms { get; set; }
    }

    public class StageObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("checks")]
        public List<ItemLocation> locations { get; set; }
    }

    public class ItemLocation
    {
        //Getters & Setters
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string ObjectName { get; set; }

        [JsonProperty("prereqs")]
        public string[] Prereqs { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }
}
