using System;

namespace SensibleDungeoner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dungeon = new Dungeon();
            dungeon.Generate();
            Console.WriteLine(dungeon);
        }
    }
}