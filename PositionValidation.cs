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
        public static int RoomPosition(float posx, float posy, Room[] rooms)
        {
            int endRoomLength = 0;
            for (int i = 0; i < rooms.Length; i++)
            {
                endRoomLength += rooms[i].RoomLength;
                if (posx < endRoomLength)
                {
                    return i;
                }
            }
            return -1;
        }
        public static bool CharacterInsideOneRoom(Character c, Room[] rooms)
        {
            if(RoomPosition(c.X - c.Width / 2, c.Y - c.Height / 2, rooms) == RoomPosition(c.X + c.Width / 2, c.Y + c.Height / 2, rooms))
            {
                return true;
            }
            return false;
        }
        public static bool PointInRectangle(float posx, float posy, 
                                            float xMin, float xMax, float yMin, float yMax)
        {
            if(xMin < posx && posx < xMax &&
                yMin < posy && posy < yMax)
            {
                return true;
            }
            return false;
        }
        public static bool RectangleToRectangle(float x1Min, float x1Width, float y1Min, float y1Height,
                                                float x2Min, float x2Width, float y2Min, float y2Height)
        {
            x1Min = x1Min - x1Width / 2;
            y1Min = y1Min - y1Height / 2;
            x2Min = x2Min - x2Width / 2;
            y2Min = y2Min - y2Height / 2;
            if(PointInRectangle(x1Min, y1Min, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min, y1Min + y1Height / 2, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min, y1Min + y1Height, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min + x1Width / 2, y1Min, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min + x1Width / 2, y1Min + y1Height / 2, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min + x1Width / 2, y1Min + y1Height, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min + x1Width, y1Min, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min + x1Width, y1Min + y1Height / 2, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height) ||
               PointInRectangle(x1Min + x1Width, y1Min + y1Height, x2Min, x2Min + x2Width, y2Min, y2Min + y2Height)
              )
            {
                return true;
            }
            return false;
        }
        public static bool PointInRotatedRectangle(float posx, float posy, double angle,
                                                   float xMin, float yMin, int width, int height)
        {
            float centerX = xMin + width / 2;
            float centerY = yMin + height / 2;
            angle = -angle;
            double reverseRotatedX = centerX + (posx - centerX) * Math.Cos(angle) - (posy - centerY) * Math.Sin(angle);
            double reverseRotatedY = centerY + (posy - centerY) * Math.Sin(angle) + (posy + centerY) * Math.Cos(angle);
            if (PointInRectangle((float)reverseRotatedX, (float)reverseRotatedY, xMin, xMin + width, yMin, yMin + height))
            {
                return true;
            }
            return false;
        }
    }
}
