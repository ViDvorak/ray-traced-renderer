namespace rt004.Util
{
    /// <summary>
    /// Represents a line parametricly in 3D using Point and Vector
    /// </summary>
    public record struct Ray
    {
        /// <summary>
        /// origin point of the ray
        /// </summary>
        public readonly Point3D Origin;
        
        /// <summary>
        /// Direction of line represented as vector of length 1
        /// </summary>
        public readonly Vector3D Direction;

        public Ray(Point3D position, Vector3D direction)
        {
            this.Origin = position;

            direction.Normalize();
            this.Direction = direction;
        }

        /// <summary>
        /// Computes point on the line by multipling the parameter with normalized directional vector.
        /// </summary>
        /// <param name="distance">Distance, how far from position in direction of Direction is the point</param>
        /// <returns>Point on the line</returns>
        public Point3D GetPointOnRay(double distance)
        {
            return Origin + Direction * distance;
        }
    }
}
