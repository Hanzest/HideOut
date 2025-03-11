using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public static class BitmapExtensions
    {
        public static void DrawFlippedY(this Bitmap bitmap, float x, float y)
        {
            SplashKit.DrawBitmap(bitmap, x, y, SplashKit.OptionFlipY());   
        }
        public static void DrawRotated(this Bitmap bitmap, float x, float y, double angle)
        {
            SplashKit.DrawBitmap(bitmap, x, y, SplashKit.OptionRotateBmp(angle * 180 / Math.PI));
        }
        public static void Free(this Bitmap bitmap)
        {
            SplashKit.FreeBitmap(bitmap);
        }
    }
}
