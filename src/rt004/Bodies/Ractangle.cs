using OpenTK.Mathematics;
using rt004.Util;

namespace rt004.Bodies
{
    internal class Ractangle : Body
    {
        public readonly float sideA;
        public readonly float sideB;


        public Ractangle(float sideA, float sideB)
        {
            this.sideA = sideA;
            this.sideB = sideB;
        }

        public override bool TryGetRayIntersection(Vector3 point, Vector3 rayDirection)
        {
            throw new NotImplementedException();
        }
    }
}
