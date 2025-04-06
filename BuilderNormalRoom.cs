using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class BuilderNormalRoom : BuilderRoom, IBuilderRoom
    {
        public BuilderNormalRoom() { }
        public override void BuildWalls(int roomSize, int[,] roomArray)
        {
            for (int i = roomSize / 2 - 2; i <= roomSize / 2 + 2; i++)
            {
                roomArray[i, 0] = 0;
                //_roomArray[i, _roomSize - 1] = 0;
            }
            roomArray[roomSize / 2 - 3, roomSize - 1] = 1;
            roomArray[roomSize / 2 - 3, 0] = 1;
        }
    }
}
