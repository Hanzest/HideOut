using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public abstract class Enemy : Character
    {
        private bool _isAttack;
        protected int _moveCounter;
        protected int _attackCounter;
        protected double _angle;
        protected Point2D _dir;
        protected float _rootVelocity;
        public Enemy(string name, int health, CharacterType type, float x, float y, float velocity, int width, int height) : base(name, health, type, width, height)
        {
            X = x;
            Y = y;
            _isAttack = false;
            _moveCounter = SplashKit.Rnd(50, 60);
            _attackCounter = 20;
            _dir = new Point2D(3f, 4f);
            Velocity = velocity;
            _rootVelocity = velocity;
        }
        public bool IsAttack
        {
            get { return _isAttack; }
            set { _isAttack = value; }
        }
        public Point2D Dir
        {
            get { return _dir; }
            set { _dir = value; }
        }

        public abstract void Move(Player p, Room[] rooms);
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
