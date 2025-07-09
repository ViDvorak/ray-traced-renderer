using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Util;
using rt004.Materials.Loading;

namespace rt004.SceneObjects
{
    /// <summary>
    /// Represents a mathematically perfect sphere solid object in a 3D scene.
    /// Provides ray-sphere intersection calculations and surface normal computation.
    /// </summary>
    public class Sphere : Solid
    {
        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        public readonly float radius;

        /// <summary>
        /// The precomputed squared radius value used for optimization in intersection calculations.
        /// </summary>
        private float radiusSquared;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        /// <param name="parentScene">The scene that contains this sphere.</param>
        /// <param name="position">The position of the sphere's center in 3D space.</param>
        /// <param name="rotation">The rotation of the sphere as Euler angles.</param>
        /// <param name="material">The material that defines the visual properties of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        public Sphere(Scene parentScene, Point3D position, Vector3 rotation, Material material, float radius) : base(parentScene, position, rotation, material)
        {
            this.radius = radius;
            radiusSquared = this.radius * this.radius;
        }

        /// <summary>
        /// Gets the surface normal vector at the specified position on the sphere.
        /// </summary>
        /// <param name="position">The position on the sphere surface in world coordinates.</param>
        /// <returns>The normalized surface normal vector pointing outward from the sphere at the specified position.</returns>
        public override Vector3D GetNormalAt(Point3D position)
        {
            Vector3D normal = (position - GlobalPosition);
            normal.Normalize();
            return normal;
        }

        /// <summary>
        /// Tests whether a ray intersects with this sphere and returns the distance to the closest intersection point.
        /// Uses the quadratic formula to solve for ray-sphere intersection.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="parameter">When this method returns, contains the distance from the ray origin to the closest intersection point if an intersection exists.</param>
        /// <returns><c>true</c> if the ray intersects with the sphere and the intersection is in front of the ray origin; otherwise, <c>false</c>.</returns>
        public override bool TryGetRayIntersection(Ray ray, out double parameter)
        {
            // It is not needed to consider rotation in this case (sphere)

            // computes intersections
            Vector3D dif = GlobalPosition - ray.Origin;

            // const float a = 1f;      // Vector3.Dot(ray.Direction, ray.Direction)     // is always 1
            double b = -2 * Vector3D.Dot(dif, ray.Direction);
            double c = Vector3D.Dot(dif, dif) - radiusSquared;

            // compute discriminant
             double discriminant = b * b - 4 * c;

            if (discriminant >= 0f)
            {
                double d = Math.Sqrt(discriminant);
                parameter = (float)((-b - d) / 2);
            }
            else
            {
                parameter = 0;
            }
            return parameter > 0;
        }

        /// <summary>
        /// Tests whether a ray intersects with this sphere and returns both the distance and UV texture coordinates.
        /// Calculates UV coordinates based on spherical coordinate mapping.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="distance">When this method returns, contains the distance from the ray origin to the intersection point if an intersection exists.</param>
        /// <param name="uvCoord">When this method returns, contains the UV texture coordinates at the intersection point if an intersection exists.</param>
        /// <returns><c>true</c> if the ray intersects with the sphere; otherwise, <c>false</c>.</returns>
        public override bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out distance);
            
            if (isIntersecting){
                Point3D intersection = ray.GetPointOnRay(distance);
                Vector3D vector = intersection - GlobalPosition;

                double vectorXZAngle = Math.Atan(vector.X / vector.Z);
                double vectorXYAngle = Math.Atan(vector.X / vector.Y);

                Vector4 objectRotation = new Vector4(Rotation);

                double XYrotation = objectRotation.Z + vectorXYAngle;
                double XZrotation = objectRotation.Y + vectorXZAngle;

                uvCoord = new Point2D((float)XYrotation, (float)XZrotation);
            }
            else
            {
                uvCoord = Point2D.Zero;
            }

            return isIntersecting;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    /// <summary>
    /// Provides functionality for loading sphere objects from configuration data.
    /// Supports XML serialization and deserialization of sphere properties.
    /// </summary>
    public class SphereLoader : SolidLoader
    {
        /// <summary>
        /// Gets or sets the diameter of the sphere to be created.
        /// </summary>
        public float diameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SphereLoader"/> class.
        /// </summary>
        public SphereLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SphereLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="position">The position of the sphere in 3D space.</param>
        /// <param name="rotation">The rotation of the sphere as Euler angles.</param>
        /// <param name="material">The material loader for creating the sphere's material.</param>
        /// <param name="diameter">The diameter of the sphere.</param>
        public SphereLoader(Point3D position, Vector3 rotation, MaterialLoader material, float diameter) : base(position, rotation, material)
        {
            this.diameter = diameter;
        }

        /// <summary>
        /// Creates a new <see cref="Sphere"/> instance from the loader's configuration.
        /// </summary>
        /// <param name="parentScene">The scene that will contain the created sphere.</param>
        /// <returns>A new <see cref="Sphere"/> instance configured with the loader's properties.</returns>
        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Sphere(parentScene, new Point3D(position), rotation, material.CreateInstance(), diameter);
        }
    }
}
