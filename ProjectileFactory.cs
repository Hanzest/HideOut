using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class ProjectileFactory
    {
        public Projectile? Create(string name, float x, float y, bool isCritical, float angle, int weaponWidth, CharacterType shootBy, bool isSameRoom)
        {
            y += 15;
            switch (name)
            {
                case "pistolBullet":
                    return new Projectile("bulletCollision", name, x, y, 13.5f, 30, isCritical, angle, weaponWidth, shootBy, 16, 16, isSameRoom);
                case "shotgunBullet":
                    return new Projectile("null", name, x, y, 0, 120, isCritical, angle, weaponWidth, shootBy, 192, 64, isSameRoom);
                case "sniperBullet":
                    return new Projectile("bulletCollision", name, x, y, 40.5f, 60, isCritical, angle, weaponWidth, shootBy, 24, 11, isSameRoom);
                case "rifleBullet":
                    return new Projectile("bulletCollision", name, x, y, 15.5f, 20, isCritical, angle, weaponWidth, shootBy, 24, 11, isSameRoom);
                case "snipezookaBullet":
                    return new Projectile("bulletCollision", name, x, y, 19.5f, 25, isCritical, angle, weaponWidth, shootBy, 16, 8, isSameRoom);
                case "fireBullet":
                    return new Projectile("bulletCollision", name, x, y, 17.5f, 1, isCritical, angle, weaponWidth, shootBy, 36, 12, isSameRoom);
                case "Glass":
                    return new Projectile("cut", name, x, y, 20.5f, 3, isCritical, angle, weaponWidth, shootBy, 24, 24, isSameRoom);
                default:
                    return null;
            }
        }
    }
}
