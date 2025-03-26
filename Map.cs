using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Map
    {
        private int _mapSize;
        private int _theme;
        private int[] _maptructure;
        private Room[] _rooms;
        public Map()
        {
            _mapSize = RandomNumberGenerator.GetInt32(3, 7);
            _mapSize = _mapSize * 2 + 1;
            _theme = RandomNumberGenerator.GetInt32(1, 3);
            _maptructure = new int[_mapSize];
            GenerateMapStructure();
            _rooms = new Room[_mapSize];
        }
        public int MapSize
        {
            get { return _mapSize; }
        }
        public Room[] Rooms
        {
            get { return _rooms; }
        }
        public void GenerateMapStructure() // 0: Start, 1: End, 2: Normal Room, 3: Path
        {
            for (int i = 0; i < _mapSize; i++)
            {
                if (i % 2 == 0)
                {
                    _maptructure[i] = 2;
                } else
                {
                    _maptructure[i] = 3;
                }
            }
            _maptructure[0] = 0;
            _maptructure[_mapSize - 1] = 1;
        }
        public void GenerateMap()
        {
            for(int i = 0; i < _mapSize; i++)
            {
                if (i == 0)
                {
                    _rooms[i] = new Room(_maptructure[i], 0, i);
                } else
                {
                    _rooms[i] = new Room(_maptructure[i], _rooms[i - 1].Adder + _rooms[i - 1].RoomLength, i);
                }
                    _rooms[i].GenerateRoom();
            }
        }
        public void UpdateMap(HashSet<Character> characters)
        {
            foreach (Room room in _rooms)
            {
                room.UpdateRoom(characters);
            }
        }
    }
}
