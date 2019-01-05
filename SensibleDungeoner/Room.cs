using System;
using System.Collections.Generic;
using System.Linq;

namespace SensibleDungeoner
{
    public class Room
    {
        public string Name { get; set; }
        public List<Door> Doors { get; set; }
        public List<Item> Items { get; set; }

        public Room NextRoom(Door door)
        {
            return door.Rooms.Item1 == this ? door.Rooms.Item2 : door.Rooms.Item1;
        }

        public override string ToString()
        {
            // Format: "Room: (Name: {string}, Items: [{Item}, ...])"
            return "Room: (Name: " + Name + (Items != null && Items.Any() ? ", Items: [" + string.Join(",", Items) + "]" : "") + ")";
        }
    }
}