using OpenTK.Mathematics;

namespace rt004.Bodies
{
    internal abstract class Body
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 Rotation { 
            get { return rotation; }
            set {
                Vector3 tmp = value / MathF.PI;
                tmp.X = MathF.Floor(tmp.X);
                tmp.Y = MathF.Floor(tmp.Y);
                tmp.Z = MathF.Floor(tmp.Z);
                rotation = value - tmp * MathF.PI;
            }
        }
        public Color4 color;

        abstract public bool TryGetRayIntersection(Vector3 point, Vector3 rayDirection);
    }
}
