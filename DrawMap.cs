using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class DrawMap
    {
        private DrawRoom _drawRoom;
        private string _path;
        public DrawMap(string path)
        {
            _drawRoom = new DrawRoom(path, 1); // Default theme
            _path = path;
        }
        public void Draw(Map map)
        {
            for (int i = 0; i < map.MapSize; i++)
            {
                _drawRoom.Draw(map.Rooms[i]);
            }
        }
    }
}
