using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public static class PositionValidation
    {
        public static bool CheckValid(float posx, float posy, Room[] rooms)
        {
            string pos = CheckValidString(posx, posy, rooms);
            if(pos == "Outside" || pos == "Floor")
            {
                return true;
            }
            return false;
        }

        public static string CheckValidString(float posx, float posy, Room[] rooms)
        {
            // Default CheckValid position for Character.
            // Enemy Checkvalid method could be different.
            int endRoomLength = 0;
            for (int i = 0; i < rooms.Length; i++)
            {
                endRoomLength += rooms[i].RoomLength;
                if (posx < endRoomLength)
                {
                    int xTile = (int)((posx - (endRoomLength - rooms[i].RoomLength)) / rooms[i].TileSize);
                    int yTile = (int)(posy + rooms[i].RoomWidth * rooms[i].TileSize / 2) / (rooms[i].TileSize);
                    string position = rooms[i].CoordinatePosition(xTile, yTile);
                    return position;
                }
            }
            return "Outside";
        }

    }
}
