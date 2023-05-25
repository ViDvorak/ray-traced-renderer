using OpenTK.Mathematics;
using rt004;
using System.Diagnostics.CodeAnalysis;

namespace rt004.Util
{
    public readonly struct Point2D
    {
        readonly Vector2 position;

        public readonly Vector2 AsVector { get => position; }

        public float X { get => position.X; }
        public float Y { get => position.Y; }

        public readonly double DistanceFromOrigin { get => position.Length; }
        public readonly double DistanceFromOriginSquared { get => position.LengthSquared; }



        public static Point2D Zero => new Point2D(0, 0);



        public Point2D(Vector2 position)
        {
            this.position = position;
        }

        public Point2D(Vector2d position)
        {
            this.position = (Vector2)position;
        }


        public Point2D(float x, float y)
        {
            this.position = new Vector2(x, y);
        }




        /// <summary>
        /// Gets values of point rounded up
        /// </summary>
        /// <returns>tuple x, y coordinates rounded up</returns>
        public (int, int) RoundUp()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;

            int xCoord = position.X.isFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = position.Y.isFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            return (xCoord, yCoord);
        }


        /// <summary>
        /// Gets values of point rounded down
        /// </summary>
        /// <returns>tuple x, y coordinates rounded down</returns>
        public (int, int) RoundDown()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;

            int xCoord = position.X.isFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = position.Y.isFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            return (xCoord, yCoord);
        }

        /// <summary>
        /// Gets values of point rounded down
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded down</returns>
        public static Point2D RoundDown(Point2D point)
        {
            return new Point2D(point.RoundDown());
        }

        /// <summary>
        /// Gets values of point rounded up
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded up</returns>
        public static Point2D RoundUp(Point2D point)
        {
            return new Point2D(point.RoundUp());
        }





        #region Operators
        public static explicit operator Vector2(Point2D dir) => dir.position;
        public static explicit operator Vector2d(Point2D dir) => (Vector2d)dir.position;


        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Point2D point && point == this;
        }

        public static bool operator ==(Point2D value, Point2D other)
        {
            return value.X.isFloatEqual(other.X) && value.Y.isFloatEqual(other.Y);
        }

        public static bool operator !=(Point2D value, Point2D other)
        {
            return !(value == other);
        }




        public static Point2D operator +(Point2D point, Vector2D vector)
        {
            return new Point2D(point.AsVector + (Vector2)vector.AsVector);
        }




        public static Vector2D operator -(Point2D point1, Point2D point2)
        {
            return new Vector2D(point1.AsVector - point2.AsVector);
        }

        public static Point2D operator -(Point2D position, Vector2D direction)
        {
            return new Point2D(position.AsVector - (Vector2)direction.AsVector);
        }




        public static Point2D operator *(Point2D point, float scale)
        {
            return new Point2D(point.AsVector * scale);
        }

        public static Point2D operator *(Point2D point, Vector2 scale)
        {
            return new Point2D(point.AsVector * scale);
        }




        public static Point2D operator /(Point2D point, float scale)
        {
            return new Point2D(point.AsVector / (float)scale);
        }

        public static Point2D operator /(Point2D p, Vector2 scale)
        {
            return new Point2D(p.AsVector / scale);
        }
        #endregion Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{position.X}, {position.Y}]";
        }
    }
}
