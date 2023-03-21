using OpenTK.Mathematics;
using rt004.Util;
using System.Net.Security;

namespace rt004.SceneObjects
{
    public class Sphere : Solid
    {
        public readonly float diameter;

        public Sphere() : base(Vector3.Zero, Vector3.Zero)
        {
            this.diameter = 1;
        }

        public Sphere(Vector3 position, Vector3 rotation, Color4 color, float diameter) : base(position, rotation, color)
        {
            this.diameter = diameter;
        }

        public override bool TryGetRayIntersection(Line ray, out float parameter)
        {
            // It is not needed to consider rotation in this case (sphere)

            // computes intersections using analitical equatuion of sphere
            float a = ray.Direction.X * ray.Direction.X + ray.Direction.Y * ray.Direction.Y + ray.Direction.Z * ray.Direction.Z;
            
            float xdif = ray.Position.X - Position.X;
            float ydif = ray.Position.Y - Position.Y;
            float zdif = ray.Position.Z - Position.Z;

            float b = 2 * ( xdif * ray.Direction.X + ydif * ray.Direction.Y + zdif * ray.Direction.Z);
            float c = xdif * xdif + ydif * ydif + zdif * zdif - diameter * diameter;

            // compute discriminant
            float d = MathF.Sqrt(b * b - 4 * a * c);

            if (d.isFloatEquals( 0f ))
            {
                parameter = (float)(-b / (2 * a));
            }

            else if (d > 0f)
            {
                float param1 = (float)(-b + d / 2 * a);
                float param2 = (float)(-b - d / 2 * a);

                parameter = param1 > param2 ? param2 : param1;
            }
            else
            {
                parameter = 0;
            }
            return a >= 0;
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

        public override SceneObject CreateInstance()
        {
            return new Sphere(position, rotation, color, diameter);
        }
    }
}
