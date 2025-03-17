using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class RewardFactory : IItemFactory
    {
        public RewardFactory() 
        {}
        public Item? Create(string name, float x, float y)
        {
            int rnd1 = SplashKit.Rnd(-24, 24);
            int rnd2 = SplashKit.Rnd(-24, 24);
            x += rnd1;
            y += rnd2;
            switch (name)
            {
                case "Coin":
                    return new Reward(ItemType.Reward, name, x, y);
                case "Energy Particle":
                    return new Reward(ItemType.Reward, name, x, y);
                default:
                    return null;
            }
        }
    }
}
