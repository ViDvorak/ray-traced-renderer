using OpenTK.Mathematics;

namespace rt004.Bodies
{
    internal class Sphere : Body
    {
        public readonly float diameter;

        public Sphere(float diameter)
        {
            this.diameter = diameter;
        }

        public override bool TryGetRayIntersection(Vector3 point, Vector3 rayDirection)
        {
            throw new NotImplementedException();
        }
    }
}
