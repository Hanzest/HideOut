using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class MeleeWeapon : Weapon
    {
        private bool _isAttack;
        private int _attackTick;
        private int _maxAttackTick;
        private double _angle;
        private CharacterType _holder;
        public MeleeWeapon(ItemType type, string name, string effect, float x, float y, int damage, int energyCost, int attackCooldown, int maxAttackTick, int width, int height)
            : base(type, name, effect, x, y, damage, energyCost, attackCooldown, width, height)
        {
            _isAttack = false;
            _angle = 0;
            _maxAttackTick = maxAttackTick;
            _attackTick = 0;
            _holder = CharacterType.Player;
        }
        public double Angle
            // Implementing Sword rotated using Angle (in radius)
        {
            get 
            {
                return _angle * Math.PI / 180d;
            }
        }
        public bool IsAttack
        {
            get { return _isAttack; }
        }
        public CharacterType Holder
        {
            get { return _holder; }
        }
        public void AttackTick()
        {
            if (_isAttack)
            {
                if (_attackTick == _maxAttackTick) 
                {
                    _attackTick = 0;
                    _angle = 0;
                    _isAttack = false;
                } else
                {
                    _attackTick++;
                    float ratio = (float)_attackTick / _maxAttackTick;
                    if (ratio <= 1f / 8f)
                    {
                        _angle = -45 * ratio * 8;
                    }
                    else if (ratio <= 5f / 8f)
                    {
                        _angle = -45 + (ratio - 1f / 8f) * 180 * 2;
                    }
                    else
                    {
                        _angle = 180 - (ratio - 5f / 8f) * 135 * 8f / 3f;
                    }
                }
            }
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
                Console.WriteLine(currentTime);
                if (currentTime - _lastAttackTime >= _attackCooldown)
                {
                    _lastAttackTime = currentTime;
                    _isAttack = true;
                    _attackPosition = Attack(c, point2D, 0);
                    Effect? effect = effectFactory.Create(_effect, _attackPosition.X, _attackPosition.Y, c.FaceLeft);
                    if(effect == null)
                    {
                        return;
                    } else
                    {
                        
                        if (c.Type == CharacterType.Player)
                        {
                            Player p = (Player)c;
                            if(p.Energy - EnergyCost >= 0)
                            {
                                p.EnergyChanged(-EnergyCost);
                                effects.Add(effect);
                            }
                            
                        }
                    }
                } 
            }
        }
        public override Point2D Attack(Character c, Point2D point2D, int range)
        {
            if(point2D.X - c.X < 0)
            {
                _faceLeft = false;
            } else
            {
                _faceLeft = true;
            }
            _attackPosition.X = X;
            _attackPosition.Y = Y;
            return _attackPosition;            
        }
    }
}
