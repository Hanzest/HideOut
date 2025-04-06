using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class Room
    {
        private int _rnd;
        private int _roomNumber;
        private int _roomIndex;
        private int[,] _roomArray;
        private int _tileSize;
        private int _roomSize;
        private int _adder;
        private bool _isClear;
        private bool _isPlayerEnter;

        public Room(int roomNumber, int adder, int roomIndex) // 0: Start, 1: End, 2: Normal Room, 3: Path
        {
            _tileSize = 48;
            _rnd = RandomNumberGenerator.GetInt32(8, 15);
            _roomNumber = roomNumber;
            _adder = adder;
            _roomIndex = roomIndex;
            if (_roomNumber == 0 || _roomNumber == 1)
            {
                _rnd = 5;
            }
            _roomSize = _rnd * 2 + 1;
            if (_roomNumber == 3)
            {
                _roomSize = 7;
                _roomArray = new int[_roomSize, _roomSize * 3];
            }
            else
            {
                _roomArray = new int[_roomSize, _roomSize];
            }
            _adder = adder;
            _isClear = false;
            _isPlayerEnter = false;
        }
        public int TileSize
        {
            get { return _tileSize; }
        }
        public int Adder
        {
            get { return _adder; }
        }
        public int RoomIndex
        {
            get { return _roomIndex; }
        }
        public int RoomLength
        {
            get { 
                if(_roomNumber == 3)
                {
                    return _roomSize * 3 * TileSize;
                } else
                {
                    return _roomSize * TileSize;
                }
            }
        }
        public int RoomWidth
        {
            get { return _roomSize; }
        }
        public bool IsClear
        {
            get { return _isClear; }
        }
        public bool IsPlayerEnter
        {
            get { return _isPlayerEnter; }
        }
        
        public void UpdateRoom(HashSet<Character> characters)
        {
            
            int cnt = 0;
            foreach (Character c in characters)
            {
                if(c.Type != CharacterType.Player && c.RoomIndex == RoomIndex)
                {
                    cnt++;
                } else if(c.Type == CharacterType.Player && c.RoomIndex == RoomIndex)
                {
                    _isPlayerEnter = true;
                }
            }
            if(cnt == 0)
            {
                _isClear = true;
                if(RoomNumber == 2 && IsClear)
                {
                    Unlock();
                }
            }
        }

        // Generate 2d array of a room
        //  A room is aligned center and justified center
        //  Outside of a room, there is a path leads to another room
        // with the width of 5 tiles.

        // _roomArray description:
        // 0: Floor
        // 1: HWall
        // 2: VWall
        // 3: Barrier
        // Type of Room (_roomNumber):
        // 0: Start, 1: End, 2: Dungeon Room, 3: Path

        // Normal Room:         Start:               End:                 Path:
        // 2 1 1 1 1 1 2        2 1 1 1 2            2 1 1 1 2            1 1 1 1 1
        // 2 0 0 0 0 0 2        2 0 0 0 0            0 0 0 0 2            0 0 0 0 0
        // 0 0 0 0 0 0 0        2 0 0 0 0            0 0 0 0 2            0 0 0 0 0
        // 0 0 0 0 0 0 0        2 0 0 0 0            0 0 0 0 2            0 0 0 0 0
        // 0 0 0 0 0 0 0        2 1 1 1 2            2 1 1 1 2            1 1 1 1 1
        // 2 0 0 0 0 0 2
        // 2 1 1 1 1 1 2

        public int [,] RoomArray{
            get { return _roomArray; }
        }
        public int RoomNumber
        {
            get { return _roomNumber; }
        }
        public string CoordinatePosition(int x, int y)
        {
            if(x >= RoomLength / TileSize || x < 0)
            {
                return "Outside";
            } else if(y >= RoomWidth || y < 0)
            {
                return "Outside";
            }
            switch (_roomArray[y, x] % 10)
            {
                case 0:
                    return "Floor";
                case 1:
                    return "HWall";
                case 2:
                    return "VWall";
                case 3:
                    return "Barrier";
                default:
                    return "Unknown";
            }
        }
        public void Lock()
        {
            for (int i = _roomSize / 2 - 2; i <= _roomSize / 2 + 2; i++)
            {
                _roomArray[i, 0] = 2;
            }
            Console.WriteLine(RoomIndex);

        }
        public void Unlock()
        {
            for (int i = _roomSize / 2 - 2; i <= _roomSize / 2 + 2; i++)
            {
                _roomArray[i, 0] = 0;
                _roomArray[i, _roomSize - 1] = 0;
            }
        }
    }
}
