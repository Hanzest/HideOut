using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public abstract class Weapon : Item
    {
        private bool _isCritical;
        private int _damage;
        private int _energyCost;
        protected string _effect;
        protected int _width;
        protected int _height;
        protected double _attackCooldown;
        protected double _lastAttackTime;
        protected Point2D _attackPosition;
        public Weapon(ItemType type, string name, string effect, float x, float y, int damage, int energyCost, int attackCooldown, int width, int height) : base(type, name, false, x, y)
        {
            _damage = damage;
            _energyCost = energyCost;
            _attackCooldown = attackCooldown;
            int rnd = SplashKit.Rnd(1, 10);
            if (rnd > 7) {
                _isCritical = true;
            }
            _effect = effect;
            _width = width;
            _height = height;
        }
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public bool IsCritical {
            get { return _isCritical; } 
        }
        public double AttackCoolDown
        {
            get { return _attackCooldown; }
            set { _attackCooldown = value; }
        }
        public int Damage
        {
            get { return _damage; }
        }
        public int EnergyCost
        {
            get { return _energyCost; }
        }
        public abstract void UseBy(Character c, Point2D point2D,
                                    HashSet<Effect> effects, EffectFactory effectFactory,
                                    HashSet<Projectile> projectiles, ProjectileFactory projectileFactory);

         public abstract Point2D Attack(Character c, Point2D point2D, int range);
    }
}
