using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class RangeWeaponFactory : IItemFactory
    {
        public RangeWeaponFactory() { }
        public Item? Create(string name, float x, float y) 
        {
            switch (name)
            {
                case "revolver":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "pistolBullet", "bulletCollision", x, y,
                                    35, 0, 200, 16, 16);
                case "sawed-off shotgun":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "shotgunBullet", "null", x, y,
                                    25, 2, 450, 100, 64);
                case "sniper":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "sniperBullet", "bulletCollision", x, y,
                                    80, 4, 800, 80, 33);
                case "rifle":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "rifleBullet", "bulletCollision", x, y,
                                    15, 1, 75, 100, 60);
                case "Broken Glass":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "Glass", "cut", x, y,
                                    2, 0, 25, 24, 24);
                case "snipezooka":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "snipezookaBullet", "bulletCollision", x, y,
                                    25, 2, 225, 48, 24);
                case "fireBeam":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "fireBullet", "bulletCollision", x, y,
                                    1, 0, 18, 1, 1);
                default:
                    return null;
            }
        }
    }
}
