using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class BuilderEnd : BuilderRoom, IBuilderRoom
    {
        public BuilderEnd() { }
        public override void BuildWalls(int roomSize, int[,] roomArray)
        {
            for (int i = roomSize / 2 - 2; i <= roomSize / 2 + 2; i++)
            {
                roomArray[i, 0] = 0;
            }
            roomArray[roomSize / 2 - 3, 0] = 1;
        }
    }
}
