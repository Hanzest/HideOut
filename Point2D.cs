using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{

    public struct Point2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        // Constructor
        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Overload + operator
        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
        }

        // Overload - operator
        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        }
        public SplashKitSDK.Point2D ToSplashKitPoint()
        {
            return new SplashKitSDK.Point2D() { X = this.X, Y = this.Y };
        }

        // Override ToString for easy display
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
