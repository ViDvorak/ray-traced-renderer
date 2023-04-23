using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Util;
using System.Net.Security;
using rt004.Materials.Loading;
using System.Xml.Linq;

namespace rt004.SceneObjects
{
    public class Sphere : Solid
    {
        public readonly float radius;

        private float radiusSquered;

        public Sphere(Scene parentScene, Point3D position, Vector3 rotation, Material material, float radius) : base(parentScene, position, rotation, material)
        {
            this.radius = radius;
            radiusSquered = this.radius * this.radius;
        }


        public override Vector3D GetNormalAt(Point3D position)
        {
            Vector3D normal = (position - Position);
            normal.Normalize();
            return normal;
        }


        public override bool TryGetRayIntersection(Ray ray, out double parameter)
        {
            // It is not needed to consider rotation in this case (sphere)

            // computes intersections
            Vector3D dif = Position - ray.Origin;
            //const float a = 1f; //Vector3.Dot(ray.Direction, ray.Direction) is always 1
            double b = -2 * Vector3D.Dot(dif, ray.Direction);
            double c = Vector3D.Dot(dif, dif) - radiusSquered;

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


        public override bool TryGetRayIntersection(Ray ray, out double distance, out Point2D uvCoord)
        {
            bool isIntersecting = TryGetRayIntersection(ray, out distance);
            
            if (isIntersecting){
                Point3D intersection = ray.GetPointOnRay(distance);
                Vector3D vector = intersection - Position;

                double vectorXZAngle = Math.Atan(vector.X / vector.Z);
                double vectorXYAngle = Math.Atan(vector.X / vector.Y);

                Vector4 objectRotation = Rotation.ToAxisAngle();

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
    public class SphereLoader : SolidLoader
    {
        public float diameter;

        public SphereLoader() { }

        public SphereLoader(Point3D position, Vector3 rotation, MaterialLoader material, float diameter) : base(position, rotation, material)
        {
            this.diameter = diameter;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Sphere(parentScene, new Point3D(position), rotation, material.CreateInstance(), diameter);
        }
    }
}
