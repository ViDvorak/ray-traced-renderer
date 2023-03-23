using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004.SceneObjects
{
    [XmlInclude(typeof(Sphere)), XmlInclude(typeof(Block))]
    public abstract class Solid : SceneObject
    {
        public Color4 color;

        public Solid(Scene parentScene, Vector3 position, Vector3 rotation) : base(parentScene, position, rotation)
        {
            color = RendererSettings.defaultSolidColor;
        }

        public Solid(Scene parentScene, Vector3 position, Vector3 rotation, Color4 color) : base(parentScene, position, rotation)
        {
            this.color = color;
        }

        abstract public Vector3 GetNormalAt(Vector3 position);

        abstract public bool TryGetRayIntersection(Line line, out float parameter);

        /// <summary>
        /// Computes closest intersection from line.Position
        /// </summary>
        /// <param name="ray">Ray to intersect with</param>
        /// <param name="intersection">closest intersection point</param>
        /// <returns>true if ray intersects with the object, otherwise false</returns>
        public bool TryGetRayIntersection(Line ray, out Vector3 intersection)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out float parameter);
            intersection = ray.GetPointOnLine(parameter);
            return isIntersecting;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    [XmlInclude(typeof(SphereLoader)), XmlInclude(typeof(BlockLoader))]
    public abstract class SolidLoader : SceneObjectLoader
    {
        public Color4 color;

        public SolidLoader() { }

        public SolidLoader(Vector3 position, Vector3 rotation ,Color4 color) : base(position, rotation)
        {
            this.color = color;
        }
    }
}
