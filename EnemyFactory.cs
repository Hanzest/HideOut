using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class EnemyFactory : ICharacterFactory
    {
        public Character? Create(string name, float x, float y)
        {
            RangeWeaponFactory rWFactory = new RangeWeaponFactory();
            Item revolver = rWFactory.Create("revolver", -1000f, 0f);
            switch (name)
            {
                case "spider":
                    return new MeleeEnemy(name, 100, CharacterType.MeleeEnemy, x, y, 8f, 64, 50, "scratch", 10, 2.25f);
                case "mantis":
                    return new MeleeEnemy(name, 250, CharacterType.MeleeEnemy, x, y, 6f, 80, 80, "scratch", 12, 2.75f);
                case "zombie":
                    RangeEnemy rangeEnemy = new RangeEnemy(name, 800, CharacterType.RangeEnemy, x, y, 2.5f, 96, 128);
                    rangeEnemy.Inventory.Add(revolver, rangeEnemy);
                    return rangeEnemy;
                default:
                    return null;
            }
        }
    }
}
