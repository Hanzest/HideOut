using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public abstract class Item : IGameObject
    {
        private ItemType _type;
        private float _x;
        private float _y;
        private bool _inInventory;
        private bool _display;
        private string _name;
        private bool _exist;
        private double _angle;
        protected bool _faceLeft;
        public Item(ItemType type, string name, bool inInventory, float x, float y)
        {
            _type = type;
            _name = name;
            _display = true;
            _inInventory = inInventory;
            _x = x;
            _y = y;
            _angle = 0;
            _exist = true;
            _faceLeft = false;
        }
        public ItemType Type
        {
            get { return _type; }
        }
        public string Name
        {
            get { return _name; }
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
        public bool Display
        {
            get { return _display; }
            set { _display = value; }
        }
        public bool Exist
        {
            get { return _exist; }
            set { _exist = value; }
        }
        public bool InInventory
        {
            get { return _inInventory; }
            set { _inInventory = value; }
        }
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }
        public bool FaceLeft
        {
            get { return _faceLeft; }
            set { _faceLeft = value; }
        }
        public Point2D Coordinate()
        {
            Point2D point = new Point2D(X, Y);
            return point;
        }
        public virtual bool NearByPlayer(Player p, int tileSize)
        {
            Point2D point = Coordinate() - p.Coordinate();
            if((Math.Abs(point.X) <= 2.5f * tileSize) && (Math.Abs(point.Y) <= 2.5f * tileSize))
            {
                return true;
            }
            return false;
        }
    }
}
