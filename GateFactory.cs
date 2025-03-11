using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class GateFactory : IItemFactory
    {
        public GateFactory() { }
        public Item? Create(string name, float x, float y)
        {
            switch (name)
            {
                case "InGate":
                    return new Gate(name, x, y);
                case "OutGate":
                    return new Gate(name, x, y);
                default:
                    return null;
            }
        }
    }
}
