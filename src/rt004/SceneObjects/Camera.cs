using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using System.Xml.Serialization;
using Util;

namespace rt004.SceneObjects
{
    public class Camera : SceneObject
    {
        public Color4 backgroundColor = RendererSettings.defaultBacgroundColor;

        private const float maxFoV = float.Pi - float.Pi / 180; // 179° in radians
        private float fov;


        public float FoV
        {
            get
            {
                return fov;
            }
            set
            {
                if (value >= maxFoV || value < 0) { throw new ArgumentException($"FoV not in range (0, PI) value was {value}"); }
                fov = value;
            }
        }

        public uint width, height;

        private float maxRenderingDistanceSquered = MathF.Sqrt(float.MaxValue);
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

        public Camera() : base(Vector3.Zero, Vector3.Zero)
        {
            this.backgroundColor = RendererSettings.defaultBacgroundColor;
            fov = float.Pi / 3;
            this.width = 1280;
            this.height = 720;
        }

        public Camera(Vector3 position, Vector3 rotation, Color4 bacgroudColor, float fov, uint width, uint height) : base(position, rotation)
        {
            backgroundColor = bacgroudColor;
            FoV = fov;
            this.width = width;
            this.height = height;
        }

        public void SetResolution(uint width, uint height)
        {
            this.width = width;
            this.height = height;
        }

        public FloatImage RenderImage(Solid[] RenderedBodies)
        {
            FloatImage image = new FloatImage((int)width, (int)height, 3);

            float distanceOfIntersectionSquared = maxRenderingDistanceSquered;

            for (int z = 0; z < width; ++z)
            {
                for (int y = 0; y < height; ++y)
                {
                    Color4 color = backgroundColor;

                    //z = (int)width / 2;
                    //y = (int)height / 2;

                    foreach (var body in RenderedBodies)
                    {
                        float pixelDensity = width / (2 * MathF.Tan(fov / 2));
                        Vector3 pixelVectorInObjectSpace = new Vector3(1, (y - height / 2) / pixelDensity, -(z - width / 2) / pixelDensity); // position == rayVector in object space
                        Vector3 pixelRayDirection = ConvertFromObjectToWorldSpace(pixelVectorInObjectSpace, false);

                        Line ray = new Line(Position, pixelRayDirection);

                        if (body.TryGetRayIntersection(ray, out Vector3 intersection))
                        {
                            var cameraToIntersection = intersection - Position;
                            if (cameraToIntersection.LengthSquared <= distanceOfIntersectionSquared)
                            {
                                distanceOfIntersectionSquared = cameraToIntersection.LengthSquared;
                                color = body.color;
                            }
                        }
                    }
                    image.PutPixel(z, y, new float[] { color.R, color.G, color.B });
                }
            }
            return image;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class CameraLoader : SceneObjectLoader
    {
        public Color4 backgroundColor;
        public float fov;

        public uint width, height;

        public CameraLoader() { }

        public CameraLoader(Vector3 position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height) : base(position, rotation)
        {
            this.backgroundColor = backgroundColor;
            this.fov = fov;
            this.width = width;
            this.height = height;
        }

        public override SceneObject CreateInstance()
        {
            return new Camera(position, rotation, backgroundColor, fov, width, height);
        }
    }
}
