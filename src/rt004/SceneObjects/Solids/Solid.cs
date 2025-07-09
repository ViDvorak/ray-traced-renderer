using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using System.Xml.Serialization;
using rt004.Materials;
using rt004.Materials.Loading;
using rt004.Util.LightModels;

namespace rt004.SceneObjects
{
    /// <summary>
    /// Abstract base class for all solid objects that can be rendered in a scene.
    /// Provides common functionality for ray-solid intersection tests and material properties.
    /// </summary>
    public abstract class Solid : SceneObject
    {
        /// <summary>
        /// Gets or sets the material that defines the visual properties of this solid.
        /// </summary>
        public Material material { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Solid"/> class with default material.
        /// </summary>
        /// <param name="parentScene">The scene that contains this solid.</param>
        /// <param name="position">The position of the solid in 3D space.</param>
        /// <param name="rotation">The rotation of the solid as Euler angles.</param>
        public Solid(Scene parentScene, Point3D position, Vector3 rotation) : base(parentScene, position, rotation)
        {
            material = Material.GetMaterialFor(RendererSettings.lightModel);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Solid"/> class with a specified material.
        /// </summary>
        /// <param name="parentScene">The scene that contains this solid.</param>
        /// <param name="position">The position of the solid in 3D space.</param>
        /// <param name="rotation">The rotation of the solid as Euler angles.</param>
        /// <param name="material">The material to assign to this solid.</param>
        /// <exception cref="InvalidCastException">Thrown when the material is not compatible with the current light model.</exception>
        public Solid(Scene parentScene, Point3D position, Vector3 rotation, Material material) : base(parentScene, position, rotation)
        {
            if (Material.GetMaterialFor(RendererSettings.lightModel).GetType() == material.GetType())
                this.material = material;
            else
                throw new InvalidCastException("Material is not usable for LightModel");
        }

        /// <summary>
        /// Gets normal of this object at specified position.
        /// </summary>
        /// <param name="globalPosition">Position, where the normal should be start on the solid side or edge. In world coordinates</param>
        /// <returns>Returns normal of the object at specified position</returns>
        abstract public Vector3D GetNormalAt(Point3D globalPosition);

        /// <summary>
        /// Tests whether a ray intersects with this solid and returns the distance to the intersection point.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="distance">When this method returns, contains the distance from the ray origin to the intersection point if an intersection exists.</param>
        /// <returns><c>true</c> if the ray intersects with this solid; otherwise, <c>false</c>.</returns>
        abstract public bool TryGetRayIntersection(Ray ray, out double distance);

        /// <summary>
        /// Computes closest intersection from line.Position
        /// </summary>
        /// <param name="ray">Ray to intersect with</param>
        /// <param name="intersection">closest intersection point</param>
        /// <returns>true if ray intersects with the object, otherwise false</returns>
        public bool TryGetRayIntersection(Ray ray, out Point3D intersection)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out double distance);
            intersection = ray.GetPointOnRay(distance);
            return isIntersecting;
        }

        /// <summary>
        /// Tests whether a ray intersects with this solid and returns the distance and UV coordinates of the intersection.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="distance">When this method returns, contains the distance from the ray origin to the intersection point if an intersection exists.</param>
        /// <param name="uvCoord">When this method returns, contains the UV texture coordinates at the intersection point if an intersection exists.</param>
        /// <returns><c>true</c> if the ray intersects with this solid; otherwise, <c>false</c>.</returns>
        abstract public bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord);

        /// <summary>
        /// Tests whether a ray intersects with this solid and returns the intersection point and UV coordinates.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="intersection">When this method returns, contains the 3D intersection point if an intersection exists.</param>
        /// <param name="uvCoord">When this method returns, contains the UV texture coordinates at the intersection point if an intersection exists.</param>
        /// <returns><c>true</c> if the ray intersects with this solid; otherwise, <c>false</c>.</returns>
        public bool TryGetRayIntersection(Ray ray, out Point3D intersection, out Point2D uvCoord)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out double distance, out uvCoord);
            intersection = ray.GetPointOnRay(distance);
            return isIntersecting;
        }

        /// <summary>
        /// Tests whether a ray intersects with this solid and returns complete intersection properties.
        /// </summary>
        /// <param name="ray">The ray to test for intersection.</param>
        /// <param name="intersection">When this method returns, contains the complete intersection properties if an intersection exists.</param>
        /// <returns><c>true</c> if the ray intersects with this solid; otherwise, <c>false</c>.</returns>
        public bool TryGetRayIntersection(Ray ray, out IntersectionProperties intersection)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out double distance, out Point2D uvCoord);
            var intersectionPoint = ray.GetPointOnRay(distance);


            intersection = new IntersectionProperties()
            {
                globalPosition = intersectionPoint,
                distance = distance,
                intersectedSolid = this,
                normal = GetNormalAt(intersectionPoint),
                uvCoordinates = uvCoord,
                incomingRay = ray
            };

            return isIntersecting;
        }
    }


    public record struct IntersectionProperties
    {
        /// <summary>
        /// The global position of the intersection point in world coordinates.
        /// </summary>
        public Point3D globalPosition;
        /// <summary>
        /// The surface normal vector at the intersection point.
        /// </summary>
        public Vector3D normal;
        /// <summary>
        /// The solid object that was intersected by the ray.
        /// </summary>
        public Solid intersectedSolid;
        /// <summary>
        /// The UV texture coordinates at the intersection point.
        /// </summary>
        public Point2D uvCoordinates;
        /// <summary>
        /// The distance from the ray origin to the intersection point.
        /// </summary>
        public double distance;
        /// <summary>
        /// The incoming ray that caused this intersection.
        /// </summary>
        public Ray incomingRay;
    }
}

namespace rt004.SceneObjects.Loading
{
    /// <summary>
    /// Abstract base class for loading solid objects from configuration data.
    /// Supports XML serialization for different types of solid loaders.
    /// </summary>
    [XmlInclude(typeof(SphereLoader)), XmlInclude(typeof(PlaneLoader))]
    public abstract class SolidLoader : SceneObjectLoader
    {
        /// <summary>
        /// Gets or sets the material loader that defines how to create the material for the solid.
        /// </summary>
        public MaterialLoader material;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidLoader"/> class.
        /// </summary>
        public SolidLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="position">The position of the solid in 3D space.</param>
        /// <param name="rotation">The rotation of the solid as Euler angles.</param>
        /// <param name="material">The material loader for creating the solid's material.</param>
        public SolidLoader(Point3D position, Vector3 rotation, MaterialLoader material) : base(position, rotation)
        {
            this.material = material;
        }
    }
}
