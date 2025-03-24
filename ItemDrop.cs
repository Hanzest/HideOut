using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class ItemDrop
    {
        private PotionFactory _potionFactory;
        public ItemDrop() 
        {
            _potionFactory = new PotionFactory();
        }
        public void Drop(IItemFactory wFactory, HashSet<Item> items, Point2D point2D)
        {
            int rnd = SplashKit.Rnd(1, 11);
            float x = point2D.X;
            float y = point2D.Y;
            int willdrop = SplashKit.Rnd(0, 10);
            WeaponFactory weaponFactory = (WeaponFactory)wFactory;
            // Drop Rate: 30%;
            if (willdrop <= 2)
            {
                return;
            }
            if (rnd <= 4)
                // 50% drop potion
            {
                
                int subrnd1 = SplashKit.Rnd(0, 2);
                if(subrnd1 == 0)
                {
                    items.Add(_potionFactory.Create("Health potion", x, y));
                }
                else
                {
                    items.Add(_potionFactory.Create("Energy potion", x, y));
                }
            } else if(rnd <= 7)
            // 20% drop melee weapon
            {

                int subrnd2 = SplashKit.Rnd(0, 2);
                if(subrnd2 == 0)
                {
                    items.Add(weaponFactory.Create("Iron Sword", x, y));
                }
                else
                {
                    items.Add(weaponFactory.Create("Light Saber", x, y));
                }
            } else
            // 30% drop range weapon
            {
                int subrnd3 = SplashKit.Rnd(0, 3);
                switch (subrnd3)
                {
                    case 0:
                        items.Add(weaponFactory.Create("rifle", x, y));
                        break;
                    case 1:
                        items.Add(weaponFactory.Create("sniper", x, y));
                        break;
                    case 2:
                        items.Add(weaponFactory.Create("sawed-off shotgun", x, y));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
