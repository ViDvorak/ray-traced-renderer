using OpenTK.Mathematics;
using System.Diagnostics.CodeAnalysis;

namespace rt004.Util
{
    /// <summary>
    /// Represents an immutable 3-dimensional vector.
    /// </summary>
    public struct Vector3D
    {
        /// <summary>
        /// The vector's internal representation as a <see cref="Vector3d"/>.
        /// </summary>
        Vector3d direction;

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        public double Length { get => direction.Length; }

        /// <summary>
        /// Gets the square of the vector's length (X*X + Y*Y + Z*Z), avoiding the cost of a square root calculation.
        /// </summary>
        public double LengthSquared { get => direction.LengthSquared; }

        /// <summary>
        /// Returns the vector's internal representation as a <see cref="Vector3d"/>.
        /// </summary>
        public Vector3d AsVector { get => direction; }

        /// <summary>
        /// Gets the X coordinate of the vector.
        /// </summary>
        public double X { get => direction.X; }

        /// <summary>
        /// Gets the Y coordinate of the vector.
        /// </summary>
        public double Y { get => direction.Y; }

        /// <summary>
        /// Gets the Z coordinate of the vector.
        /// </summary>
        public double Z { get => direction.Z; }

        /// <summary>
        /// Returns a new instance of a zero vector (0, 0, 0).
        /// </summary>
        public static Vector3D Zero => new Vector3D(0, 0, 0);

        /// <summary>
        /// Returns a unit vector along the X axis.
        /// </summary>
        public static Vector3D OneX => new Vector3D(1, 0, 0);

        /// <summary>
        /// Returns a unit vector along the Y axis.
        /// </summary>
        public static Vector3D OneY => new Vector3D(0, 1, 0);

        /// <summary>
        /// Returns a unit vector along the Z axis.
        /// </summary>
        public static Vector3D OneZ => new Vector3D(0, 0, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> struct from a <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="vector">The vector to initialize from.</param>
        public Vector3D(Vector3d vector)
        {
            direction = vector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> struct from the specified X, Y, and Z coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the vector.</param>
        /// <param name="y">The Y coordinate of the vector.</param>
        /// <param name="z">The Z coordinate of the vector.</param>
        public Vector3D(double x, double y, double z)
        {
            direction = new Vector3d(x, y, z);
        }

        /// <summary>
        /// Returns a normalized (unit length) version of this vector.
        /// </summary>
        /// <returns>A new <see cref="Vector3D"/> with unit length.</returns>
        public Vector3D Normalized()
        {
            double length = Length;
            return new Vector3D(direction.X / length, direction.Y / length, direction.Z / length);
        }

        /// <summary>
        /// Normalizes the vector in place, updating its direction to unit length.
        /// </summary>
        public void Normalize()
        {
            direction /= Length;
        }

        /// <summary>
        /// Rounds up the X, Y, and Z coordinates of the vector to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X, Y, and Z coordinates.</returns>
        public (int X, int Y, int Z) RoundUp()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;
            int zBasicRounding = (int)direction.Z;

            int xCoord = direction.X.IsFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = direction.Y.IsFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            int zCoord = direction.Z.IsFloatEqual(zBasicRounding) ? zBasicRounding : zBasicRounding + 1;
            return (xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// Rounds down the X, Y, and Z coordinates of the vector to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X, Y, and Z coordinates.</returns>
        public (int X, int Y, int Z) RoundDown()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;
            int zBasicRounding = (int)direction.Z;

            int xCoord = direction.X.IsFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = direction.Y.IsFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            int zCoord = direction.Z.IsFloatEqual(zBasicRounding + 1) ? zBasicRounding + 1 : zBasicRounding;
            return (xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// Returns a new vector with the X, Y, and Z coordinates rounded down.
        /// </summary>
        /// <param name="point">The vector to round down.</param>
        /// <returns>A new <see cref="Vector3D"/> instance with rounded coordinates.</returns>
        public static Vector3D RoundDown(Vector3D point)
        {
            return new Vector3D(point.RoundDown());
        }

        /// <summary>
        /// Returns a new vector with the X, Y, and Z coordinates rounded up.
        /// </summary>
        /// <param name="point">The vector to round up.</param>
        /// <returns>A new <see cref="Vector3D"/> instance with rounded coordinates.</returns>
        public static Vector3D RoundUp(Vector3D point)
        {
            return new Vector3D(point.RoundUp());
        }

        #region Operators

        /// <summary>
        /// Explicitly converts a <see cref="Vector3D"/> to a <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="dir">The <see cref="Vector3D"/> instance to convert.</param>
        public static explicit operator Vector3d(Vector3D dir) => dir.direction;

        /// <summary>
        /// Explicitly converts a <see cref="Vector3D"/> to a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="dir">The <see cref="Vector3D"/> instance to convert.</param>
        public static explicit operator Vector3(Vector3D dir) => (Vector3)dir.direction;

        /// <summary>
        /// Negates the vector, flipping its direction.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3D"/> instance to negate.</param>
        /// <returns>A new <see cref="Vector3D"/> pointing in the opposite direction.</returns>
        public static Vector3D operator -(Vector3D vector) => new Vector3D(-vector.AsVector);

        /// <summary>
        /// Subtracts one <see cref="Vector3D"/> from another.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector to subtract from the first.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the difference.</returns>
        public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.AsVector - vector2.AsVector);
        }

        /// <summary>
        /// Subtracts a <see cref="Vector3D"/> from a <see cref="Point3D"/>.
        /// </summary>
        /// <param name="vector">The vector to subtract.</param>
        /// <param name="point">The point from which to subtract the vector.</param>
        /// <returns>A new <see cref="Point3D"/> representing the result of the subtraction.</returns>
        public static Point3D operator -(Vector3D vector, Point3D point)
        {
            return new Point3D((Vector3)(vector.AsVector - point.AsVector));
        }

        /// <summary>
        /// Adds two <see cref="Vector3D"/> instances.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the sum of the two vectors.</returns>
        public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.AsVector + vector2.AsVector);
        }

