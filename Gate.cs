using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Gate : Item
    {
        private bool _isPlayerInteract;
        public Gate(string name, float x, float y) : base(ItemType.Gate, name, false, x, y)
        {
            _isPlayerInteract = false;
        }
        public void Interact(Player p)
        {
            if(Name == "OutGate")
            {
                _isPlayerInteract = true;
            }
        }
        public bool IsPlayerInteract
        {
            get { return _isPlayerInteract; }
        }
    }
}
