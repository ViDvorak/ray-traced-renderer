using OpenTK.Mathematics;
using rt004.Util;
using rt004.Util.LightModels;
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

        private uint width, height;
        private double pixelDensity;


        public uint Width { 
            get { return width; }
            set { SetResolution(value, height); }
        }

        public uint Height {
            get { return height; }
            set { SetResolution(width, value); }
        }

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

        public Camera(Scene parentScene, Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height ) 
            : base(parentScene, position, rotation)
        {
            this.backgroundColor = backgroundColor;
            FoV = fov;
            SetResolution(width, height);
        }

        /// <summary>
        /// sets resolution of the camera
        /// </summary>
        /// <param name="width">camera width in pixels</param>
        /// <param name="height">camera height in pixels</param>
        public void SetResolution(uint width, uint height)
        {
            this.width = width;
            this.height = height;
            pixelDensity = Math.Max(width, height) / (2 * Math.Tan(fov / 2));
        }

        /// <summary>
        /// gets camera resolution.
        /// </summary>
        /// <returns>returns tuple of uints representing width and height respectivly</returns>
        public (uint, uint) GetResolution()
        {
            return (width, height);
        }

        /// <summary>
        /// Computes ray in world space from coordiantes in screen space
        /// </summary>
        /// <param name="u">width coordinate in pixels</param>
        /// <param name="v">height coordinate in pixels</param>
        /// <returns>Returns line from camera position to position on screen</returns>
        public Ray GetRayFromScreenSpace(double u, double v)
        {
            var pixelVectorInObjectSpace = new Vector3D((u - width / 2) / pixelDensity, -(v - height / 2) / pixelDensity, 1);
            return new Ray( Position, ConvertFromObjectToWorldSpace(pixelVectorInObjectSpace));
        }

        /// <summary>
        /// Renderes image from scene
        /// </summary>
        /// <returns>Returns FlaotImage containing redered image for scene.</returns>
        public FloatImage RenderImage()
        {
            float maxIntensity = 0;
            float debugX = 0, debugY = 449;

            FloatImage image = new FloatImage((int)width, (int)height, 3);

            LightModelComputation lightModel = RendererSettings.lightModel.GetLightModelComputation();

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (x == debugX && y == debugY)
                    { /* For Debug */ }

                    Color4 pixelColor = backgroundColor;
                    Ray ray = GetRayFromScreenSpace(x + 0.5, y + 0.5);

                    if (ParentScene.CastRay(ray, out IntersectionProperties properties))
                    {
                        pixelColor = lightModel.ComputeLightColor(properties, backgroundColor, ParentScene.GetLightSources());
                    }

                    image.PutPixel(x, y, new float[] { pixelColor.R, pixelColor.G, pixelColor.B });
                }
            }
            Console.WriteLine($"max specular intensity is {maxIntensity}");
            return image;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class CameraLoader : SceneObjectLoader
    {
        public Color4 backgroundColor;
        public float fov;// loaded as degrees

        public uint width, height;

        public string lightModelName;

        private bool hasCameraFoVBeenConverted = false;

        public CameraLoader() { }

        public CameraLoader(Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height)
            : base(position, rotation)
        {
            this.backgroundColor = backgroundColor;
            this.fov = fov; hasCameraFoVBeenConverted = true;
            this.width = width;
            this.height = height;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            if (!hasCameraFoVBeenConverted)
                fov = fov / 180 * MathF.PI;
            return new Camera(parentScene, new Point3D(position), rotation, backgroundColor, fov, width, height);
        }
    }
}
