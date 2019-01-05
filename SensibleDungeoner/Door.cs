using System;
using System.Collections.Generic;

namespace SensibleDungeoner
{
    public class Door
    {
        public int Key { get; set; }
        public Tuple<Room, Room> Rooms { get; set; }
        public bool Closed { get; set; }
        public bool Hidden { get; set; }
        

        public static Door Create(Room room1, Room room2, int keyNumber = 0, bool closed = true, bool hidden = false)
        {
            var door = new Door {Key = keyNumber, Closed = closed, Hidden = hidden, Rooms = new Tuple<Room, Room>(room1, room2)};
            if (room1.Doors == null)
            {
                room1.Doors = new List<Door>();
            }
            if (room2.Doors == null)
            {
                room2.Doors = new List<Door>();
            }
            room1.Doors.Add(door);
            room2.Doors.Add(door);

            return door;
        }

        public bool IsLocked()
        {
            return Key > 0;
        }

        public override string ToString()
        {
            return "Door: (" + (IsLocked() ? "Lock#: " + Key : "Unlocked") + ", " + (Closed ? "Closed" : "Open") + ", " + Rooms.Item1 +  " -> " + Rooms.Item2 + ")";
        }

        public void Lock(int keyNumber)
        {
            Key = keyNumber;
        }

        public void Hide()
        {
            Hidden = true;
        }
    }
}