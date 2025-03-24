using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class RangeWeapon : Weapon
    {
        private string _bullet;
        public RangeWeapon(ItemType type, string name, string bullet, string effect, float x, float y, int damage, int energyCost, int attackCooldown, int width, int height)
            : base(type, name, effect, x, y, damage, energyCost, attackCooldown, width, height)
        {
            _bullet = bullet;
            _attackCooldown = attackCooldown;
        }
        public override void UseBy(Character c, Point2D point2D,
                            HashSet<Effect> effects, EffectFactory effectFactory,
                            HashSet<Projectile> projectiles, ProjectileFactory projectileFactory)
        {
            if (!InInventory)
            {
                InInventory = true;
            }
            else
            {
                double currentTime = SplashKit.CurrentTicks();
                if (currentTime - _lastAttackTime >= _attackCooldown)
                {
                    _lastAttackTime = currentTime;
                    _attackPosition = Attack(c, point2D, Width);
                    Angle = Math.Atan2(point2D.Y - c.Y, point2D.X - c.X);
                    bool isSameRoom = (c.RoomIndex % 2 == 0) ? true : false;
                    float attackPositionX = _attackPosition.X;
                    float attackPositionY = _attackPosition.Y;
                    int weaponWidthParameter = Width / 2;
                    if (_bullet == "shotgunBullet")
                    {
                        weaponWidthParameter *= 2;
                    }
                        Projectile? projectile = projectileFactory.Create(
                            _bullet, _attackPosition.X, _attackPosition.Y + 8, IsCritical,
                            (float)Angle, weaponWidthParameter, c.Type,
                            isSameRoom);
                    if (projectile == null)
                    {
                        return;
                    } else
                    {
                        
                        if(c.Type == CharacterType.Player)
                        {
                            Player p = (Player)c;
                            if(p.Energy - EnergyCost >= 0)
                            {
                                p.EnergyChanged(-EnergyCost);
                                projectiles.Add(projectile);
                            }
                        } else
                        {
                            projectiles.Add(projectile);
                        }
                    }
                }
            }
        }
        public override Point2D Attack(Character c, Point2D point2D, int range)
        {
            _attackPosition.X = c.X;
            _attackPosition.Y = c.Y;
            return _attackPosition;
        }
    }
}
