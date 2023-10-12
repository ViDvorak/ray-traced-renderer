using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Materials.Loading;
using rt004.Util;

namespace rt004.SceneObjects
{
    public class Plane : Solid
    {
        BasicPlane plane;

        public Plane(Scene parentScene, Point3D position, Vector3 rotation)
            : this(parentScene, position, rotation, Material.GetMaterialFor(RendererSettings.lightModel))
        { }

        public Plane(Scene parentScene, Point3D position, Vector3 rotation, Material material) : base(parentScene, position, rotation, material)
        {
            if (material.GetType() != Material.GetMaterialFor(RendererSettings.lightModel).GetType())
                throw new InvalidCastException("Set material is not of the same type as set in renderer");

            plane = new BasicPlane(position, new Vector3D( Vector3d.TransformNormal( Vector3.UnitZ, Extensions.RotationMatrix(rotation))));
        }

        public override Vector3D GetNormalAt(Point3D globalPosition)
        {
            return plane.Normal;
        }

        public override bool TryGetRayIntersection(Ray line, out double parameter)
        {
            return Geometry.TryToIntersect(line, plane, out parameter);
        }

        public override bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord)
        {
            bool hasIntersected = TryGetRayIntersection(ray, out distance);
            var intersection = ray.GetPointOnRay( distance);

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
    public class PlaneLoader : SolidLoader
    {
        public PlaneLoader() { }

        public PlaneLoader(Point3D position, Vector3 rotation, MaterialLoader material): base(position, rotation, material) { }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Plane(parentScene, new Point3D(position), rotation, material.CreateInstance());
        }
    }
}
