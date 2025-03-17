using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class DrawingComponent
    {
        public DrawingComponent() { }
        public void DrawRectangle(Color outer, Color inner, float x, float y, int width, int height)
        {
            SplashKit.FillRectangle(outer, x, y, width, height);
            SplashKit.FillRectangle(inner, x + 2, y + 2, width - 4, height - 4);
        }
        public void DrawHoveringRectangle(float posx, float posy, Color outer, Color inner, Color hoverInner, float x, float y, int width, int height)
        {
            if(PositionValidation.PointInRectangle(posx, posy, x, x + width, y, y + height))
            {
                DrawRectangle(outer, hoverInner, x, y, width, height);
            } else
            {
                DrawRectangle(outer, inner, x, y, width, height);
            }
        }
    }
}
