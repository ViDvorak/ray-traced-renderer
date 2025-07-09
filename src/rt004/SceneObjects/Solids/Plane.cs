using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Materials.Loading;
using rt004.Util;

namespace rt004.SceneObjects
{
    /// <summary>
    /// Represents an infinite plane solid in 3D space for ray tracing and intersection tests.
    /// </summary>
    public class Plane : Solid
    {
        BasicPlane plane;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class with the default material for the current lighting model.
        /// </summary>
        /// <param name="parentScene">The parent scene to which this plane belongs.</param>
        /// <param name="position">The position of the plane in world coordinates.</param>
        /// <param name="rotation">The rotation of the plane in Euler angles (degrees or radians as per convention).</param>
        public Plane(Scene parentScene, Point3D position, Vector3 rotation)
            : this(parentScene, position, rotation, Material.GetMaterialFor(RendererSettings.lightModel))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class with a specified material.
        /// </summary>
        /// <param name="parentScene">The parent scene to which this plane belongs.</param>
        /// <param name="position">The position of the plane in world coordinates.</param>
        /// <param name="rotation">The rotation of the plane in Euler angles (degrees or radians as per convention).</param>
        /// <param name="material">The material to use for rendering the plane.</param>
        /// <exception cref="InvalidCastException">Thrown if the provided material is not compatible with the current renderer settings.</exception>
        public Plane(Scene parentScene, Point3D position, Vector3 rotation, Material material) : base(parentScene, position, rotation, material)
        {
            if (material.GetType() != Material.GetMaterialFor(RendererSettings.lightModel).GetType())
                throw new InvalidCastException("Set material is not of the same type as set in renderer");

            plane = new BasicPlane(position, new Vector3D(Vector3d.TransformNormal(Vector3.UnitZ, Extensions.RotationMatrix(rotation))));
        }

        /// <summary>
        /// Gets the normal vector of the plane at the specified global position.
        /// </summary>
        /// <param name="globalPosition">The position on the plane (unused, as the normal is constant).</param>
        /// <returns>The normal vector of the plane.</returns>
        public override Vector3D GetNormalAt(Point3D globalPosition)
        {
            return plane.Normal;
        }

        /// <summary>
        /// Attempts to compute the intersection between a ray and this plane.
        /// </summary>
        /// <param name="line">The ray to test for intersection.</param>
        /// <param name="parameter">If intersection occurs, contains the distance along the ray to the intersection point.</param>
        /// <returns>True if the ray intersects the plane; otherwise, false.</returns>
        public override bool TryGetRayIntersection(Ray line, out double parameter)
        {
            return Geometry.TryToIntersect(line, plane, out parameter);
        }

        /// <summary>
        /// Attempts to compute the intersection between a ray and this plane, and calculates the UV coordinates at the intersection point.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="distance">If intersection occurs, contains the distance along the ray to the intersection point.</param>
        /// <param name="uvCoord">If intersection occurs, contains the UV coordinates at the intersection point on the plane.</param>
        /// <returns>True if the ray intersects the plane; otherwise, false.</returns>
        public override bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord)
        {
            bool hasIntersected = TryGetRayIntersection(ray, out distance);
            var intersection = ray.GetPointOnRay(distance);

            uvCoord = Point2D.Zero;
            if (hasIntersected)
            {
                var xAxis = ToWorldSpace(Vector3D.OneX);
                var yAxis = ToWorldSpace(Vector3D.OneY);
                uvCoord = new Point2D(Geometry.LinearCombination(plane.PointOnPlane, yAxis, xAxis, intersection));
            }

            return hasIntersected;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    /// <summary>
    /// Loader class for deserializing and instantiating <see cref="Plane"/> objects from scene data.
    /// </summary>
    public class PlaneLoader : SolidLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneLoader"/> class.
        /// </summary>
        public PlaneLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="position">The position of the plane.</param>
        /// <param name="rotation">The rotation of the plane.</param>
        /// <param name="material">The material loader for the plane.</param>
        public PlaneLoader(Point3D position, Vector3 rotation, MaterialLoader material) : base(position, rotation, material) { }

        /// <summary>
        /// Creates an instance of a <see cref="Plane"/> using the loader's configuration.
        /// </summary>
        /// <param name="parentScene">The parent scene to which the plane will be added.</param>
        /// <returns>A new <see cref="Plane"/> instance.</returns>
        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Plane(parentScene, new Point3D(position), rotation, material.CreateInstance());
        }
    }
}
