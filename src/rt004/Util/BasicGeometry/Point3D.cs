using OpenTK.Mathematics;

namespace rt004.Util
{
    /// <summary>
    /// Represents a 3D point with double precision, defined by X, Y, and Z coordinates.
    /// </summary>
    public struct Point3D
    {
        /// <summary>
        /// The position of the point represented as a <see cref="Vector3d"/>.
        /// </summary>
        readonly Vector3d position;

        /// <summary>
        /// Gets the position of the point as a <see cref="Vector3d"/>.
        /// </summary>
        public readonly Vector3d AsVector { get => position; }

        /// <summary>
        /// Gets the X coordinate of the point.
        /// </summary>
        public double X { get => position.X; }

        /// <summary>
        /// Gets the Y coordinate of the point.
        /// </summary>
        public double Y { get => position.Y; }

        /// <summary>
        /// Gets the Z coordinate of the point.
        /// </summary>
        public double Z { get => position.Z; }

        /// <summary>
        /// Gets the distance of the point from the origin.
        /// </summary>
        public readonly double DistanceFromOrigin { get => position.Length; }

        /// <summary>
        /// Gets the square of the distance of the point from the origin.
        /// </summary>
        public readonly double DistanceFromOriginSquared { get => position.LengthSquared; }

        /// <summary>
        /// Gets a <see cref="Point3D"/> instance representing the point (0, 0, 0).
        /// </summary>
        public static Point3D Zero => new Point3D(0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3D"/> struct from a <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="position">The position of the point.</param>
        public Point3D(Vector3d position)
        {
            this.position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3D"/> struct from the specified X, Y, and Z coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        /// <param name="y">The Y coordinate of the point.</param>
        /// <param name="z">The Z coordinate of the point.</param>
        public Point3D(double x, double y, double z)
        {
            this.position = new Vector3d(x, y, z);
        }

        /// <summary>
        /// Rounds the X, Y, and Z coordinates of the point up to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X, Y, and Z coordinates.</returns>
        public (int, int, int) RoundUp()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;
            int zBasicRounding = (int)position.Z;

            int xCoord = position.X.IsFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = position.Y.IsFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            int zCoord = position.Z.IsFloatEqual(zBasicRounding) ? zBasicRounding : zBasicRounding + 1;
            return (xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// Rounds the X, Y, and Z coordinates of the point down to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X, Y, and Z coordinates.</returns>
        public (int, int, int) RoundDown()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;
            int zBasicRounding = (int)position.Z;

            int xCoord = position.X.IsFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = position.Y.IsFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            int zCoord = position.Z.IsFloatEqual(zBasicRounding + 1) ? zBasicRounding + 1 : zBasicRounding;
            return (xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// Rounds the X, Y, and Z coordinates of a given point down.
        /// </summary>
        /// <param name="point">The point to round down.</param>
        /// <returns>A new <see cref="Point3D"/> instance with the rounded coordinates.</returns>
        public static Point3D RoundDown(Point3D point)
        {
            return new Point3D(point.RoundDown());
        }

        /// <summary>
        /// Rounds the X, Y, and Z coordinates of a given point up.
        /// </summary>
        /// <param name="point">The point to round up.</param>
        /// <returns>A new <see cref="Point3D"/> instance with the rounded coordinates.</returns>
        public static Point3D RoundUp(Point3D point)
        {
            return new Point3D(point.RoundUp());
        }

        #region Operators

        /// <summary>
        /// Explicitly converts a <see cref="Point3D"/> to a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance to convert.</param>
        public static explicit operator Vector3(Point3D point) => (Vector3)point.position;

        /// <summary>
        /// Explicitly converts a <see cref="Point3D"/> to a <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance to convert.</param>
        public static explicit operator Vector3d(Point3D point) => point.position;

        /// <summary>
        /// Determines whether two <see cref="Point3D"/> instances are equal by comparing their X, Y, and Z coordinates.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return obj is not null && obj is Point3D point && Equals(point);
        }

        /// <summary>
        /// Determines whether this <see cref="Point3D"/> is equal to another <see cref="Point3D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point3D"/> to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Point3D"/> has the same X, Y, and Z values as the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Point3D other)
        {
            return X.IsFloatEqual(other.X) && Y.IsFloatEqual(other.Y) && Z.IsFloatEqual(other.Z);
        }

        /// <summary>
        /// Compares two <see cref="Point3D"/> instances for equality.
        /// </summary>
        /// <param name="value">The first <see cref="Point3D"/> instance.</param>
        /// <param name="other">The second <see cref="Point3D"/> instance.</param>
        /// <returns>True if the X, Y, and Z coordinates are equal, false otherwise.</returns>
        public static bool operator ==(Point3D value, Point3D other)
        {
            return value.X.IsFloatEqual(other.X) && value.Y.IsFloatEqual(other.Y) && value.Z.IsFloatEqual(other.Z);
        }

        /// <summary>
        /// Compares two <see cref="Point3D"/> instances for inequality.
        /// </summary>
        /// <param name="value">The first <see cref="Point3D"/> instance.</param>
        /// <param name="other">The second <see cref="Point3D"/> instance.</param>
        /// <returns>True if the X, Y, and Z coordinates are not equal, false otherwise.</returns>
        public static bool operator !=(Point3D value, Point3D other)
        {
            return !(value == other);
        }

        /// <summary>
        /// Adds a <see cref="Vector3D"/> to a <see cref="Point3D"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance.</param>
        /// <param name="vector">The <see cref="Vector3D"/> instance.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the addition.</returns>
        public static Point3D operator +(Point3D point, Vector3D vector)
        {
            return new Point3D(point.AsVector + (Vector3)vector);
        }

        /// <summary>
        /// Negates the coordinates of this <see cref="Point3D"/>, returning a new <see cref="Point3D"/> with the opposite values.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance to negate.</param>
        /// <returns>A new <see cref="Point3D"/> with X, Y, and Z values negated.</returns>
        public static Point3D operator -(Point3D point)
        {
            return new Point3D(-point.X, -point.Y, -point.Z);
        }

        /// <summary>
        /// Subtracts one <see cref="Point3D"/> from another, returning the result as a <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point3D"/>.</param>
        /// <param name="point2">The second <see cref="Point3D"/>.</param>
        /// <returns>A <see cref="Vector3D"/> representing the difference.</returns>
        public static Vector3D operator -(Point3D point1, Point3D point2)
        {
            return new Vector3D(point1.AsVector - point2.AsVector);
        }

        /// <summary>
        /// Subtracts a <see cref="Vector3D"/> from a <see cref="Point3D"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> position.</param>
        /// <param name="direction">The <see cref="Vector3D"/> direction.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the subtraction.</returns>
        public static Point3D operator -(Point3D point, Vector3D direction)
        {
            return new Point3D(point.AsVector - (Vector3)direction);
        }

        /// <summary>
        /// Multiplies a <see cref="Point3D"/> by a scalar value.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance.</param>
        /// <param name="scale">The scalar value to multiply by.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the multiplication.</returns>
        public static Point3D operator *(Point3D point, float scale)
        {
            return new Point3D(point.AsVector * scale);
        }

        /// <summary>
        /// Multiplies a <see cref="Point3D"/> by a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance.</param>
        /// <param name="scale">The <see cref="Vector3d"/> to multiply by.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the multiplication.</returns>
        public static Point3D operator *(Point3D point, Vector3d scale)
        {
            return new Point3D(point.AsVector * scale);
        }

        /// <summary>
        /// Divides a <see cref="Point3D"/> by a scalar value.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance.</param>
        /// <param name="scale">The scalar value to divide by.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the division.</returns>
        public static Point3D operator /(Point3D point, double scale)
        {
            return new Point3D(point.AsVector / scale);
        }

        /// <summary>
        /// Divides a <see cref="Point3D"/> by a <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> instance.</param>
        /// <param name="scale">The <see cref="Vector3d"/> to divide by.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the division.</returns>
        public static Point3D operator /(Point3D point, Vector3D scale)
        {
            return new Point3D(point.AsVector / scale.AsVector);
        }

        #endregion Operators

        /// <summary>
        /// Transforms a <see cref="Point3D"/> by applying the specified transformation matrix.
        /// </summary>
        /// <param name="point">The <see cref="Point3D"/> to transform.</param>
        /// <param name="transformation">The transformation matrix to apply.</param>
        /// <returns>A new <see cref="Point3D"/> resulting from the transformation.</returns>
        public static Point3D Transform(Point3D point, Matrix4d transformation)
        {
            return new Point3D(Vector3d.TransformPosition((Vector3d)point, transformation));
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
        /// Returns a string that represents the current point.
        /// </summary>
        /// <returns>A string in the format "[X, Y, Z]".</returns>
        public override string ToString()
        {
            return $"[{position.X}, {position.Y}, {position.Z}]";
        }
    }
}
