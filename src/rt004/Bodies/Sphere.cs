using OpenTK.Mathematics;

namespace rt004.Bodies
{
    internal class Sphere : Body
    {
        public readonly float diameter;

        public Sphere(Vector3 position, Vector3 rotation, Color4 color, float diameter) : base(position, rotation, color)
        {
            this.diameter = diameter;
        }

        public override bool TryGetRayIntersection(Vector3 point, Vector3 rayDirection, out Vector3 intersection)
        {
            var pointToObject = Position - point;

            intersection = Vector3.Dot(pointToObject, rayDirection) / rayDirection.Length * rayDirection + point;
            return (point - intersection).Length <= diameter;
        }
    }
}
