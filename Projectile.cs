using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Projectile
    {
        private string _collision;
        private string _name;
        private float _x;
        private float _y;
        private float _speed;
        private double _angle;
        private float _dx;
        private float _dy;
        private int _damage;
        private bool _collided;
        private bool _isCritical;
        public Projectile(string collision, string name, float x, float y, float speed, int damage , bool isCritical, float ex, float ey)
        {
            _collision = collision;
            _name = name;
            _x = x;
            _y = y;
            _speed = speed;
            _damage = damage;
            _isCritical = isCritical;
            _angle = Math.Atan2(ey - y, ex - x);
            _dx = (float)Math.Cos(_angle) * speed;
            _dy = (float)Math.Sin(_angle) * speed;
        }
        public string Collision
        {
            get { return _collision; }
        }
        public string Name
        {
            get { return $"{_name}Bullet"; }
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
            // _angle is in radians
            // return type is degrees.
        {
            get { return _angle * 180 / Math.PI; }
        }
        public float Damage
        {
            get { return _damage; }
        }
        public bool Collided
        {
            get { return _collided; }
        }
        public void Move(Room[] rooms)
        {
            // Slightly up for 8 pixel for 3D effects
            if(CheckValidMove(_x + _dx, _y - 8 + _dy, rooms))
            {
                _x += _dx;
                _y += _dy;
            }
            else
            {
                _collided = true;
            }
        }
        public bool CheckValidMove(float x, float y, Room[] rooms)
        {
            return PositionValidation.CheckValid(x, y, rooms);
        }
    }
}
