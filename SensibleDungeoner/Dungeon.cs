using System;
using System.Collections.Generic;
using System.Linq;

namespace SensibleDungeoner
{
    public class Dungeon
    {
        public List<Room> Rooms { get; set; }
        public int KeyAmount { get; set; }
        public int PathAmount { get; set; }
        
        private Random rand { get; set; }

        public int NextKeyNumber()
        {
            KeyAmount++;
            return KeyAmount;
        }
        
        public int NextPathNumber()
        {
            PathAmount++;
            return PathAmount;
        }

        /// <summary>
        /// Generate a dungeon of a random amount of rooms (between 5 and 10 by default
        /// </summary>
        /// <param name="minRooms"></param>
        /// <param name="maxRooms"></param>
        /// <param name="lockChance"></param>
        /// <param name="passageChance"></param>
        /// <param name="hiddenPassageChance"></param>
        /// <param name="detourChance"></param>
        public void Generate(
            int minRooms = 5,
            int maxRooms = 10,
            double lockChance = 0.10,
            double passageChance = 0.15,
            double hiddenPassageChance = 0.35,
            double detourChance = 0.10)    // FIXME: Export magic numbers
        {
            KeyAmount = 0;
            PathAmount = 0;
            
            // Determine critical path length;
            rand = new Random();
            var roll = rand.Next(minRooms, maxRooms);
            
            // Add Starting Room;
            var startingRoom = new Room {Name = "S1R0"};
            Rooms = new List<Room> {startingRoom};

            // Create Critical Path
            var path = CreateSubpath(startingRoom, PathType.Normal, 0, minRooms, maxRooms, lockChance, passageChance, hiddenPassageChance, detourChance);
            Rooms.AddRange(path.Rooms);
        }

        private Path CreateSubpath(
            Room startingRoom,
            PathType pathType = PathType.Normal,
            int keyNumber = 0,
            int minRooms = 3,
            int maxRooms = 5,
            double lockChance = 0.10,
            double passageChance = 0.15,
            double hiddenPassageChance = 0.35,
            double detourChance = 0.10)    // FIXME: Export magic numbers
        {
            // Determine critical path length;
            var roll = rand.Next(minRooms, maxRooms);
            
            // Add Starting Room;
            var subpath = Path.CreateEmpty();
            var pathNumber = NextPathNumber();
            var currentRoom = startingRoom;
            
            // While there are still rooms to add to the critical path AND it’s not the final room:
            for (var i = 1; i < roll; i++)
            {
                var nextRoom = new Room {Name = $"S{pathNumber}R{i} - {pathType}"};
                
                // Add next room;
                subpath.Rooms.Add(nextRoom);
                
                // Add door to the next room;
                var door = Door.Create(currentRoom, nextRoom);
                if (pathType == PathType.Hidden)
                {
                    door.Hide();
                }
                
                // Determine if the door is locked;
                var rollPercent = rand.NextDouble();
                
                // If the door is locked:
                if (rollPercent < lockChance)
                {
                    // Lock the door;
                    var generatedKeyNumber = NextKeyNumber();
                    door.Lock(generatedKeyNumber);
                    // Create a Key subpath();
                    var path = CreateSubpath(currentRoom, PathType.Key, generatedKeyNumber);
                    subpath.Rooms.AddRange(path.Rooms);
                }
                
                // Determine if the room has a bifurcation;
                rollPercent = rand.NextDouble();
                
                // If there is a bifurcation:
                if (rollPercent < passageChance)
                {
                    // Determine if the bifurcation is hidden;
                    rollPercent = rand.NextDouble();
                    
                    // If the passage is hidden:
                    if (rollPercent < hiddenPassageChance)
                    {
                        // Create a Hidden subpath();
                        var path = CreateSubpath(currentRoom, PathType.Hidden);
                        subpath.Rooms.AddRange(path.Rooms);
                    }
                    else
                    {
                        // Create a Normal subpath();
                        var path = CreateSubpath(currentRoom);
                        subpath.Rooms.AddRange(path.Rooms);
                    }
                }
                
                // Determine if there is a detour to the next room*;
                rollPercent = rand.NextDouble();

                // If there is a detour:
                if (rollPercent < detourChance)
                {
                    // Create a Detour subpath that starts in this room and ends on the next one();
                    var path = CreateSubpath(currentRoom, PathType.Detour);
                    
                    // Add door to the next room;
                    var detourDoor = Door.Create(path.Rooms.LastOrDefault(), nextRoom);
                    
                    // Is it a hidden door?
                    rollPercent = rand.NextDouble();
                    if (rollPercent < hiddenPassageChance)
                    {
                        detourDoor.Hide();
                    }
                    
                    subpath.Rooms.AddRange(path.Rooms);
                }

                currentRoom = nextRoom;
            }
    
            // Last room treatment
            var lastRoom = subpath.Rooms.LastOrDefault();
            if (lastRoom.Items == null)
            {
                lastRoom.Items = new List<Item>();
            }
            
            // Add the key if this is a Key Subpath
            if (pathType == PathType.Key)
            {
                lastRoom.Items.Add(new Item {Name = $"Key#{keyNumber}"});
            }
            
            // Add a reward too unless it's a Detour Subpath
            if (pathType != PathType.Detour)
            {
                lastRoom.Items.Add(new Item{Name = "Reward"});
            }

            return subpath;
        }

        private enum PathType
        {
            Normal,
            Hidden,
            Key,
            Detour
        }
    }
}