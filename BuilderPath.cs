using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class BuilderPath : BuilderRoom, IBuilderRoom
    {
        public BuilderPath() { }
        public override void BuildWalls(int roomSize, int[,] roomArray)
        {
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize * 3; j++)
                {
                    if (i == 0 || i == roomSize - 1)
                    {
                        roomArray[i, j] = 1;
                    }
                    else
                    {
                        roomArray[i, j] = 0;
                    }
                }
            }
        }
    }
}
