using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class MeleeWeaponFactory : IItemFactory
    {
        public MeleeWeaponFactory() { }
        public Item? Create(string name, float x, float y)
        {
            switch (name)
            {
                case "Iron Sword":
                    return new MeleeWeapon(ItemType.MeleeWeapon, name, "paleslash", x, y, 35, 0, 1500, 32, 160, 160);
                case "Light Saber":
                    return new MeleeWeapon(ItemType.MeleeWeapon, name, "slash", x, y, 20, 0,400, 16, 160, 160);
                default:
                    return null;
            }
        }
    }
}
