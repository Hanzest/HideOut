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
        public void LoadResource(DrawGameObject dgObject, DrawMap dMap, DrawStatusBoard dsBoard, Spawner spawner)
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
                for(int i = 1; i <= 6; i++)
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
                    if(i <= 4)
                    {
                        for(int j = 0; j < cnt; j++)
                        {
                            string name = _reader.ReadLine();
                            dgObject.AddBitmap(name, type);
                        }
                    } else if(i == 5)
                    {
                        // Default theme
                        dMap.SetPath(themeSource, 1);
                        for(int j = 0; j < cnt; j++)
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
 
                    } else if(i == 6)
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
    }
}
