using OpenTK.Mathematics;
using rt004.Util;

namespace rt004.SceneObjects
{
    public class Plane : Solid
    {
        PlaneStruct plane;

        public Plane(Scene parentScene, Vector3 position, Vector3 rotation) : this(parentScene, position, rotation, RendererSettings.defaultSolidColor) { }

        public Plane(Scene parentScene, Vector3 position, Vector3 rotation, Color4 color) : base(parentScene, position, rotation, color)
        {
            plane = new PlaneStruct(position, Vector3.Transform( Vector3.UnitZ, Rotation));
        }

        public override Vector3 GetNormalAt(Vector3 position)
        {
            return plane.Normal;
        }

        public override bool TryGetRayIntersection(Line line, out float parameter)
        {
            return Geometry.TryToIntersect(line, plane, out parameter);
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class PlaneLoader : SolidLoader
    {
        public Vector3 normal;

        public PlaneLoader() { }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Plane(parentScene, position, rotation);
        }
    }
}
