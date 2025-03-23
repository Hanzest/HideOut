using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Loader
    {
        private StreamReader _reader;
        public Loader() { }
        public void LoadResource(DrawGameObject dgObject, DrawMap dMap, DrawStatusBoard dsBoard, Spawner spawner, int theme)
        {
            _reader = new StreamReader("Resource\\SetUp.txt");
            try
            {
                string charSource = _reader.ReadLine();
                string effSource = _reader.ReadLine();
                string itemSource = _reader.ReadLine();
                string projSource = _reader.ReadLine();
                string themeSource = _reader.ReadLine();
                string iconSource = _reader.ReadLine();
                ClassType type;
                dgObject.SetPath(charSource, effSource, itemSource, projSource);
                List<List<string>> lls = new List<List<string>>();
                for (int i = 1; i <= 6; i++)
                {
                    int cnt = _reader.ReadInteger();
                    switch (i)
                    {
                        case 1:
                            type = ClassType.Character;
                            break;
                        case 2:
                            type = ClassType.Effect;
                            break;
                        case 3:
                            type = ClassType.Item;
                            break;
                        case 4:
                            type = ClassType.Projectile;
                            break;
                        default:
                            type = ClassType.Null;
                            break;
                    }
                    if (i <= 4)
                    {
                        for (int j = 0; j < cnt; j++)
                        {
                            string name = _reader.ReadLine();
                            dgObject.AddBitmap(name, type);
                        }
                    }
                    else if (i == 5)
                    {
                        // Default theme
                        dMap.SetPath(themeSource, theme);
                        for (int j = 0; j < cnt; j++)
                        {
                            int cntls = _reader.ReadInteger();
                            List<string> ls = new List<string>();
                            for (int k = 0; k < cntls; k++)
                            {
                                string temps = _reader.ReadLine();
                                ls.Add(temps);
                            }
                            lls.Add(ls);
                        }
                        Console.WriteLine(lls.Count);
                        spawner.ThemeListEnemy = lls;

                    }
                    else if (i == 6)
                    {
                        dsBoard.SetPath(iconSource);
                    }
                }
            }
            finally
            {
                _reader.Close();
            }
        }
        public void LoadSaveGame( ref Player p, ref Saver s, ref ICharacterFactory pFactory, ref ICharacterFactory eFactory
                                , ref BuffManager buffManager, ref IItemFactory wFactory, ref HashSet<Item> items)
        {
            string temp;
            EnemyFactory enemyFactory = (EnemyFactory)eFactory;
            PlayerFactory playerFactory = (PlayerFactory)pFactory;
            WeaponFactory weaponFactory = (WeaponFactory)wFactory;
            _reader = new StreamReader("Resource\\SaveGame.txt");
            try
            {
                Console.WriteLine("Loading Save Game...");
                temp = _reader.ReadLine();
                if(temp == "yes")
                {
                    for(int i = 0; i < 7; i++)
                    {
                        int value = _reader.ReadInteger();
                        buffManager.SetBuffIndex(i, value);
                    }
                    // Player's buff
                    playerFactory.BonusHP = buffManager.GetBuffIndex(0);
                    playerFactory.BonusEnergy = buffManager.GetBuffIndex(1);
                    playerFactory.BonusArmor = buffManager.GetBuffIndex(2);
                    playerFactory.BonusSpeed = buffManager.GetBuffIndex(3);
                    Console.WriteLine($"Load Player's Buff: {playerFactory.BonusHP} {playerFactory.BonusEnergy} {playerFactory.BonusArmor} {playerFactory.BonusSpeed}");
                    pFactory = playerFactory;
                    
                    // Enemy's debuff
                    enemyFactory.DecreaseHP = buffManager.GetBuffIndex(4);
                    enemyFactory.DecreaseSpeed = buffManager.GetBuffIndex(5);
                    enemyFactory.DecreaseDamage = buffManager.GetBuffIndex(6);
                    Console.WriteLine($"Load Enemy's Debuff: {enemyFactory.DecreaseHP} {enemyFactory.DecreaseSpeed} {enemyFactory.DecreaseDamage}");
                    eFactory = enemyFactory;
                    
                    // Basic Information
                    s.Level = _reader.ReadInteger();
                    p.Name = _reader.ReadLine();
                    Player newPlayer = (Player)playerFactory.Create(p.Name, 200, 0);
                    p = newPlayer;
                    p.Health = _reader.ReadInteger();
                    p.Coin = _reader.ReadInteger();
                    p.Energy = _reader.ReadInteger();
                    Console.WriteLine($"Loaded: {p.Name} {p.Health} {p.Energy} {p.Coin}");
                    int numberOfItem = _reader.ReadInteger();
                    for (int i = 0; i < numberOfItem; i++)
                    {
                        string itemName = _reader.ReadLine();
                        Item item = weaponFactory.Create(itemName, 0, 0);
                        items.Add(item);
                        Console.WriteLine($"Loaded Item: {item.Name}");
                        p.Inventory.Add(item, p);
                    }
                } else
                {
                    s.Level = 1;
                    s.IsAbleToContinue = false;
                }
                
            }
            finally
            {
                _reader.Close();
            }
        }
        public void LoadDefault(Player p, Saver s)
        {
            s.IsAbleToContinue = true;
            s.Level = 1;
            s.Coin = 0;
        }
    }
}