using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class DrawText
    {
        private Font _myFont;
        public DrawText() 
        {
            _myFont = SplashKit.LoadFont("ThaleahFat", "" +
                "A:\\Study\\Swinburne\\CS\\COS20007\\Assignments\\" +
                "Week 6 HD Custom Program\\Hideout\\Resource\\Fonts\\" +
                "ThaleahFat.ttf");
        }
        public void DrawH1(string text, float x, float y)
        {
            SplashKit.DrawText(text, Color.White, _myFont, 32,
                x - text.Length * 6.2f, y - 76);
        }
        public void DrawH2(string text, float x, float y)
        {
            SplashKit.DrawText(text, Color.White, _myFont, 28, x, y);
        }
    }
}
