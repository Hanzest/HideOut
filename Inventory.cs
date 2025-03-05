using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Inventory
    {
        private int _maxSize;
        private List<Item> _items;
        public Inventory(int maxSize)
        {
            _items = new List<Item>();
            _maxSize = maxSize;
        }
        public void Add(Item item)
        {
            if(_items.Count == _maxSize)
            {
                Console.WriteLine("Inventory is full");
                return;
            }
            _items.Add(item);
        }
        public void Remove(Item item)
        {
            _items.Remove(item);
        }
        public List<Item> Items
        {
            get { return _items; }
        }
    }
}
