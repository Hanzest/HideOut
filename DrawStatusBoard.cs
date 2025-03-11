using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class DrawStatusBoard
    {
        private DrawText _drawText;

        private Bitmap _heartIcon;
        private Bitmap _energyIcon;
        private Bitmap _armorIcon;
        private int _width;
        private int _height;
        public DrawStatusBoard(string path) 
        {
            _drawText = new DrawText();
            _width = 400;
            _height = 180;
            _heartIcon = new Bitmap("heart", $"{path}heart.png");
            _energyIcon = new Bitmap("energy", $"{path}energy.png");
            _armorIcon = new Bitmap("armor", $"{path}armor.png");
        }
        public float X
        {
            get; set;
        }
        public float Y
        {
            get; set;
        }
        public void Draw(Player p)
        {
            X = p.X - 800 + 24;
            Y = p.Y - 480 + 24;
            
            DrawFrame();
            
            // Health Bar
            _heartIcon.Draw(X + 25, Y + 33);
            DrawBar((float)p.Health / p.MaxHealth, Color.RGBColor(255, 0, 58), X + 75, Y + 30, _width - 125, 25);
            _drawText.DrawH1($"{p.Health} / {p.MaxHealth}",
                X + 75 + 0.5f * (_width - 125), Y + 28 + 76);
            // Energy Bar
            _energyIcon.Draw(X + 25, Y + 83);
            DrawBar((float)p.Energy / p.MaxEnergy, Color.RGBColor(0, 74, 255), X + 75, Y + 80, _width - 125, 25);
            _drawText.DrawH1($"{p.Energy} / {p.MaxEnergy}",
                X + 75 + 0.5f * (_width - 125), Y + 78 + 76);
            // Armor Bar
            _armorIcon.Draw(X + 25, Y + 133);
            DrawBar((float)p.Armor / p.MaxArmor, Color.RGBColor(200, 200, 200), X + 75, Y + 130, _width - 125, 25);
            _drawText.DrawH1($"{p.Armor} / {p.MaxArmor}",
                X + 75 + 0.5f * (_width - 125), Y + 128 + 76);
        }
        public void DrawFrame()
        {
            SplashKit.FillRectangle(Color.RGBColor(50, 50, 50),
                X, Y, _width, _height);
            SplashKit.FillRectangle(Color.RGBColor(249, 169, 10),
                X + 2, Y + 2, _width - 4, _height - 4);
        }
        public void DrawBar(float ratio, Color color, float x, float y, float width, float height)
        {
            SplashKit.FillRectangle(Color.RGBColor(73, 74, 0),
                x, y, width, height);
            SplashKit.FillRectangle(color,
                x + 2, y + 2, ratio * (width - 4f), height - 4);
        }
        
    }
}
