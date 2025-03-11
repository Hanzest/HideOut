using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class ProjectileFactory
    {
        public Projectile? Create(string name, float x, float y, bool isCritical, float angle, int weaponWidth, CharacterType shootBy)
        {
            y += 15;
            switch (name)
            {
                case "pistolBullet":
                    return new Projectile("bulletCollision", name, x, y, 10.5f, 15, isCritical, angle, weaponWidth, shootBy, 16, 16);
                case "shotgunBullet":
                    return new Projectile("null", name, x, y, 0, 27, isCritical, angle, weaponWidth, shootBy, 100, 64);
                case "sniperBullet":
                    return new Projectile("bulletCollision", name, x, y, 40.5f, 60, isCritical, angle, weaponWidth, shootBy, 24, 11);
                case "rifleBullet":
                    return new Projectile("bulletCollision", name, x, y, 15.5f, 20, isCritical, angle, weaponWidth, shootBy, 24, 11);
                default:
                    return null;
            }
        }
    }
}
