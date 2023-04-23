using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using System.Xml.Serialization;
using rt004.Materials;
using rt004.Materials.Loading;

namespace rt004.SceneObjects
{
    public abstract class Solid : SceneObject
    {
        public Material material { get; set; }

        public Solid(Scene parentScene, Point3D position, Vector3 rotation) : base(parentScene, position, rotation)
        {
            material = new Material(null, null, null, null);
        }

        public Solid(Scene parentScene, Point3D position, Vector3 rotation, Material material) : base(parentScene, position, rotation)
        {
            this.material = material;
        }

        abstract public Vector3D GetNormalAt(Point3D position);

        abstract public bool TryGetRayIntersection(Ray line, out double distance);

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

        abstract public bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord);

        public bool TryGetRayIntersection(Ray ray, out Point3D intersection, out Point2D uvCoord)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out double distance, out uvCoord);
            intersection = ray.GetPointOnRay(distance);
            return isIntersecting;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    [XmlInclude(typeof(SphereLoader)), XmlInclude(typeof(PlaneLoader))]
    public abstract class SolidLoader : SceneObjectLoader
    {
        public MaterialLoader material;

        public SolidLoader() { }

        public SolidLoader(Point3D position, Vector3 rotation, MaterialLoader material) : base(position, rotation)
        {
            this.material = material;
        }
    }
}
