using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public interface IBuilderRoom
    {
        public void BuildBaseFloor(int roomNumber, int roomSize, int[,] roomArray);
        public void BuildWalls(int roomSize, int[,] roomArray);
        public void BuildBarriers(int roomNumber, int roomSize, int[,] roomArray);
    }
}
