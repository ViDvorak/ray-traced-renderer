using OpenTK.Mathematics;
using rt004.Bodies;
using Util;

namespace rt004
{
    internal class Camera
    {
        public Vector3 position;
        private Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                Vector3 tmp = value / MathF.PI;
                tmp.X = MathF.Floor(tmp.X);
                tmp.Y = MathF.Floor(tmp.Y);
                tmp.Z = MathF.Floor(tmp.Z);
                rotation = value - tmp * MathF.PI;
            }
        }

        public Color4 beckgroundColor;
        public float FoV;

        private uint width, height;

        public uint Width { get { return width; } }
        public uint Height { get { return height; } }

        public void SetResolution(uint width, uint height)
        {
            this.width  = width;
            this.height = height;
        }

        public FloatImage RenderImage(Body[] RenderedBodies)
        {
            throw new NotImplementedException();
        }

    }
}
