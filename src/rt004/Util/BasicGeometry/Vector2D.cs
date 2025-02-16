using OpenTK.Mathematics;

namespace rt004.Util
{
    /// <summary>
    /// Represents an immutable 2-dimensional vector.
    /// </summary>
    public struct Vector2D
    {
        /// <summary>
        /// The vector's internal representation as a <see cref="Vector2d"/>.
        /// </summary>
        Vector2d direction;

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        public double Length { get => direction.Length; }

        /// <summary>
        /// Gets the square of the vector's length (X*X + Y*Y), avoiding the cost of square root calculation.
        /// </summary>
        public double LengthSquared { get => direction.LengthSquared; }

        /// <summary>
        /// Returns the vector's internal representation as a <see cref="Vector2d"/>.
        /// </summary>
        public Vector2d AsVector { get => direction; }

        /// <summary>
        /// Gets the X coordinate of the vector.
        /// </summary>
        public double X { get => direction.X; }

        /// <summary>
        /// Gets the Y coordinate of the vector.
        /// </summary>
        public double Y { get => direction.Y; }

        /// <summary>
        /// Returns a new instance of a zero vector (0, 0).
        /// </summary>
        public static Vector2D Zero => new Vector2D(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct from a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="vector">The vector to initialize from.</param>
        public Vector2D(Vector2d vector)
        {
            direction = vector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct from the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the vector.</param>
        /// <param name="y">The Y coordinate of the vector.</param>
        public Vector2D(double x, double y)
        {
            direction = new Vector2d(x, y);
        }

        /// <summary>
        /// Returns a normalized (unit length) version of this vector.
        /// </summary>
        /// <returns>A new <see cref="Vector2D"/> with unit length.</returns>
        public Vector2D Normalized()
        {
            double length = Length;
            return new Vector2D(direction.X / length, direction.Y / length);
        }

        /// <summary>
        /// Normalizes the vector in place, updating its direction to unit length.
        /// </summary>
        public void Normalize()
        {
            direction /= Length;
        }

        /// <summary>
        /// Rounds up the X and Y coordinates of the vector to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X and Y coordinates.</returns>
        public (int X, int Y) RoundUp()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;

            int xCoord = direction.X.IsFloatEqual(xBasicRounding) ? xBasicRounding : xBasicRounding + 1;
            int yCoord = direction.Y.IsFloatEqual(yBasicRounding) ? yBasicRounding : yBasicRounding + 1;
            return (xCoord, yCoord);
        }

        /// <summary>
        /// Rounds down the X and Y coordinates of the vector to the nearest integer.
        /// </summary>
        /// <returns>A tuple containing the rounded X and Y coordinates.</returns>
        public (int X, int Y) RoundDown()
        {
            int xBasicRounding = (int)direction.X;
            int yBasicRounding = (int)direction.Y;

            int xCoord = direction.X.IsFloatEqual(xBasicRounding + 1) ? xBasicRounding + 1 : xBasicRounding;
            int yCoord = direction.Y.IsFloatEqual(yBasicRounding + 1) ? yBasicRounding + 1 : yBasicRounding;
            return (xCoord, yCoord);
        }

        /// <summary>
        /// Returns a new vector with the X and Y coordinates rounded down.
        /// </summary>
        /// <param name="point">The vector to round down.</param>
        /// <returns>A new <see cref="Vector2D"/> instance with rounded coordinates.</returns>
        public static Vector2D RoundDown(Vector2D point)
        {
            return new Vector2D(point.RoundDown());
        }

        /// <summary>
        /// Returns a new vector with the X and Y coordinates rounded up.
        /// </summary>
        /// <param name="point">The vector to round up.</param>
        /// <returns>A new <see cref="Vector2D"/> instance with rounded coordinates.</returns>
        public static Vector2D RoundUp(Vector2D point)
        {
            return new Vector2D(point.RoundUp());
        }

        #region Operators

        /// <summary>
        /// Explicitly converts a <see cref="Vector2D"/> to a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="dir">The <see cref="Vector2D"/> instance to convert.</param>
        public static explicit operator Vector2d(Vector2D dir) => dir.direction;

        /// <summary>
        /// Explicitly converts a <see cref="Vector2D"/> to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="dir">The <see cref="Vector2D"/> instance to convert.</param>
        public static explicit operator Vector2(Vector2D dir) => (Vector2)dir.direction;

        /// <summary>
        /// Determines whether this <see cref="Vector2D"/> is equal to another <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector2D"/> to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Vector2D"/> is equal to the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector2D other)
        {
            return X.IsFloatEqual(other.X) && Y.IsFloatEqual(other.Y);
        }

        /// <summary>
        /// Determines whether two <see cref="Vector2D"/> instances are equal by comparing their components.
        /// </summary>
        /// <param name="left">The first <see cref="Vector2D"/> instance.</param>
        /// <param name="right">The second <see cref="Vector2D"/> instance.</param>
        /// <returns><c>true</c> if the components of both vectors are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector2D left, Vector2D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="Vector2D"/> instances are not equal by comparing their components.
        /// </summary>
        /// <param name="left">The first <see cref="Vector2D"/> instance.</param>
        /// <param name="right">The second <see cref="Vector2D"/> instance.</param>
        /// <returns><c>true</c> if the components of both vectors are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector2D left, Vector2D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Overrides the default Equals method to compare the current instance with another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the object is a <see cref="Vector2D"/> and its components are equal to those of the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is not null && obj is Vector2D other && Equals(other);
        }


