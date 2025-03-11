using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class PlayerFactory : ICharacterFactory
    {
        public Character? Create(string name, float x, float y)
        {
            switch (name)
            {
                case "alchemist":
                    return new Player(name, 150, 200, 60, CharacterType.Player, x, y, 10f, 96, 128);
                default:
                    return null;
            }
            
        }
    }
}
