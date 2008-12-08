using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    public struct Vector2
    {
        [XmlAttribute]
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 Zero=new Vector2(0f, 0f);
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x  - b.x, a.y - b.y);
        }
        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }
        public static Nullable<Vector2> lineCircleIntersection(Vector2 lineStart, Vector2 lineEnd, Vector2 circlePos, float circleRadius)
        {
            Vector2 toLineEnd = lineEnd - lineStart;
            Vector2 toCircle = circlePos - lineStart;
            float t = Vector2.Dot(toCircle, toLineEnd) / Vector2.Dot(toLineEnd, toLineEnd);
            if (t < 0f)
            {
                t = 0f;
            }
            else if (t > 1f)
            {
                t = 1f;
            }
            Vector2 closest = lineStart + t * toLineEnd;
            Vector2 d = circlePos - closest;

            double dist = d.getLength();
            if (dist <= circleRadius)
            {
                double theta = Math.Acos(dist / circleRadius);
                double collideOffset = Math.Sin(theta) * circleRadius;
                double x = t - (collideOffset / toLineEnd.getLength());
                return lineStart + (toLineEnd * (float)x);
            }
            else
            {
                return null;
            }
        }
        public static double lineCircleDistance(Vector2 lineStart, Vector2 lineEnd, Vector2 circlePos)
        {
            Vector2 toLineEnd = lineEnd - lineStart;
            Vector2 toCircle = circlePos - lineStart;
            float t = Vector2.Dot(toCircle, toLineEnd) / Vector2.Dot(toLineEnd, toLineEnd);
            if (t < 0f)
            {
                t = 0f;
            }
            else if (t > 1f)
            {
                t = 1f;
            }
            Vector2 closest = lineStart + t * toLineEnd;
            Vector2 d = circlePos - closest;

            return d.getLength();
        }
        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator *(float b, Vector2 a)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x / b, a.y / b);
        }
        public static Vector2 operator /(float b, Vector2 a)
        {
            return new Vector2(a.x / b, a.y / b);
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public override bool Equals(Object o)
        {
            if (o is Vector2)
            {
                Vector2 b = (Vector2)o;
                return x == b.x && y == b.y;
            }
            else
            {
                return false;
            }
            
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public double getLength()
        {
            return Math.Sqrt((x * x) + (y * y));
        }
        public float getSqrdLength()
        {
            return (x * x) + (y * y);
        }
        public void normalise()
        {
            float length=(float)getLength();
            if (length != 0)
            {
                x /= length;
                y /= length;
            }
        }
        public void makePerpendicular()
        {
            float temp = x;
            x = -y;
            y = temp;
        }
        public static double getSqrdDistance(Vector2 a, Vector2 b)
        {
            float xdist = a.x - b.x;
            float ydist = a.y - b.y;
            return (xdist * xdist) + (ydist * ydist);
        }
        public static double getDistance(Vector2 a, Vector2 b)
        {
            float xdist = a.x - b.x;
            float ydist = a.y - b.y;
            return Math.Sqrt((xdist * xdist) + (ydist * ydist));
        }
        public string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }
    }
}
