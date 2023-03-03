using OpenTK.Mathematics;
using rt004.Bodies;
using Util;

namespace rt004
{
    internal class Camera : SceneObject
    {
        public Color4 beckgroundColor;

        private const float maxFoV = float.Pi - float.Pi / 180; // 179° in radians
        private float fov;
        

        public float FoV {
            get
            {
                return fov;
            }
            set
            {
                if (value >= maxFoV || value < 0 ) { throw new ArgumentException($"FoV not in range 0 - PI value was {value}"); }
                fov = value;
            }
        }

        private uint width, height;

        public uint Width {
            get { return width; }
            set { width = value; }
        }
        public uint Height {
            get { return height; }
            set { height = value; }
        }

        private float maxRenderingDistanceSquered = MathF.Sqrt( float.MaxValue);
        public float MaxRenderingDistance
        {
            get
            {
                return MathF.Sqrt(maxRenderingDistanceSquered);
            }
            set
            {
                maxRenderingDistanceSquered = value * value;
            }
        }

        public Camera(Vector3 position, Vector3 rotation, Color4 bacgroudColor, float fov, uint width, uint height) : base(position, rotation)
        {
            beckgroundColor = bacgroudColor;
            FoV = fov;
            this.width = width;
            this.height = height;
        }

        public void SetResolution(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        public FloatImage RenderImage(Body[] RenderedBodies)
        {
            FloatImage image = new FloatImage((int)Width, (int)Height, 3);

            float distanceOfIntersection = maxRenderingDistanceSquered;

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Color4 color = beckgroundColor;

                    foreach (var body in RenderedBodies)
                    {
                        var pixelDensity = Width / (2 * Math.Tan(fov / 2));
                        Vector3 pixelVectorInObjectSpace = new Vector3(x - width / 2, y - height / 2, 1); // position == rayVector in object space
                        Vector3 pixelRayDirection = ConvertFromObjectToWorldSpace(pixelVectorInObjectSpace);

                        if (body.TryGetRayIntersection(Position, pixelRayDirection, out Vector3 intersection))
                        {
                            var cameraToIntersection = intersection - Position;
                            if (cameraToIntersection.LengthSquared <= maxRenderingDistanceSquered)
                            {
                                distanceOfIntersection = cameraToIntersection.Length;
                                color = body.color;
                            }
                        }
                    }
                    image.PutPixel(x, y, new float[] { color.R, color.G, color.B });
                }
            }
            return image;
        }
    }
}
