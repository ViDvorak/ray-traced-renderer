using OpenTK.Mathematics;
using rt004.Util;
using System.Net.Security;

namespace rt004.SceneObjects
{
    public class Sphere : Solid
    {
        public readonly float radius;

        private float radiusSquered;

        public Sphere(Scene parentScene, Vector3 position, Vector3 rotation, Color4 color, float radius) : base(parentScene, position, rotation, color)
        {
            this.radius = radius;
            radiusSquered = this.radius * this.radius;
        }

        public override bool TryGetRayIntersection(Line ray, out float parameter)
        {
            // It is not needed to consider rotation in this case (sphere)

            // computes intersections
            Vector3 dif = Position - ray.Position;
            //const float a = 1f; //Vector3.Dot(ray.Direction, ray.Direction) is always 1
            float b = -2 * Vector3.Dot(dif, ray.Direction);
            float c = Vector3.Dot(dif, dif) - radiusSquered;

            // compute discriminant
             float discriminant = b * b - 4 * c;

            if (discriminant >= 0f)
            {
                float d = MathF.Sqrt(discriminant);
                parameter = (-b - d) / 2;
            }
            else
            {
                parameter = 0;
            }
            return parameter > 0;
        }


        public override Vector3 GetNormalAt(Vector3 position)
        {
            Vector3 normal = (position - Position);
            normal.Normalize();
            return normal;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class SphereLoader : SolidLoader
    {
        public float diameter;

        public SphereLoader() { }

        public SphereLoader(Vector3 position, Vector3 rotation, Color4 color, float diameter) : base(position, rotation, color)
        {
            this.diameter = diameter;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Sphere(parentScene, position, rotation, color, diameter);
        }
    }
}
