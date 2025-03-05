using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _name;
        private bool _exist;
        public Item(ItemType type, string name, bool inInventory, float x, float y)
        {
            _type = type;
            _name = name;
            _inInventory = inInventory;
            _x = x;
            _y = y;
            _exist = true;
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
        }
        public float Y
        {
            get { return _y; }
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
