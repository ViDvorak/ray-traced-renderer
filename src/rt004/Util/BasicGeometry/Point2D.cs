using OpenTK.Mathematics;

namespace rt004.Util
{
    /// <summary>
    /// Represents a 2D point with double precision, defined by X and Y coordinates.
    /// </summary>
    public readonly struct Point2D
    {
        /// <summary>
        /// The position of the point represented as a <see cref="Vector2d"/>.
        /// </summary>
        readonly Vector2d position;

        /// <summary>
        /// Gets the position of the point as a <see cref="Vector2d"/>.
        /// </summary>
        public readonly Vector2d AsVector { get => position; }

        /// <summary>
        /// Gets the X coordinate of the point.
        /// </summary>
        public double X { get => position.X; }

        /// <summary>
        /// Gets the Y coordinate of the point.
        /// </summary>
        public double Y { get => position.Y; }

        /// <summary>
        /// Gets the distance of the point from the origin.
        /// </summary>
        public readonly double DistanceFromOrigin { get => position.Length; }

        /// <summary>
        /// Gets the square of the distance of the point from the origin.
        /// </summary>
        public readonly double DistanceFromOriginSquared { get => position.LengthSquared; }

        /// <summary>
        /// Gets a <see cref="Point2D"/> instance representing the point (0, 0).
        /// </summary>
        public static Point2D Zero => new Point2D(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct from a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="position">The position of the point.</param>
        public Point2D(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct from a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="position">The position of the point.</param>
        public Point2D(Vector2d position)
        {
            this.position = (Vector2)position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct from the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        /// <param name="y">The Y coordinate of the point.</param>
        public Point2D(double x, double y)
        {
            this.position = new Vector2d(x, y);
        }

        /// <summary>
        /// Rounds the X and Y coordinates of the point up to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X and Y coordinates.</returns>
        public (int, int) RoundUp()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;

            int xCoord = position.X.IsFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = position.Y.IsFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            return (xCoord, yCoord);
        }

        /// <summary>
        /// Rounds the X and Y coordinates of the point down to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X and Y coordinates.</returns>
        public (int, int) RoundDown()
        {
            int xBasicRounding = (int)position.X;
            int yBasicRounding = (int)position.Y;

            int xCoord = position.X.IsFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = position.Y.IsFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            return (xCoord, yCoord);
        }

        /// <summary>
        /// Rounds the X and Y coordinates of a given point down.
        /// </summary>
        /// <param name="point">The point to round down.</param>
        /// <returns>A new <see cref="Point2D"/> instance with the rounded coordinates.</returns>
        public static Point2D RoundDown(Point2D point)
        {
            return new Point2D(point.RoundDown());
        }

        /// <summary>
        /// Rounds the X and Y coordinates of a given point up.
        /// </summary>
        /// <param name="point">The point to round up.</param>
        /// <returns>A new <see cref="Point2D"/> instance with the rounded coordinates.</returns>
        public static Point2D RoundUp(Point2D point)
        {
            return new Point2D(point.RoundUp());
        }

        #region Operators

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="dir">The <see cref="Point2D"/> instance to convert.</param>
        public static explicit operator Vector2(Point2D dir) => (Vector2)dir.position;

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> to a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="dir">The <see cref="Point2D"/> instance to convert.</param>
        public static explicit operator Vector2d(Point2D dir) => dir.position;

        /// <summary>
        /// Determines whether two <see cref="Point2D"/> instances are equal by comparing their X and Y coordinates.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return obj is not null && obj is Point2D point && Equals(point);
        }

        /// <summary>
        /// Determines whether this <see cref="Point2D"/> is equal to another <see cref="Point2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point2D"/> to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Point2D"/> has the same X and Y values as the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Point2D other)
        {
            return X.IsFloatEqual(other.X) && Y.IsFloatEqual(other.Y);
        }

        /// <summary>
        /// Compares two <see cref="Point2D"/> instances for equality.
        /// </summary>
        /// <param name="value">The first <see cref="Point2D"/> instance.</param>
        /// <param name="other">The second <see cref="Point2D"/> instance.</param>
        /// <returns>True if the X and Y coordinates are equal, false otherwise.</returns>
        public static bool operator ==(Point2D value, Point2D other)
        {
            return value.Equals(other);
        }

        /// <summary>
        /// Compares two <see cref="Point2D"/> instances for inequality.
        /// </summary>
        /// <param name="value">The first <see cref="Point2D"/> instance.</param>
        /// <param name="other">The second <see cref="Point2D"/> instance.</param>
        /// <returns>True if the X and Y coordinates are not equal, false otherwise.</returns>
        public static bool operator !=(Point2D value, Point2D other)
        {
            return !value.Equals(other);
        }

        /// <summary>
        /// Adds a <see cref="Vector2D"/> to a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> instance.</param>
        /// <param name="vector">The <see cref="Vector2D"/> instance.</param>
        /// <returns>A new <see cref="Point2D"/> resulting from the addition.</returns>
        public static Point2D operator +(Point2D point, Vector2D vector)
        {
            return new Point2D(point.AsVector + vector.AsVector);
        }

        /// <summary>
        /// Negates the coordinates of this <see cref="Point2D"/>, returning a new <see cref="Point2D"/> with the opposite values.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> instance to negate.</param>
        /// <returns>A new <see cref="Point2D"/> with X and Y values negated.</returns>
        public static Point2D operator -(Point2D point)
        {
            return new Point2D(-point.X, -point.Y);
        }

        /// <summary>
        /// Subtracts one <see cref="Point2D"/> from another, returning the result as a <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point2D"/>.</param>
        /// <param name="point2">The second <see cref="Point2D"/>.</param>
        /// <returns>A <see cref="Vector2D"/> representing the difference.</returns>
        public static Vector2D operator -(Point2D point1, Point2D point2)
        {
            return new Vector2D(point1.AsVector - point2.AsVector);
        }

        /// <summary>
        /// Subtracts a <see cref="Vector2D"/> from a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="position">The <see cref="Point2D"/> position.</param>
        /// <param name="direction">The <see cref="Vector2D"/> direction.</param>
        /// <returns>A new <see cref="Point2D"/> resulting from the subtraction.</returns>
        public static Point2D operator -(Point2D position, Vector2D direction)
        {
            return new Point2D(position.AsVector - direction.AsVector);
        }

        /// <summary>
        /// Multiplies a <see cref="Point2D"/> by a scalar value.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> instance.</param>
        /// <param name="scale">The scalar value to multiply by.</param>
        /// <returns>A new <see cref="Point2D"/> resulting from the multiplication.</returns>
        public static Point2D operator *(Point2D point, double scale)
        {
            return new Point2D(point.AsVector * scale);
        }

        /// <summary>
        /// Multiplies a <see cref="Point2D"/> by a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> instance.</param>
        /// <param name="scale">The <see cref="Vector2d"/> to multiply by.</param>
        /// <returns>A new <see cref="Point2D"/> resulting from the multiplication.</returns>
        public static Point2D operator *(Point2D point, Vector2D scale)
        {
            return new Point2D(point.AsVector * scale.AsVector);
        }

        /// <summary>
        /// Divides a <see cref="Point2D"/> by a scalar value.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> instance.</param>
        /// <param name="scale">The scalar value to divide by.</param>
        /// <returns>A new <see cref="Point2D"/> resulting from the division.</returns>
        public static Point2D operator /(Point2D point, double scale)
        {
            return new Point2D(point.AsVector / scale);
        }

        /// <summary>
        /// Divides a <see cref="Point2D"/> by a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="p">The <see cref="Point2D"/> instance.</param>
        /// <param name="scale">The <see cref="Vector2d"/> to divide by.</param>
        /// <returns>A new <see cref="Point2D"/> resulting from the division.</returns>
        public static Point2D operator /(Point2D p, Vector2D scale)
        {
            return new Point2D(p.AsVector / scale.AsVector);
        }

        #endregion Operators

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
        /// <returns>A string in the format "[X, Y]".</returns>
        public override string ToString()
        {
            return $"[{position.X}, {position.Y}]";
        }
    }
}
