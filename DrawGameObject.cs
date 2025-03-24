using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class DrawGameObject
    {

        private List<Bitmap> _bitmap;
        private string _pathCharacter;
        private string _pathEffect;
        private string _pathItem;
        private string _pathProjectile;
        public DrawGameObject(string pathCharacter, string pathEffect, string pathItem, string pathProjectile)
        {
            _bitmap = new List<Bitmap>();
            _pathCharacter = pathCharacter;
            _pathEffect = pathEffect;
            _pathItem = pathItem;
            _pathProjectile = pathProjectile;
        }
        public DrawGameObject()
        {
            _bitmap = new List<Bitmap>();
        }
        public Bitmap? GetBitmap(string name)
        {
            return _bitmap.Find(bmp => bmp.Name == name);
        }
        public void AddBitmap(string name, ClassType classType)
        {
            switch (classType)
            {
                case ClassType.Character:
                    for (int i = 1; i <= 4; i++)
                    {
                        _bitmap.Add(new Bitmap($"{name}{i}",
                                    $"{_pathCharacter}{name}{i}.png"));
                    }
                    break;
                case ClassType.Effect:
                    int q = 4;
                    if (name == "slash" || name == "paleslash")
                        q = 8;
                    for (int i = 1; i <= q; i++)
                    {
                        _bitmap.Add(new Bitmap($"{name}{i}", $"{_pathEffect}{name}{i}.png"));
                    }
                    break;
                case ClassType.Item:
                    _bitmap.Add(new Bitmap(name, $"{_pathItem}{name}.png"));
                    if(name != "Energy potion" && name != "Health potion" && name != "Broken Glass" && name != "InGate" && name != "OutGate")
                    {
                        _bitmap.Add(new Bitmap($"flip{name}", $"{_pathItem}flip{name}.png"));
                    }
                    break;
                case ClassType.Projectile:
                    _bitmap.Add(new Bitmap(name, $"{_pathProjectile}{name}.png"));
                    break;
                default:
                    break;
            }
            
        }
        public void Draw(HashSet<Character> _character)
        {
            foreach (Character c in _character)
            {
                Bitmap bmp = GetBitmap($"{c.Name}{c.BmpIndex}");
                if (bmp != null)
                {
                    if (c.FaceLeft)
                    {
                        bmp.DrawFlippedY(c.X - bmp.Width / 2, c.Y - bmp.Height / 2); // centering image with X, Y
                    }
                    else
                    {
                        bmp.Draw(c.X - bmp.Width / 2, c.Y - bmp.Height / 2); // centering image with X, Y
                    }
                }
            }
        }
        public void Draw(HashSet<Effect> effects)
        {
            foreach (Effect effect in effects)
            {
                Bitmap bmp = GetBitmap($"{effect.Name}{effect.Index()}");
                if (bmp == null)
                {
                    Console.WriteLine($"{effect.Name}{effect.Index()}");
                }
                if (bmp != null)
                {
                    if (effect.FaceLeft)
                    {
                        bmp.DrawFlippedY(effect.X - bmp.Width / 2, effect.Y - bmp.Height / 2);
                    }
                    else
                    {
                        bmp.Draw(effect.X - bmp.Width / 2, effect.Y - bmp.Height / 2);
                    }
                }
            }
        }
        public void Draw(HashSet<Item> items, double angle)
        {
            foreach (Item item in items)
            {
                if (item.Display)
                {
                    Bitmap bmp = GetBitmap(item.Name);
                    
                    if (bmp != null)
                    {
                        
                        if (item.InInventory)
                        {
                            switch (item.Type)
                            {
                                case ItemType.RangeWeapon:
                                    // Fix Dir: where the item pointed at
                                    if (Math.Abs(item.Angle) < 1.57)
                                    {
                                        bmp.DrawRotated(item.X - bmp.Width / 2, item.Y - bmp.Height / 2, item.Angle);
                                    }
                                    else
                                    {
                                        bmp = GetBitmap("flip" + item.Name);
                                        if (bmp != null)
                                            bmp.DrawRotated(item.X - bmp.Width / 2, item.Y - bmp.Height / 2, -Math.PI + item.Angle);
                                    }
                                    break;
                                case ItemType.MeleeWeapon:
                                    MeleeWeapon mWeapon = (MeleeWeapon)item;
                                    if (Math.Abs(item.Angle) < 1.57)
                                    {
                                        bmp.DrawRotated(item.X - bmp.Width / 2, item.Y - bmp.Height / 2, mWeapon.Angle);
                                    }
                                    else
                                    {
                                        bmp = GetBitmap("flip" + item.Name);
                                        if (bmp != null)
                                            bmp.DrawRotated(item.X - bmp.Width / 2, item.Y - bmp.Height / 2, - mWeapon.Angle);
                                    }
                                    break;
                            }
                            
                            
                        } else
                        {
                            if(item.Type != ItemType.Gate)
                            {
                                bmp.Draw(item.X - bmp.Width / 2, item.Y - bmp.Height / 2);
                            }
                        }
                    }
                }
            }
        }
        public void Draw(HashSet<Projectile> projectiles)
        {
            foreach (Projectile p in projectiles)
            {
                Bitmap projectile = GetBitmap(p.Name);
                if (projectile != null)
                {
                    projectile.DrawRotated(p.X - p.Width/2, p.Y - p.Height/2, p.Angle);
                }
            }
        }

        public void DrawPrioritizedItem(Item item, double angle)
        {
            HashSet<Item> items = new HashSet<Item>();
            items.Add(item);
            Draw(items, item.Angle);
        }
        public void DrawStaticItem(Item item)
        {
            Bitmap bmp = GetBitmap(item.Name);
            if (bmp != null)
            {
                bmp.Draw(item.X - bmp.Width / 2, item.Y - bmp.Height / 2);
            }
        }
        public void SetPath(string pathCharacter, string pathEffect, string pathItem, string pathProjectile)
        {
            _pathCharacter = pathCharacter;
            _pathEffect = pathEffect;
            _pathItem = pathItem;
            _pathProjectile = pathProjectile;
        }
    }
}
