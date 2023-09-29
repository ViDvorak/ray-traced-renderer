using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace rt004.Util
{
    public struct Vector3D
    {
        Vector3d direction;
        public double Length { get => direction.Length; }
        public double LengthSquared { get => direction.LengthSquared; }
        public Vector3d AsVector { get => direction; }


        public double X { get => direction.X; }
        public double Y { get => direction.Y; }
        public double Z { get => direction.Z; }




        public static Vector3D Zero => new Vector3D(0, 0, 0);
        public static Vector3D OneX => new Vector3D(1, 0, 0);
        public static Vector3D OneY => new Vector3D(0, 1, 0);
        public static Vector3D OneZ => new Vector3D(0, 0, 1);


        public Vector3D(Vector3d vector)
        {
            direction = vector;
        }

        public Vector3D(double x, double y, double z)
        {
            direction = new Vector3d(x, y, z);
        }





        public Vector3D Normalized()
        {
            double lenght = Length;
            return new Vector3D(direction.X / lenght, direction.Y / lenght, direction.Z / lenght);
        }

        public void Normalize()
        {
            direction /= Length;
        }




        /// <summary>
        /// Gets values of vector rounded up
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded up</returns>
        public (int, int, int) RoundUp()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;
            int zBasicRounding = (int)direction.Z;

            int xCoord = direction.X.isFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = direction.Y.isFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            int zCoord = direction.Z.isFloatEqual(zBasicRounding) ? zBasicRounding : zBasicRounding + 1;
            return (xCoord, yCoord, zCoord);
        }


        /// <summary>
        /// Gets values of vector rounded down
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded down</returns>
        public (int, int, int) RoundDown()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;
            int zBasicRounding = (int)direction.Z;

            int xCoord = direction.X.isFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = direction.Y.isFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            int zCoord = direction.Z.isFloatEqual(zBasicRounding + 1) ? zBasicRounding + 1 : zBasicRounding;
            return (xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// Gets values of point rounded down
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded down</returns>
        public static Vector3D RoundDown(Vector3D point)
        {
            return new Vector3D(point.RoundDown());
        }

        /// <summary>
        /// Gets values of point rounded up
        /// </summary>
        /// <returns>tuple x, y, z coordinates rounded up</returns>
        public static Vector3D RoundUp(Vector3D point)
        {
            return new Vector3D(point.RoundUp());
        }






        #region Opeartors

        public static explicit operator Vector3d(Vector3D dir) => dir.direction;
        public static explicit operator Vector3(Vector3D dir) => (Vector3)dir.direction;


        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector3D vector && vector == this;
        }

        public static bool operator ==(Vector3D value, Vector3D other)
        {
            return value.X.isFloatEqual(other.X) && value.Y.isFloatEqual(other.Y) && value.Z.isFloatEqual(other.Z);
        }

        public static bool operator !=(Vector3D value, Vector3D other)
        {
            return !(value == other);
        }



        public static Vector3D operator +(Vector3D vector) => vector;
        public static Vector3D operator -(Vector3D vector) => new Vector3D(-vector.AsVector);




        public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.AsVector + vector2.AsVector);
        }

        public static Point3D operator +(Vector3D vector, Point3D point)
        {
            return new Point3D((Vector3)(point.AsVector + vector.AsVector));
        }




        public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.AsVector - vector2.AsVector);
        }

        public static Point3D operator -(Point3D point, Vector3D vector)
        {
            return new Point3D((Vector3)(point.AsVector - vector.AsVector));
        }



        public static Vector3D operator *(Vector3D vector, Vector3D scale)
        {
            return new Vector3D(vector.AsVector * scale.AsVector);
        }

        public static Vector3D operator *(Vector3D vector, Vector3d scale)
        {
            return new Vector3D(vector.AsVector * scale);
        }

        public static Vector3D operator *(Vector3D vector, double scale)
        {
            return new Vector3D(vector.AsVector * scale);
        }




        public static Vector3D operator /(Vector3D vector, Vector3d scale)
        {
            return new Vector3D(vector.AsVector / scale);
        }

        public static Vector3D operator /(Vector3D dir, double scale)
        {
            return new Vector3D(dir.AsVector / scale);
        }
        
    #endregion Operators




        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(Vector3d.Cross(vector1.AsVector, vector2.AsVector));
        }

        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
            return Vector3d.Dot(vector1.AsVector, vector2.AsVector);
        }


        public static Vector3D Transform(Vector3D vector, Quaternion rotation)
        {
            return new Vector3D(Vector3d.Transform((Vector3d)vector, new Quaterniond((Vector3d)rotation.ToEulerAngles())));
        }

        public static Vector3D Transform(Vector3D vector, Matrix4d transform)
        {
            return new Vector3D(Vector3d.TransformVector((Vector3d)vector, transform));
        }




        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{direction.X}, {direction.Y}, {direction.Z}]";
        }
    }
}
