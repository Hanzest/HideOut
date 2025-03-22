using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class DrawRoom 
    {
        private Bitmap _barrier;
        private Bitmap[] _floor;
        private Bitmap _hwall;
        private Bitmap _vwall;
        private string _path;
        public DrawRoom(string path, int theme)
        {
            _path = path;
            _floor = new Bitmap[3];
            UpdateTheme(theme);
        }
        public DrawRoom()
        {
            _path = "";
            _floor = new Bitmap[3];
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public void UpdateTheme(int theme)
        {
            string curPath = $"{_path}{theme}\\";
            Console.WriteLine(curPath);
            for(int i = 0; i < 3; i++)
            {
                if (_floor[i] != null)
                {
                    _floor[i].Free();
                }
                _floor[i] = new Bitmap($"floor{i}", $"{curPath}floor{i}.png");
            }
            if(_barrier != null)
            {
                _barrier.Free();
            }
            if(_hwall != null)
            {
                _hwall.Free();
            }
            if (_vwall != null)
            {
                _vwall.Free();
            }
            _barrier = new Bitmap("barrier", curPath + "barrier.png");
            _hwall = new Bitmap("hwall", curPath + "hwall.png");
            _vwall = new Bitmap("vwall", curPath + "vwall.png");
            
        }
        // _roomArray description:
        // 0: Floor
        // 1: HWall
        // 2: VWall
        // 3: Barrier
        public void Draw(Room room)
        {
            int centering = room.RoomWidth/ 2 * room.TileSize;
            for (int i = 0; i < room.RoomArray.GetLength(0); i++)
            {
                for(int j = 0; j < room.RoomArray.GetLength(1); j++)
                {
                    if (room.RoomArray[i, j] % 10 == 0)
                    {
                        switch(room.RoomArray[i, j] / 10)
                        {
                            case 0:
                                _floor[0].Draw(j * room.TileSize + room.Adder, i * room.TileSize - centering);
                                break;
                            case 1:
                                _floor[1].Draw(j * room.TileSize + room.Adder, i * room.TileSize - centering);
                                break;
                            case 2:
                                _floor[2].Draw(j * room.TileSize + room.Adder, i * room.TileSize - centering);
                                break;
                        }
                    }
                    else if (room.RoomArray[i, j] == 1)
                    {
                        
                        _hwall.Draw(j * room.TileSize + room.Adder, i * room.TileSize - 16 - centering);
                    } else if(room.RoomArray[i, j] == 2)
                    {
                        _vwall.Draw(j * room.TileSize + room.Adder, i * room.TileSize - 16 - centering);
                    }
                    else if (room.RoomArray[i, j] == 3)
                    {
                        _barrier.Draw(j * room.TileSize + room.Adder, i * room.TileSize - centering - 16);
                    }
                }
            }
        }
    }
}
