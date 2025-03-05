using System;
using System.Numerics;
using SplashKitSDK;

namespace HideOut
{
    public class Program
    {
        public static void Main()
        {
            
            Window window = new Window("Hide Out", 1600, 960);
            GameManager gameManager = new GameManager();
            gameManager.SetUp();
            while (!window.CloseRequested) // Fix: Access CloseRequested as a property, not a method  
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.Black);
                gameManager.Update();
                gameManager.Draw();
                SplashKit.RefreshScreen(60);
                SplashKit.Delay(1000 / 60); // Ensure 60 FPS  
            }
        }
    }
}
