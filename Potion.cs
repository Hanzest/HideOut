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
        public void UseBy(Character c)
        {
            Player p = (Player)c;
            if (Name == "Energy potion")
            {
                p.EnergyChanged(50);
                Exist = false;
            }
            else if (Name == "Health potion")
            {
                p.HealthChanged(15);
                Exist = false;
            }
        }
    }
}
