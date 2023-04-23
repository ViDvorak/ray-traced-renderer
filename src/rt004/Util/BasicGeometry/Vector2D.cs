using OpenTK.Mathematics;
using System.Diagnostics.CodeAnalysis;

namespace rt004.Util
{
    public struct Vector2D
    {
        Vector2d direction;
        public double Length { get => direction.Length; }
        public double LengthSquared { get => direction.LengthSquared; }
        public Vector2d AsVector { get => direction; }


        public double X { get => direction.X; }
        public double Y { get => direction.Y; }



        public static Vector2D Zero => new Vector2D(0, 0);



        public Vector2D(Vector2d vector)
        {
            direction = vector;
        }

        public Vector2D(double x, double y)
        {
            direction = new Vector2d(x, y);
        }




        public Vector2D Normalized()
        {
            double lenght = Length;
            return new Vector2D(direction.X / lenght, direction.Y / lenght);
        }

        public void Normalize()
        {
            direction /= Length;
        }




        /// <summary>
        /// Gets values of vector rounded up
        /// </summary>
        /// <returns>tuple x, y coordinates rounded up</returns>
        public (int, int) RoundUp()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;

            int xCoord = direction.X.isFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = direction.Y.isFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            return (xCoord, yCoord);
        }


        /// <summary>
        /// Gets values of vector rounded down
        /// </summary>
        /// <returns>tuple x, y coordinates rounded down</returns>
        public (int, int) RoundDown()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;

            int xCoord = direction.X.isFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = direction.Y.isFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            return (xCoord, yCoord);
        }

        /// <summary>
        /// Gets values of point rounded down
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded down</returns>
        public static Vector2D RoundDown(Vector2D point)
        {
            return new Vector2D(point.RoundDown());
        }

        /// <summary>
        /// Gets values of point rounded up
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded up</returns>
        public static Vector2D RoundUp(Vector2D point)
        {
            return new Vector2D(point.RoundUp());
        }




        #region Opeartors

        public static explicit operator Vector2d(Vector2D dir) => dir.direction;
        public static explicit operator Vector2 (Vector2D dir) => (Vector2)dir.direction;



        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector2D vector && vector == this;
        }

        public static bool operator ==(Vector2D value, Vector2D other)
        {
            return value.X.isFloatEqual(other.X) && value.Y.isFloatEqual(other.Y);
        }

        public static bool operator !=(Vector2D value, Vector2D other)
        {
            return !(value == other);
        }



        public static Vector2D operator +(Vector2D vector)
        {
            return new Vector2D(vector.AsVector);
        }

        public static Vector2D operator +(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.direction + vector2.direction);
        }

        public static Vector2D operator +(Vector2D vector1, Point2D point)
        {
            return new Vector2D(point.AsVector + vector1.AsVector);
        }



        public static Vector2D operator -(Vector2D vector)
        {
            return new Vector2D(-(Vector2)vector);
        }

        public static Vector2D operator -(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.AsVector - vector2.AsVector);
        }




        public static Vector2D operator *(Vector2D direction, Vector2d scale)
        {
            return new Vector2D(direction.AsVector * scale);
        }

        public static Vector2D operator *(Vector2D direction, double scale)
        {
            return new Vector2D(direction.AsVector * scale);
        }





        public static Vector2D operator /(Vector2D dir, Vector2D scale)
        {
            return new Vector2D(dir.AsVector / scale.AsVector);
        }

        public static Vector2D operator /(Vector2D dir, double scale)
        {
            return new Vector2D(dir.AsVector / scale);
        }
        #endregion Operators




        public static double Dot(Vector2D vector1, Vector2D vector2)
        {
            return Vector2d.Dot(vector1.AsVector, vector2.AsVector);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{direction.X}, {direction.Y}]";
        }
    }
}
