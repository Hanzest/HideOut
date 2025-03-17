using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Reward : Item
    {
        public Reward(ItemType type, string name, float x, float y) : base(type, name, false, x, y)
        {

        }

        public void MoveTowardsPlayer(Character c)
        {
            if((c.X - X) * (c.X - X) + (c.Y - Y) * (c.Y - Y) < 225792)
                // Radius of 7 blocks
            {
                Angle = Math.Atan2(c.Y - Y, c.X - X);
                X += (float)Math.Cos(Angle) * 12f;
                Y += (float)Math.Sin(Angle) * 12f;
                if((c.X - X) * (c.X - X) + (c.Y - Y) * (c.Y - Y) < 10368)
                    // Radius of 1.5 blocks
                {
                    Exist = false;
                }
            }
        }
    }
}
