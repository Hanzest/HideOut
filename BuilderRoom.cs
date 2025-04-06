using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public abstract class BuilderRoom : IBuilderRoom
    {
        public virtual void BuildBaseFloor(int roomNumber, int roomSize, int[,] roomArray)
        {
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    if (i == 0 || i == roomSize - 1 || j == 0 || j == roomSize - 1)
                    {
                        roomArray[i, j] = 2;
                        if (i == 0 && (1 <= j && j <= roomSize - 2)
                            || i == roomSize - 1)
                        {
                            roomArray[i, j] = 1;
                        }
                    } else
                    {
                        int rnd = SplashKit.Rnd(0, 3);
                        roomArray[i, j] = 0 + rnd * 10;
                    }
                }
            }
        }
        public abstract void BuildWalls(int roomSize, int[,] roomArray);
        public virtual void BuildBarriers(int roomNumber, int roomSize, int[,] roomArray)
        {
            for(int i = 4; i <= roomSize - 5; i++)
            {
                for(int j = 4; j <= roomSize - 5; j++)
                {
                    if (roomNumber == 2)
                    {
                        int rnd2 = SplashKit.Rnd(0, 100);
                        if (rnd2 < 20)
                        {
                            bool noBarrierSurrounding = true;
                            for (int x = i - 4; x <= i; x++)
                            {
                                for (int y = j - 4; y <= j; y++)
                                {
                                    if (roomArray[x, y] == 3)
                                    {
                                        noBarrierSurrounding = false;
                                    }
                                }
                            }
                            if (noBarrierSurrounding)
                            {
                                roomArray[i, j] = 3;
                            }
                        }
                    }
                }
            }
        }
    }
}
