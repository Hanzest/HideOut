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
            Item brokenGlass = rWFactory.Create("Broken Glass", -1000f, 0f);
            switch (name)
            {
                // theme 1: old castle
                case "spider":
                    return new MeleeEnemy(name, 100, CharacterType.MeleeEnemy, x, y, 8f, 64, 50, "scratch", 10, 2.25f);
                case "scorpio":
                    return new MeleeEnemy(name, 200, CharacterType.MeleeEnemy, x, y, 4f, 100, 60, "cut", 15, 3f);
                case "shadow":
                    return new MeleeEnemy(name, 150, CharacterType.MeleeEnemy, x, y, 6f, 80, 20, "cut", 15, 3f);
                case "zombie":
                    RangeEnemy rangeEnemy1 = new RangeEnemy(name, 300, CharacterType.RangeEnemy, x, y, 2.5f, 96, 128);
                    rangeEnemy1.Inventory.Add(revolver, rangeEnemy1);
                    return rangeEnemy1;
                // theme 2: grassy
                case "mantis":
                    return new MeleeEnemy(name, 250, CharacterType.MeleeEnemy, x, y, 6f, 80, 80, "scratch", 12, 2.75f);
                case "butterfly":
                    return new MeleeEnemy(name, 100, CharacterType.MeleeEnemy, x, y, 8f, 60, 60, "cut", 10, 2.75f);
                case "diamond":
                    RangeEnemy rangeEnemy2 = new RangeEnemy(name, 300, CharacterType.RangeEnemy, x, y, 2.5f, 64, 64);
                    rangeEnemy2.Inventory.Add(brokenGlass, rangeEnemy2);
                    return rangeEnemy2;
                default:
                    return null;
            }
        }
    }
}