        /// <summary>
        /// Adds a <see cref="Vector3D"/> and a <see cref="Point3D"/>.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        /// <param name="point">The point to add to the vector.</param>
        /// <returns>A new <see cref="Point3D"/> representing the result of the addition.</returns>
        public static Point3D operator +(Vector3D vector, Point3D point)
        {
            return new Point3D((Vector3)(point.AsVector + vector.AsVector));
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The scalar multiplier.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the scaled vector.</returns>
        public static Vector3D operator *(Vector3D vector, double scale)
        {
            return new Vector3D(vector.AsVector * scale);
        }
        
        /// <summary>
        /// Multiplies two vectors component-wise.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The vector to scale by.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the scaled vector.</returns>
        public static Vector3D operator *(Vector3D vector, Vector3D scale)
        {
            return new Vector3D(vector.AsVector * scale.AsVector);
        }

        /// <summary>
        /// Multiplies a <see cref="Vector3D"/> by a <see cref="Vector3d"/> component-wise.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3D"/> to multiply.</param>
        /// <param name="scale">The <see cref="Vector3d"/> to multiply by.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the result of the component-wise multiplication.</returns>
        public static Vector3D operator *(Vector3D vector, Vector3d scale)
        {
            return new Vector3D(vector.AsVector * scale);
        }

        /// <summary>
        /// Divides a vector by another vector component-wise.
        /// </summary>
        /// <param name="vector">The vector to divide.</param>
        /// <param name="scale">The vector to divide by.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the divided vector.</returns>
        public static Vector3D operator /(Vector3D vector, Vector3d scale)
        {
            return new Vector3D(vector.AsVector / scale);
        }


        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="dir">The vector to divide.</param>
        /// <param name="scale">The scalar to divide by.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the scaled vector.</returns>
        public static Vector3D operator /(Vector3D dir, double scale)
        {
            return new Vector3D(dir.AsVector / scale);
        }

        #endregion Operators

        /// <summary>
        /// Computes the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>A new <see cref="Vector3D"/> representing the cross product.</returns>
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(Vector3d.Cross(vector1.AsVector, vector2.AsVector));
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product result as a double.</returns>
        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
            return Vector3d.Dot(vector1.AsVector, vector2.AsVector);
        }

        /// <summary>
        /// Transforms a vector by applying the specified quaternion rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The rotation quaternion.</param>
        /// <returns>A new <see cref="Vector3D"/> resulting from the rotation.</returns>
        public static Vector3D Transform(Vector3D vector, Quaternion rotation)
        {
            return new Vector3D(Vector3d.Transform((Vector3d)vector, new Quaterniond((Vector3d)rotation.ToEulerAngles())));
        }

        /// <summary>
        /// Transforms a direction vector by the given matrix, ignoring any translation.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <param name="transform">The transformation matrix.</param>
        /// <returns>A new <see cref="Vector3D"/> resulting from the transformation.</returns>
        public static Vector3D Transform(Vector3D vector, Matrix4d transform)
        {
            return new Vector3D(Vector3d.TransformVector((Vector3d)vector, transform));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>An integer representing the hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current vector.
        /// </summary>
        /// <returns>A string in the format "[X, Y, Z]".</returns>
        public override string ToString()
        {
            return $"[{direction.X}, {direction.Y}, {direction.Z}]";
        }


        /// <summary>
        /// Determines whether this <see cref="Vector3D"/> is equal to another <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector3D"/> to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Vector3D"/> is equal to the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector3D other)
        {
            return X.IsFloatEqual(other.X) && Y.IsFloatEqual(other.Y) && Z.IsFloatEqual(other.Z);
        }

        /// <summary>
        /// Determines whether two <see cref="Vector3D"/> instances are equal by comparing their components.
        /// </summary>
        /// <param name="left">The first <see cref="Vector3D"/> instance.</param>
        /// <param name="right">The second <see cref="Vector3D"/> instance.</param>
        /// <returns><c>true</c> if the components of both vectors are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="Vector3D"/> instances are not equal by comparing their components.
        /// </summary>
        /// <param name="left">The first <see cref="Vector3D"/> instance.</param>
        /// <param name="right">The second <see cref="Vector3D"/> instance.</param>
        /// <returns><c>true</c> if the components of both vectors are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether this instance of <see cref="Vector3D"/> is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="Vector3D"/>.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> is a <see cref="Vector3D"/> and has the same X, Y, and Z values
        /// as the current instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is not null && obj is Vector3D other && Equals(other);
        }
    }
}
