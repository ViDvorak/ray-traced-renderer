using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004.Util
{
    public struct Point3D
    {
        readonly Vector3 position;

        public readonly Vector3 AsVector { get => position; }

        public float X { get => position.X; }
        public float Y { get => position.Y; }
        public float Z { get => position.Z; }

        public readonly double DistanceFromOrigin { get => position.Length; }
        public readonly double DistanceFromOriginSquared { get => position.LengthSquared; }



        public static Point3D Zero => new Point3D(0, 0, 0);



        public Point3D(Vector3 position)
        {
            this.position = position;
        }

        public Point3D(float x, float y, float z)
        {
            this.position = new Vector3(x, y, z);
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

            int xCoord = position.X.isFloatEquals(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = position.Y.isFloatEquals(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            int zCoord = position.Z.isFloatEquals(zBasicRounding) ? zBasicRounding : zBasicRounding + 1;
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

            int xCoord = position.X.isFloatEquals(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = position.Y.isFloatEquals(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            int zCoord = position.Z.isFloatEquals(zBasicRounding + 1) ? zBasicRounding + 1 : zBasicRounding;
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

        public static explicit operator Vector3(Point3D point) => point.position;
        public static explicit operator Vector3d(Point3D point) => (Vector3d)point.position;




        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Point3D point && point == this;
        }

        public static bool operator ==(Point3D value, Point3D other)
        {
            return value.X.isFloatEquals(other.X) && value.Y.isFloatEquals(other.Y) && value.Z.isFloatEquals(other.Z);
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
            return new Point3D(point.AsVector / scale);
        }
        #endregion Operators


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
