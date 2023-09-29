using OpenTK.Mathematics;
using System.Diagnostics.CodeAnalysis;

namespace rt004.Util
{
    public struct Point3D
    {
        readonly Vector3d position;

        public readonly Vector3d AsVector { get => position; }

        public double X { get => position.X; }
        public double Y { get => position.Y; }
        public double Z { get => position.Z; }

        public readonly double DistanceFromOrigin { get => position.Length; }
        public readonly double DistanceFromOriginSquared { get => position.LengthSquared; }



        public static Point3D Zero => new Point3D(0, 0, 0);



        public Point3D(Vector3d position)
        {
            this.position = position;
        }

        public Point3D(double x, double y, double z)
        {
            this.position = new Vector3d(x, y, z);
        }




        /// <summary>
        /// Gets values of point rounded up
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded up</returns>
        public (int, int, int) RoundUp()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;
            int zBasicRounding = (int)position.Z;

            int xCoord = position.X.isFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = position.Y.isFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            int zCoord = position.Z.isFloatEqual(zBasicRounding) ? zBasicRounding : zBasicRounding + 1;
            return (xCoord, yCoord, zCoord);
        }


        /// <summary>
        /// Gets values of point rounded down
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded down</returns>
        public (int, int, int) RoundDown()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;
            int zBasicRounding = (int)position.Z;

            int xCoord = position.X.isFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = position.Y.isFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            int zCoord = position.Z.isFloatEqual(zBasicRounding + 1) ? zBasicRounding + 1 : zBasicRounding;
            return (xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// Gets values of point rounded down
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded down</returns>
        public static Point3D RoundDown( Point3D point)
        {
            return new Point3D( point.RoundDown());
        }

        /// <summary>
        /// Gets values of point rounded up
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded up</returns>
        public static Point3D RoundUp(Point3D point)
        {
            return new Point3D(point.RoundUp());
        }




        #region Operators
        public static explicit operator Vector3(Point3D point) => (Vector3)point.position;
        public static explicit operator Vector3d(Point3D point) => point.position;




        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Point3D point && point == this;
        }

        public static bool operator ==(Point3D value, Point3D other)
        {
            return value.X.isFloatEqual(other.X) && value.Y.isFloatEqual(other.Y) && value.Z.isFloatEqual(other.Z);
        }

        public static bool operator !=(Point3D value, Point3D other)
        {
            return !(value == other);
        }




        public static Point3D operator +(Point3D point, Vector3D vector)
        {
            return new Point3D(point.AsVector + (Vector3)vector);
        }




        public static Vector3D operator -(Point3D point1, Point3D point2)
        {
            return new Vector3D(point1.AsVector - point2.AsVector);
        }

        public static Point3D operator -(Point3D point, Vector3D direction)
        {
            return new Point3D(point.AsVector - (Vector3)direction);
        }




        public static Point3D operator *(Point3D point, float scale)
        {
            return new Point3D(point.AsVector * scale);
        }
        public static Point3D operator *(Point3D point, Vector3 scale)
        {
            return new Point3D(point.AsVector * scale);
        }





        public static Point3D operator /(Point3D point, float scale)
        {
            return new Point3D(point.AsVector / scale);
        }

        public static Point3D operator /(Point3D point, Vector3 scale)
        {
            return point / (Vector3d)scale;
        }

        public static Point3D operator /(Point3D point, Vector3d scale)
        {
            return new Point3D(point.AsVector / scale);
        }
        #endregion Operators


        public static Point3D Transform(Point3D point, Matrix4d transformation)
        {
            return new Point3D(Vector3d.TransformPosition((Vector3d)point, transformation));
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{position.X}, {position.Y}, {position.Z}]";
        }
    }
}
