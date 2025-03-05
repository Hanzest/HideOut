using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Potion : Item
    {
        public Potion(ItemType type, string name, bool inInventory, float x, float y) : base(type, name, inInventory, x, y) { }
        public void Use(Player p)
        {
            if (Name == "Energy potion")
            {
                p.EnergyChanged(80);
                Exist = false;
            }
            else if (Name == "Health potion")
            {
                p.HealthChanged(10);
                Exist = false;
            }
        }
    }
}
