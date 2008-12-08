using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    /// <summary>
    /// Simple class to hold a rectangle of doubles
    /// </summary>
    /// <owner>Jonathan</owner>
    public struct Rectangle
    {
        private double x1;
        private double y1;
        private double x2;
        private double y2;
        private double height;
        private double width;

        public double X { get { return x1; } }
        public double Y { get { return y1; } }
        public double Left { get { return x1; } }
        public double Right { get { return x2; } }
        /// <summary>
        /// Returns the greatest Y value, unlike other Rectangles
        /// </summary>
        public double Top { get { return y2; } }
        /// <summary>
        /// Returns the smallest Y value, unlike other Rectangles
        /// </summary>
        public double Bottom { get { return y1; } }
        public double Width { get { return width; } }
        public double Height { get { return height; } }
        
        public Rectangle(double x, double y, double width, double height)
        {
            x1 = x;
            y1 = y;
            x2 = x + width;
            y2 = y + height;
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return string.Format("[({0}, {1}) - ({2}, {3})] ({4}, {5})", x1, y1, x2, y2, width, height);
        }
    }
}
