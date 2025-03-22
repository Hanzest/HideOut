using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class EnemyFactory : ICharacterFactory
    {
        private int _decreaseHP;
        private int _decreaseSpeed;
        private int _decreaseDamage;
        public EnemyFactory()
        {

        }
        public Character? Create(string name, float x, float y)
        {
            RangeWeaponFactory rWFactory = new RangeWeaponFactory();
            Item revolver = rWFactory.Create("revolver", -1000f, 0f);
            Item brokenGlass = rWFactory.Create("Broken Glass", -1000f, 0f);
            Item fireBeam = rWFactory.Create("fireBeam", -1000f, 0f);
            Item snipezooka = rWFactory.Create("snipezooka", -1000f, 0f);
            switch (name)
            {
                // theme 1: old castle
                case "spider":
                    return new MeleeEnemy(name, 100 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 8f - _decreaseSpeed * 2, 64, 50, "scratch", 10, 2.25f);
                case "scorpio":
                    return new MeleeEnemy(name, 200 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 8f - _decreaseSpeed * 1.5f, 100, 60, "cut", 15, 3f);
                case "shadow":
                    return new MeleeEnemy(name, 150 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 6f - _decreaseSpeed * 2, 80, 20, "cut", 15, 3f);
                case "zombie":
                    RangeEnemy rangeEnemy1 = new RangeEnemy(name, 300 - _decreaseHP * 10, CharacterType.RangeEnemy, x, y, 2.5f - _decreaseSpeed, 96, 128);
                    rangeEnemy1.Inventory.Add(revolver, rangeEnemy1);
                    return rangeEnemy1;
                // theme 2: grassy
                case "mantis":
                    return new MeleeEnemy(name, 250 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 6f - _decreaseSpeed * 2, 80, 80, "scratch", 12, 2.75f);
                case "butterfly":
                    return new MeleeEnemy(name, 100 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 8f - _decreaseSpeed * 2, 60, 60, "cut", 10, 2.75f);
                case "diamond":
                    RangeEnemy rangeEnemy2 = new RangeEnemy(name, 300 - _decreaseHP * 10, CharacterType.RangeEnemy, x, y, 2.5f, 64, 64);
                    rangeEnemy2.Inventory.Add(brokenGlass, rangeEnemy2);
                    return rangeEnemy2;
                // theme 3: dark castle
                case "ghost":
                    return new MeleeEnemy(name, 200 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 8f - _decreaseSpeed * 2, 48, 48, "scratch", 18, 2.25f);
                case "spike":
                    return new MeleeEnemy(name, 250 - _decreaseHP * 10, CharacterType.MeleeEnemy, x, y, 12f - _decreaseSpeed * 2, 24, 24, "cut", 6, 2.25f);
                case "rod":
                    RangeEnemy rangeEnemy3 = new RangeEnemy(name, 300 - _decreaseHP * 10, CharacterType.RangeEnemy, x, y, 0f, 32, 128);
                    rangeEnemy3.Inventory.Add(fireBeam, rangeEnemy3);
                    return rangeEnemy3;
                case "scientist":
                    RangeEnemy rangeEnemy4 = new RangeEnemy(name, 250 - _decreaseHP * 10, CharacterType.RangeEnemy, x, y, 4f - _decreaseSpeed, 48, 64);
                    rangeEnemy4.Inventory.Add(snipezooka, rangeEnemy4);
                    return rangeEnemy4;
                default:
                    return null;
            }
        }
        public void UpgradeDecreaseHP()
        {
            _decreaseHP++;
        }
        public void UpgradeDecreaseSpeed()
        {
            _decreaseSpeed++;
        }
        public void UpgradeDecreaseDamage()
        {
            _decreaseDamage++;
        }
        public int DecreaseHP
        {
            get { return _decreaseHP; }
        }
        public int DecreaseSpeed
        {
            get { return _decreaseSpeed; }
        }
        public int DecreaseDamage
        {
            get { return _decreaseDamage; }
        }
    }
}
