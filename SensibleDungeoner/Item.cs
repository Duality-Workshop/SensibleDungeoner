using System.Dynamic;

namespace SensibleDungeoner
{
    public class Item
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return "Item: " + Name;
        }
    }
}