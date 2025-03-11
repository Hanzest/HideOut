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

        public void UseBy(Character c)
        {
            Player p = (Player)c;
        }
    }
}
