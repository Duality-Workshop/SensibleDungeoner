using System.Collections.Generic;

namespace SensibleDungeoner
{
    public class Path
    {
        public List<Room> Rooms { get; set; }
        public Room ParentRoom { get; set; }
        public Room EndingRoom { get; set; }

        public static Path CreateEmpty()
        {
            return new Path{Rooms = new List<Room>(), ParentRoom = null, EndingRoom = null};
        }

        public Path Create(List<Room> rooms, Room parent = null, Room ending = null)
        {
            return new Path{Rooms = rooms, ParentRoom = parent, EndingRoom = ending};
        }
        
        public bool IsCriticalPath()
        {
            return ParentRoom == null;
        }
        
        public bool IsDetour()
        {
            return EndingRoom == null;
        }
    }
}