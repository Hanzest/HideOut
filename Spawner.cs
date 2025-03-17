using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class Spawner
    {
        List<List<string>> _themeListEnemy;
        public Spawner(List<List<string>> themeListEnemy)
        {
            _themeListEnemy = themeListEnemy;
        }
        public List<List<string>> ThemeListEnemy
        {
            get { return _themeListEnemy; }
            set { _themeListEnemy = value; }
        }
        public void SetUpRoom(HashSet<Character> characters, HashSet<Item> items, Room[] rooms, ICharacterFactory enemyFactory, IItemFactory gateFactory, int theme)
        {
            foreach (Room room in rooms)
            {
                int centering = room.RoomWidth / 2 * room.TileSize;
                if (room.RoomNumber == 0)
                {
                    items.Add(gateFactory.Create("InGate", room.Adder + room.RoomWidth / 2 * room.TileSize, 0));
                }
                if(room.RoomNumber == 1)
                {
                    items.Add(gateFactory.Create("OutGate", room.Adder + room.RoomWidth / 2 * room.TileSize, 0));
                }
                if (room.RoomNumber == 2)
                {
                    int checkInfLoop = 0;
                    HashSet<Character> dump_c = new HashSet<Character>();
                    int cnt = 0;
                    bool ok = false;
                    
                    while (cnt < room.RoomWidth / 6)
                    {
                        ok = true;
                        if(checkInfLoop > 1000)
                        {
                            Console.WriteLine("Loop is break");
                            break;
                        }
                        checkInfLoop++;
                        
                        int rnd1 = SplashKit.Rnd(4, room.RoomWidth - 5);
                        int rnd2 = SplashKit.Rnd(4, room.RoomWidth - 5);
                        for (int i = rnd1 - 1; i <= rnd1 + 3; i++)
                        {
                            for (int j = rnd2 - 1; j <= rnd2 + 1; j++)
                            {
                                if (room.RoomArray[i, j] == 3)
                                {
                                    ok = false;
                                }
                            }
                        }
                        
                        if (ok && PositionValidation.CheckValid(room.Adder + rnd1 * room.TileSize, rnd2 * room.TileSize - centering, rooms))
                        {
                            // Spawning here
                            Character c = enemyFactory.Create(_themeListEnemy[theme][rnd1 % 4], room.Adder + rnd1 * room.TileSize, rnd2 * room.TileSize - centering);
                            if(c != null)
                            {
                                dump_c.Add(c);
                                cnt++;
                            } else
                            {
                                Console.WriteLine("enemy trying to create is null");
                            }
                        }
                    }
                    foreach(Character character in dump_c)
                    {
                        characters.Add(character);
                        if(character.Type == CharacterType.RangeEnemy)
                        {
                            RangeEnemy rEnemy = (RangeEnemy)character;
                            items.Add(rEnemy.Inventory.GetItem);
                        }                              
                    }
                }
            }
        }
    }
}
