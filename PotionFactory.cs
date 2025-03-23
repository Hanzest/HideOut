using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class PotionFactory : IItemFactory
    {
        public PotionFactory() { }
        public Item? Create(string name, float x, float y)
        {
            switch (name)
            {
                case "Health potion":
                    return new Potion(ItemType.Potion, name, false, x, y);
                case "Energy potion":
                    return new Potion(ItemType.Potion, name, false, x, y);
                default:
                    return null;
            }
        }
    }
}
