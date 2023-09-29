using OpenTK.Mathematics;
using rt004.Util;
using rt004.Util.LightModels;
using Util;

namespace rt004.SceneObjects
{
    public class PrespectiveCamera : Camera
    {
        public Color4 backgroundColor = RendererSettings.defaultBacgroundColor;

        private const float maxFoV = float.Pi - float.Pi / 180; // 179° in radians
        private float fov;

        private double pixelDensity;


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



        public PrespectiveCamera(Scene parentScene, Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height ) 
            : base(parentScene, position, rotation, width, height)
        {
            this.backgroundColor = backgroundColor;
            FoV = fov;
        }


        public override void SetResolution(uint width, uint height)
        {
            this.width = width;
            this.height = height;
            pixelDensity = Math.Max(width, height) / (2 * Math.Tan(fov / 2));
        }



        /// <summary>
        /// Computes ray in world space from coordiantes in screen space
        /// </summary>
        /// <param name="u">width coordinate in pixels</param>
        /// <param name="v">height coordinate in pixels</param>
        /// <returns>Returns line from camera GlobalPosition to GlobalPosition on screen</returns>
        private Ray GetRayFromScreenSpace(double u, double v)
        {
            var pixelVectorInObjectSpace = new Vector3D((u - width / 2) / pixelDensity, -(v - height / 2) / pixelDensity, 1);
            return new Ray( GlobalPosition, ToWorldSpace(pixelVectorInObjectSpace));
        }

        /// <summary>
        /// Renderes image from scene
        /// </summary>
        /// <returns>Returns FlaotImage containing redered image for scene.</returns>
        public override FloatImage RenderImage()
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
    public class PrespectiveCameraLoader : CameraLoader
    {
        public float fov;// loaded as degrees

        public string lightModelName;

        private bool hasCameraFoVBeenConverted = false;

        public PrespectiveCameraLoader() { }

        public PrespectiveCameraLoader(Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height)
            : base(position, rotation, backgroundColor, width, height)
        {
            this.fov = fov; hasCameraFoVBeenConverted = true;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            if (!hasCameraFoVBeenConverted)
                fov = fov / 180 * MathF.PI;
            return new PrespectiveCamera(parentScene, new Point3D(position), rotation, backgroundColor, fov, width, height);
        }
    }
}
