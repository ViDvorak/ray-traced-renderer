using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using System.Xml.Serialization;
using rt004.Materials;
using rt004.Materials.Loading;
using rt004.Util.LightModels;

namespace rt004.SceneObjects
{
    public abstract class Solid : SceneObject
    {
        public Material material { get; set; }

        public Solid(Scene parentScene, Point3D position, Vector3 rotation) : base(parentScene, position, rotation)
        {
            material = Material.GetMaterialFor(RendererSettings.lightModel);
        }

        public Solid(Scene parentScene, Point3D position, Vector3 rotation, Material material) : base(parentScene, position, rotation)
        {
            if (Material.GetMaterialFor(RendererSettings.lightModel).GetType() == material.GetType())
                this.material = material;
            else
                throw new InvalidCastException("Material is not usable for LightModel");
        }

        abstract public Vector3D GetNormalAt(Point3D position);

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

        abstract public bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord);

        public bool TryGetRayIntersection(Ray ray, out Point3D intersection, out Point2D uvCoord)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out double distance, out uvCoord);
            intersection = ray.GetPointOnRay(distance);
            return isIntersecting;
        }

        public bool TryGetRayIntersection(Ray ray, out IntersectionProperties intersection)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out double distance, out Point2D uvCoord);
            var intersectionPoint = ray.GetPointOnRay(distance);


            intersection = new IntersectionProperties()
            {
                position = intersectionPoint,
                distance = distance,
                intersectedSolid = this,
                normal = GetNormalAt(intersectionPoint),
                uvCoordinates = uvCoord,
                incommingRay = ray
            };

            return isIntersecting;
        }
    }


    public record struct IntersectionProperties
    {
        public Point3D position;
        public Vector3D normal;
        public Solid intersectedSolid;
        public Point2D uvCoordinates;
        public double distance;
        public Ray incommingRay;
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
