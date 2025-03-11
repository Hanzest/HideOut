using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        private int _animationTick;
        private int _bmpIndex;
        private int _width;
        private int _height;
        private int _roomNumber;
        protected bool _faceLeft;
        protected bool _enemyNearBy;
        public Character(string name, int health, CharacterType type, int width, int height)
        {
            _name = name;
            _health = health;
            _type = type;
            _maxHealth = health;
            _exist = true;
            _animationTick = 0;
            _bmpIndex = 1;
            _faceLeft = false;
            _width = width;
            _height = height;
            _enemyNearBy = false;
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
        public int BmpIndex
        {
            get { return _bmpIndex; }
        }
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
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
            set { _faceLeft = value; }
        }
        public bool EnemyNearBy
        {
            get { return _enemyNearBy; }
        }
        public int RoomNumber
        {
            get { return _roomNumber; }
        }
        public virtual void TakeDamage(int value, Point2D dir, Room[] rooms)
        {
            HealthChanged(-value);
        }
        public Point2D Coordinate()
        {
            Point2D point = new Point2D(X, Y);
            return point;
        }
        public void UpdateRoomNumber(Room[] rooms)
        {
            if(PositionValidation.CharacterInsideOneRoom(this, rooms))
            {
                _roomNumber = PositionValidation.RoomPosition(X, Y, rooms);
            }
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
                        if(!EnemyNearBy)
                            _faceLeft = true;   
                    }
                    break;
                case Direction.Right:
                    if(CheckValidMove(X + Velocity, Y, rooms, width, height))
                    {
                        X += Velocity;
                        if(!EnemyNearBy)
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
        public virtual void CollisionWithProjectile(HashSet<Projectile> projectiles, Room[] rooms)
        {
            foreach (Projectile p in projectiles)
            {
                if ((PositionValidation.PointInRotatedRectangle(X, Y, p.Angle, p.X, p.Y, p.Width, p.Height)
                    || PositionValidation.PointInRectangle(p.X, p.Y, X - Width / 2, X + Width / 2
                                                                   , Y - Height / 2, Y + Height / 2))
                    && p.ShootBy != Type && p.Damage > 0)
                {
                    Console.WriteLine($"{Name} was hit by {p.Name}");
                    Console.WriteLine($"{Name} has {Health} HP left");
                    if(p.Name != "shotgunBullet")
                    {
                        p.Collided = true;
                    }
                    Point2D point2D = new Point2D((float)Math.Cos(p.Angle), (float)Math.Sin(p.Angle));
                    TakeDamage(p.Damage, point2D, rooms);
                    //if (CheckValidMove(X + (float)Math.Cos(p.Angle) * 25, Y, rooms, Width, Height))
                    //{
                    //    X = X + (float)Math.Cos(p.Angle) * 25;
                    //}
                    if (CheckValidMove(X, Y + (float)Math.Sin(p.Angle) * 25, rooms, Width, Height))
                    {
                        Y = Y + (float)Math.Sin(p.Angle) * 25;
                    }
                    
                }
            }
        }
        public virtual void CollisionWithMeleeWeapon(HashSet<Item> items, Room[] rooms)
        {
            foreach (Item item in items)
            {

                if (item.Type == ItemType.MeleeWeapon)
                {
                    MeleeWeapon mWeapon = (MeleeWeapon)item;
                    if (mWeapon.IsAttack && mWeapon.Holder != Type)
                    {
                        //Console.WriteLine($"{Name} is attacked by {mWeapon.Name}");
                        int knockback = -30;
                        float mWeaponXpara = mWeapon.X;
                        if (mWeapon.FaceLeft)
                        {
                            knockback = 30;
                            mWeaponXpara -= 40 + mWeapon.Width / 2;
                        }
                        if (PositionValidation.RectangleToRectangle(X, Width, Y, Height, mWeaponXpara, mWeapon.Width / 2, mWeapon.Y, mWeapon.Height))
                        {
                            //if (CheckValidMove(X + knockback, Y, rooms, Width, Height))
                            //{
                            //   X += knockback;
                            //}
                            Point2D point2D = new Point2D(knockback, 0);
                            TakeDamage(mWeapon.Damage, point2D, rooms);
                            Console.WriteLine($"{Name} was hit by {mWeapon.Name}");
                            Console.WriteLine($"{Name} has {Health}HP left");
                        }
                    }
                }
            }
        }

        public virtual Point2D NearestEnemy(HashSet<Character> characters)
        {
            Point2D point2D = new Point2D(60, 0);
            if(FaceLeft)
            {
                point2D = Coordinate() - point2D;
            } else
            {
                point2D = Coordinate() + point2D;
            }
            float dist = 230400;
            _enemyNearBy = false;
            foreach (Character character in characters)
            {
                if (character.Type != Type)
                {
                    if ((character.X - X) * (character.X - X) + (character.Y - Y) * (character.Y - Y) < dist)
                    {
                        dist = (character.X - X) * (character.X - X) + (character.Y - Y) * (character.Y - Y);
                        point2D.X = character.X;
                        point2D.Y = character.Y;
                        _enemyNearBy = true;
                        if(X < character.X)
                        {
                            FaceLeft = false;
                        } else
                        {
                            FaceLeft = true;
                        }
                    }
                }
            }
            return point2D;
        }
        public virtual double NearestEnemyAngle(Point2D enemyPosition)
        {
            double angle = Math.Atan2(enemyPosition.Y - Y, enemyPosition.X - X);
            return angle;
        }
    }
}
