
namespace rt004.Util
{
    /// <summary>
    /// Represents line parametricly in 3D
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
        /// Computes point on the line with specified parameter
        /// </summary>
        /// <param name="distance">Distance, how far from position in direction of Direction is the point</param>
        /// <returns>Point on the line</returns>
        public Point3D GetPointOnRay(double distance)
        {
            return Origin + Direction * distance;
        }
    }
}
