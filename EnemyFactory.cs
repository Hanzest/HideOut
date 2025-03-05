using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class EnemyFactory : ICharacterFactory
    {
        public Character Create(string name, int health, CharacterType type, int x, int y, float velocity)
        {
            return new Enemy(name, health, type, x, y, velocity);
        }
    }
}
