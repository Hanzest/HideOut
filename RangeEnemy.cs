using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class RangeEnemy : Enemy
    {
        private Inventory _inventory;
        public RangeEnemy(string name, int health, CharacterType type, float x, float y, float velocity, int width, int height)
                            :base(name, health, type, x, y, velocity, width, height)
        {
            _inventory = new Inventory(1);
        }
        public Inventory Inventory
        {
            get { return _inventory; }
        }
        public override void FindPlayerNearby(Player p, Room[] rooms)
        {
            IsAttack = false;
            if ((Math.Abs(p.X - X) <= 18 * rooms[0].TileSize) && (Math.Abs(p.Y - Y) <= 18 * rooms[0].TileSize) 
                && p.RoomIndex == RoomIndex)
            {
                Move(p, rooms);
                IsAttack = true;
            }
        }
        public override void Move(Player p, Room[] rooms)
        {
            _moveCounter--;
            if (CheckValidMove(X - _dir.X, Y , rooms, Width, Height))
            {
                if (_dir.X > 0)
                {
                    _faceLeft = false;
                }
                else
                {
                    _faceLeft = true;
                }
                X -= _dir.X;
                
                Tick();
            }
            if(CheckValidMove(X, Y - _dir.Y, rooms, Width, Height))
            {
                Y -= _dir.Y;
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
