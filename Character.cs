using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public abstract class Character : IGameObject
    {
        //  x, y: Coordinate
        //  id: Team selection / Race selection
        //  health: Health of the character
        //  exist: Check if the character is still alive
        //  velocity: Speed of the character
        //  path: Path to the character's image
        //  animationTick: Tick for animation
        private float _x;
        private float _y;
        private string _name;
        private CharacterType _type;
        private int _health;
        private int _maxHealth;
        private bool _exist;
        private float _velocity;
        private string _path;
        private int _animationTick;
        private int _bmpIndex;
        protected bool _faceLeft;
        public Character(string name, int health, CharacterType type)
        {
            _name = name;
            _health = health;
            _type = type;
            _maxHealth = health;
            _exist = true;
            _path = $"{name}.png";
            _animationTick = 0;
            _bmpIndex = 1;
            _faceLeft = false;
        }
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public int Health
        {
            get { return _health; }
        }
        public CharacterType Type
        {
            get {return _type; }    
        }
        public bool Exist
        {
            get { return _exist; }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }
        public float Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public int BmpIndex
        {
            get { return _bmpIndex; }
        }
        public void Tick()
        {
            _animationTick = (_animationTick + 1) % 10;
            if (_animationTick == 0)
            {
                _bmpIndex = _bmpIndex % 4 + 1;
            }
        }
        public void HealthChanged(int change)
        {
            if(_health + change > _maxHealth)
            {
                _health = _maxHealth;
            }
            else
            {
                _health += change;
            }
            if (_health <= 0)
            {
                _health = 0;
                _exist = false;
            }
        }
        public bool FaceLeft
        {
            get { return _faceLeft; }
        }
        public Point2D Coordinate()
        {
            Point2D point = new Point2D(X, Y);
            return point;
        }
        public virtual void Move(Direction d, Room[] rooms, int width, int height)
        {
            switch (d)
            {
                case Direction.Up:
                    if(CheckValidMove(X, Y - Velocity, rooms, width, height))
                    {
                        Y -= Velocity;
                    }
                    break;
                case Direction.Down:
                    if (CheckValidMove(X, Y + Velocity, rooms, width, height))
                    {
                        Y += Velocity;
                    }
                    break;
                case Direction.Left:
                    if(CheckValidMove(X - Velocity, Y, rooms, width, height))
                    {
                        X -= Velocity;
                        _faceLeft = true;
                    }
                    break;
                case Direction.Right:
                    if(CheckValidMove(X + Velocity, Y, rooms, width, height))
                    {
                        X += Velocity;
                        _faceLeft = false;
                    }
                    break;
            }
        }
        public virtual bool CheckValidMove(float posx, float posy, Room[] rooms, int width, int height)
        {
            bool a = PositionValidation.CheckValid(posx - width / 2, posy - (float)(height / 8), rooms);
            bool b = PositionValidation.CheckValid(posx + width / 2, posy - (float)(height / 8), rooms);
            bool c = PositionValidation.CheckValid(posx - width / 2, posy + (float)(height / (8f / 3f)), rooms);
            bool d = PositionValidation.CheckValid(posx + width / 2, posy + (float)(height / (8f / 3f)), rooms);
            //      .       .       .
            //      .       x       .
            //      .       .       .
            bool e = PositionValidation.CheckValid(posx, posy - (float)(height / 8), rooms);
            bool f = PositionValidation.CheckValid(posx, posy + (float)(height / (8f / 3f)), rooms);
            bool g = PositionValidation.CheckValid(posx - width / 2, posy, rooms);
            bool h = PositionValidation.CheckValid(posx + width / 2, posy, rooms);
            if (a && b && c && d && e && f && g && h)
            {
                return true;
            }
            return false;
        }
    }
}