        /// <summary>
        /// Negates the vector, flipping its direction.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2D"/> instance to negate.</param>
        /// <returns>A new <see cref="Vector2D"/> pointing in the opposite direction.</returns>
        public static Vector2D operator -(Vector2D vector)
        {
            return new Vector2D(-(Vector2)vector);
        }

        /// <summary>
        /// Subtracts one <see cref="Vector2D"/> from another.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector to subtract from the first.</param>
        /// <returns>A new <see cref="Vector2D"/> representing the difference.</returns>
        public static Vector2D operator -(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.AsVector - vector2.AsVector);
        }

        /// <summary>
        /// Subtracts a <see cref="Point2D"/> from a <see cref="Vector2D"/>, resulting in a new <see cref="Point2D"/>.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2D"/> to subtract from.</param>
        /// <param name="point">The <see cref="Point2D"/> to be subtracted.</param>
        /// <returns>A new <see cref="Point2D"/> representing the difference.</returns>
        public static Point2D operator -(Vector2D vector, Point2D point)
        {
            return new Point2D(vector.X - point.X, vector.Y - point.Y);
        }

        /// <summary>
        /// Adds two <see cref="Vector2D"/> instances.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>A new <see cref="Vector2D"/> representing the sum of the two vectors.</returns>
        public static Vector2D operator +(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.direction + vector2.direction);
        }

        /// <summary>
        /// Adds a <see cref="Vector2D"/> to a <see cref="Point2D"/>, resulting in a new <see cref="Point2D"/>.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2D"/> to add.</param>
        /// <param name="point">The <see cref="Point2D"/> to which the vector is added.</param>
        /// <returns>A new <see cref="Point2D"/> representing the sum of the point and vector.</returns>
        public static Point2D operator +(Vector2D vector, Point2D point)
        {
            return new Point2D(point.X + vector.X, point.Y + vector.Y);
        }


        /// <summary>
        /// Multiplies two vectors component-wise.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The vector to scale by.</param>
        /// <returns>A new <see cref="Vector2D"/> representing the scaled vector.</returns>
        public static Vector2D operator *(Vector2D vector, Vector2D scale)
        {
            return new Vector2D(vector.AsVector * scale.AsVector);
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="direction">The vector to scale.</param>
        /// <param name="scale">The scalar multiplier.</param>
        /// <returns>A new <see cref="Vector2D"/> representing the scaled vector.</returns>
        public static Vector2D operator *(Vector2D direction, double scale)
        {
            return new Vector2D(direction.AsVector * scale);
        }

        /// <summary>
        /// Divides a vector by another vector component-wise.
        /// </summary>
        /// <param name="dir">The vector to divide.</param>
        /// <param name="scale">The vector to divide by.</param>
        /// <returns>A new <see cref="Vector2D"/> representing the divided vector.</returns>
        public static Vector2D operator /(Vector2D dir, Vector2D scale)
        {
            return new Vector2D(dir.AsVector / scale.AsVector);
        }

        /// <summary>
        /// Divides a <see cref="Vector2D"/> by a scalar value, resulting in a scaled <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2D"/> to scale down.</param>
        /// <param name="scale">The scalar divisor.</param>
        /// <returns>A new <see cref="Vector2D"/> representing the vector divided by the scalar.</returns>
        public static Vector2D operator /(Vector2D vector, double scale)
        {
            return new Vector2D(vector.X / scale, vector.Y / scale);
        }

        #endregion Operators

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product result as a double.</returns>
        public static double Dot(Vector2D vector1, Vector2D vector2)
        {
            return Vector2d.Dot(vector1.AsVector, vector2.AsVector);
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
        /// <returns>A string in the format "[X, Y]".</returns>
        public override string ToString()
        {
            return $"[{direction.X}, {direction.Y}]";
        }
    }
}
