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
                                    0, 0, 200, 16, 16);
                case "sawed-off shotgun":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "shotgunBullet", "null", x, y,
                                    0, 2, 275, 60, 40);
                case "sniper":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "sniperBullet", "bulletCollision", x, y,
                                    0, 3, 300, 80, 33);
                case "rifle":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "rifleBullet", "bulletCollision", x, y,
                                    0, 1, 125, 100, 60);
                case "Broken Glass":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "Glass", "cut", x, y,
                                    0, 0, 60, 24, 24);
                case "snipezooka":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "snipezookaBullet", "bulletCollision", x, y,
                                    0, 0, 225, 48, 24);
                case "fireBeam":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "fireBullet", "bulletCollision", x, y,
                                    0, 0, 18, 1, 1);
                case "fireWand":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "fireParticle", "fireExplosion", x, y,
                                    0, 8, 500, 64, 96);
                case "Plutonium":
                    return new RangeWeapon(ItemType.RangeWeapon, name, "Plutoniumball", "radiation", x, y,
                                    0, 10, 550, 60, 80);
                default:
                    return null;
            }
        }
    }
}
