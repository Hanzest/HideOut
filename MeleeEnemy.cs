using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class MeleeEnemy : Enemy
    {
        private string _attackType;
        private int _damage;
        private float _range;
        public MeleeEnemy(string name, int health, CharacterType type, float x, float y, float velocity, int width, int height, string attackType, int damage, float range) 
            : base(name, health, type, x, y, velocity, width, height)
        {
            _attackType = attackType;
            _damage = damage;
            _range = range;
        }
        public void FindPlayerNearby(Player p, Room[] rooms)
        {
            if ((Math.Abs(p.X - X) <= 15 * rooms[0].TileSize) && (Math.Abs(p.Y - Y) <= 15 * rooms[0].TileSize) && p.RoomIndex == this.RoomIndex)
            {
                IsAttack = false;
                Move(p, rooms);
                if ((Math.Abs(p.X - X) <= _range * rooms[0].TileSize) && (Math.Abs(p.Y - Y) <= _range * rooms[0].TileSize))
                {
                    _attackCounter--;
                    if (_attackCounter <= 0)
                    {
                        p.TakeDamage(_damage, Dir, rooms);
                        IsAttack = true;
                        _attackCounter = 20;
                    }
                }
            }
        }

        public override void Move(Player p, Room[] rooms)
        {
            _moveCounter--;
            if (CheckValidMove(X + _dir.X, Y + _dir.Y, rooms, Width, Height))
            {
                if (_dir.X > 0)
                {
                    _faceLeft = false;
                }
                else
                {
                    _faceLeft = true;
                }
                X += _dir.X;
                Y += _dir.Y;
                Tick();
            }
            if (_moveCounter <= 0)
            {
                if (SplashKit.Rnd(0, 10) >= 2)
                {
                    Velocity = _rootVelocity * 1.8f;
                    _angle = Math.Atan2(p.Y - Y, p.X - X);
                    _dir.X = (float)Math.Cos(_angle) * Velocity;
                    _dir.Y = (float)Math.Sin(_angle) * Velocity;
                    _moveCounter = SplashKit.Rnd(40, 60);
                }
                else
                {
                    Velocity = _rootVelocity;
                    SetRandomDirection(rooms);
                    _moveCounter = SplashKit.Rnd(20, 40);
                }
            }
        }
    }
}
