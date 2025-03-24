using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Projectile : IGameObject
    {
        private string _collision;
        private string _name;
        private float _x;
        private float _y;
        private float _velocity;
        private double _angle;
        private float _dx;
        private float _dy;
        private int _damage;
        private bool _collided;
        private bool _isCritical;
        private int _tick;
        private bool _exist;
        private CharacterType _shootBy;
        private int _width;
        private int _height;
        public Projectile(string collision, string name, float x, float y, float velocity, int damage , bool isCritical, double angle, int weaponWidth, CharacterType shootBy, int width, int height, bool isSameRoom)
        {
            _collision = collision;
            _name = name;
            _velocity = velocity;
            _damage = damage;
            _isCritical = isCritical;
            _angle = angle;
            _dx = (float)Math.Cos(_angle) * velocity;
            _dy = (float)Math.Sin(_angle) * velocity;
            if (collision == "null")
            {
                _tick = 6;
            }
            else
            {
                _tick = -1;
            }
            _x = x + weaponWidth * (float)Math.Cos(_angle);
            _y = y + weaponWidth * (float)Math.Sin(_angle);
            _shootBy = shootBy;
            _width = width;
            _height = height;
            if (isSameRoom == false)
            {
                _damage = 0;
            }
            _exist = true;

        }
        public string Collision
        {
            get { return _collision; }
        }
        public string Name
        {
            get { return $"{_name}"; }
        }
        public float X
        {
            get { return _x; }
        }
        public float Y
        {
            get { return _y; }
        }
        public double Angle
            // Angle is in Radian
        {
            get { return _angle; }
        }
        public int Damage
        {
            get { return (_isCritical) ? _damage : _damage * 2; }
            set {  _damage = value; }
        }
        public bool Collided
        {
            get { return _collided; }
            set { _collided = value; }
        }
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        public CharacterType ShootBy
        {
            get { return _shootBy; }
        }
        public Point2D Coordinate()
        {
            return new Point2D(_x, _y);
        }
        public void Move(Room[] rooms)
        {
            // Slightly up for 8 pixel for 3D effects
            if(_tick < 0)
            {
                if (CheckValidMove(_x + _dx, _y - 8 + _dy, rooms))
                {
                    _x += _dx;
                    _y += _dy;
                }
                else
                {
                    _collided = true;
                }
            } else
            {
                _tick--;
                if(_tick == 0) {
                    _collided = true;
                }
            }
            
        }
        public bool CheckValidMove(float x, float y, Room[] rooms)
        {
            return PositionValidation.CheckValid(x, y, rooms);
        }
    }
}
