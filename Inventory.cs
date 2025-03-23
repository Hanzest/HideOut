using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Inventory
    {
        private int _index;
        private int _maxSize;
        private List<Item> _items;
        public Inventory(int maxSize)
        {
            _index = -1;
            _maxSize = maxSize;
            _items = new List<Item>();
        }
        public void Add(Item item, Character c)
        {
            
            if(_items.Count == _maxSize)
            {
                GetItem.Display = true;
                GetItem.InInventory = false;
                _items.Remove(GetItem);
                _items.Add(item);
                return;
            } else
            {
                if(c.Type == CharacterType.RangeEnemy)
                {
                    RangeWeapon rangeWeapon = (RangeWeapon)item;
                    rangeWeapon.AttackCoolDown *= 2;
                }
                _items.Add(item);
                item.InInventory = true;
                IncrementIndex();
            }
        }
        public void UpdateItemPosition(Character c)
        {
            foreach (Item item in _items)
            {
                if(item != GetItem)
                {
                    item.Display = false;
                } else
                {
                    item.Display = true;
                }
                    switch (item.Type)
                    {
                        case ItemType.MeleeWeapon:
                            MeleeWeapon mWeapon = (MeleeWeapon)item;
                            mWeapon.AttackTick();
                            if (c.FaceLeft)
                                mWeapon.X = c.X - 20;
                            else
                                mWeapon.X = c.X + 20;
                            mWeapon.Y = c.Y + 20;
                            break;
                        case ItemType.RangeWeapon:
                            item.X = c.X;
                            item.Y = c.Y + 20;
                            break;
                    }
            }
        }
        public Item GetItem
        {
            get {
                if(_items.Count == 0)
                {
                    return null;
                }
                return _items[_index]; 
            }
        }
        public Item GetItemByIndex(int index)
        {
            if(index < 0 || index >= _items.Count)
            {
                return null;
            }
            return _items[index];
        }
        public int Count
        {
            get { return _items.Count; }
        }

        public void IncrementIndex()
        {
            if(_items.Count == 0)
            {
                Index = 0;
            } else
            {
                Index = (Index + 1) % _items.Count;
            }
        }
        public void DecrementIndex()
        {
            Index = (Index - 1) % _items.Count;
        }
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
