using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class Enemy : Character
    {
        private Point2D _dir;
        private float _rootVelocity;
        private bool _isAttack;
        private int _attackCounter;
        private int _moveCounter;
        private int _movePhase; // 0: Random, 1: Attack
        private int _width;
        private int _height;
        private double _angle;
        public Enemy(string name, int health, CharacterType type, int x, int y, float velocity) : base(name, health, type)
        {
            X = x;
            Y = y;
            _isAttack = false;
            _moveCounter = SplashKit.Rnd(50, 60);
            _movePhase = 0;
            _attackCounter = 20;
            _dir = new Point2D(3f, 4f);
            Velocity = velocity;
            _rootVelocity = velocity;
            _width = 64;
            _height = 50;
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
        public bool IsAttack
        {
            get { return _isAttack; }
        }
        public Point2D Dir
        {
            get { return _dir; }
            set { _dir = value; }
        }

        public void FindPlayerNearby(Player p, Room[] rooms)
        {
            if((Math.Abs(p.X - X) <= 10 * rooms[0].TileSize) && (Math.Abs(p.Y - Y) <= 10 * rooms[0].TileSize))
            {
                _isAttack = false;
                Move(p, rooms);
                if((Math.Abs(p.X - X) <= 2.25f * rooms[0].TileSize) && (Math.Abs(p.Y - Y) <= 2.25f * rooms[0].TileSize))
                {
                    _attackCounter--;
                    if(_attackCounter <= 0)
                    {
                        p.TakeDamage(10, Dir, rooms);
                        _isAttack = true;
                        _attackCounter = 20;
                    }
                }
            }
        }

        public void Move(Player p, Room[] rooms)
        {
            _moveCounter--;
            if (CheckValidMove(X + _dir.X, Y + _dir.Y, rooms, _width, _height))
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
                    _movePhase = 1;
                    _moveCounter = SplashKit.Rnd(40, 60);
                }
                else
                {
                    Velocity = _rootVelocity;
                    SetRandomDirection(rooms);
                    _movePhase = 0;
                    _moveCounter = SplashKit.Rnd(20, 40);
                }
            }
        }
        public Point2D SetRandomDirection(Room[] rooms)
        {
            float a = SplashKit.Rnd(-314, 314) / 100;
            float b = SplashKit.Rnd(-314, 314) / 100;
            _dir.X = (float)Math.Cos(a)*Velocity;
            _dir.Y = (float)Math.Sin(b)*Velocity;
            return _dir;
        }
    }
}
