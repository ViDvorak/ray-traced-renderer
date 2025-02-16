using OpenTK.Mathematics;
using rt004.Util;
using rt004.Util.LightModels;
using Util;

namespace rt004.SceneObjects
{
    /// <summary>
    /// Represents a perspective camera that renders a 3D scene with a specified field of view (FoV).
    /// </summary>
    public class PerspectiveCamera : Camera
    {
        /// <summary>
        /// The background color of the camera view when no object is rendered.
        /// </summary>
        public Color4 backgroundColor = RendererSettings.defaultBackgroundColor;

        private const float maxFoV = float.Pi - float.Pi / 180; // 179° in radians
        private float fov;

        private double pixelDensity;

        /// <summary>
        /// Gets or sets the field of view in radians.
        /// Throws an exception if set outside the range (0, PI).
        /// </summary>
        public float FoV
        {
            get
            {
                return fov;
            }
            set
            {
                if (value >= maxFoV || value < 0)
                {
                    throw new ArgumentException($"FoV not in range (0, PI). Value was {value}");
                }
                fov = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerspectiveCamera"/> class with specified parameters.
        /// </summary>
        /// <param name="parentScene">The parent scene that this camera will render.</param>
        /// <param name="position">The position of the camera in the scene.</param>
        /// <param name="rotation">The rotation of the camera in the scene.</param>
        /// <param name="backgroundColor">The background color of the camera.</param>
        /// <param name="fov">The field of view in radians.</param>
        /// <param name="width">The horizontal resolution of the camera.</param>
        /// <param name="height">The vertical resolution of the camera.</param>
        public PerspectiveCamera(Scene parentScene, Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height)
            : base(parentScene, position, rotation, width, height)
        {
            this.backgroundColor = backgroundColor;
            FoV = fov; // in this case, FoV must be set before pixelDensity computation
            SetResolution(width, height);
        }

        /// <summary>
        /// Sets the resolution of the camera in pixels.
        /// Updates the pixel density based on the field of view and resolution.
        /// </summary>
        /// <param name="width">Number of pixels horizontally.</param>
        /// <param name="height">Number of pixels vertically.</param>
        public override void SetResolution(uint width, uint height)
        {
            this.width = width;
            this.height = height;
            pixelDensity = Math.Max(width, height) / (2 * Math.Tan(fov / 2));
        }

        /// <summary>
        /// Computes a ray in world space from coordinates in screen space.
        /// </summary>
        /// <param name="u">Width coordinate in pixels.</param>
        /// <param name="v">Height coordinate in pixels.</param>
        /// <returns>A <see cref="Ray"/> from the camera's global position to the target on the screen.</returns>
        private Ray GetRayFromScreenSpace(double u, double v)
        {
            var pixelVectorInObjectSpace = new Vector3D((u - width / 2) / pixelDensity, -(v - height / 2) / pixelDensity, 1);
            return new Ray(GlobalPosition, ToWorldSpace(pixelVectorInObjectSpace));
        }

        /// <summary>
        /// Renders an image of the scene from the perspective of this camera.
        /// </summary>
        /// <returns>A <see cref="FloatImage"/> containing the rendered image of the scene.</returns>
        public override FloatImage RenderImage()
        {
            float maxIntensity = 0;
            FloatImage image = new FloatImage((int)width, (int)height, 3);
            LightModelComputation lightModel = RendererSettings.lightModel.GetLightModelComputation();

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Vector4 pixelColor = Vector4.Zero;

                    // Anti-aliasing loop
                    double step = 1f / RendererSettings.AntialiasingFraquency;
                    int oddFactor = (RendererSettings.AntialiasingFraquency - 1) % 2;

                    for (int i = 0; i < RendererSettings.AntialiasingFraquency; ++i)
                    {
                        for (int j = 0; j < RendererSettings.AntialiasingFraquency; ++j)
                        {
                            Ray ray = GetRayFromScreenSpace(x + i * step + oddFactor * step / 2, y + j * step + oddFactor * step / 2);

                            // Sum pixel color contributions
                            if (ParentScene.CastRay(ray, out IntersectionProperties properties))
                            {
                                pixelColor += (Vector4)lightModel.ComputeLightColor(properties, backgroundColor, ParentScene.GetLightSources());
                            }
                        }
                    }

                    int pixelCount = RendererSettings.AntialiasingFraquency * RendererSettings.AntialiasingFraquency;

                    image.PutPixel(x, y, new float[] {
                        pixelColor[0] / pixelCount + backgroundColor.R,
                        pixelColor[1] / pixelCount + backgroundColor.G,
                        pixelColor[2] / pixelCount + backgroundColor.B
                    });
                }
            }

            Console.WriteLine($"Max specular intensity is {maxIntensity}");
            return image;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    /// <summary>
    /// Responsible for loading a <see cref="PerspectiveCamera"/> from specified parameters.
    /// </summary>
    public class PerspectiveCameraLoader : CameraLoader
    {
        /// <summary>
        /// The field of view to be loaded in degrees.
        /// </summary>
        public float fov;

        /// <summary>
        /// The name of the light model used in this camera setup.
        /// </summary>
        public string lightModelName = "";

        private bool hasCameraFoVBeenConverted = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerspectiveCameraLoader"/> class.
        /// </summary>
        public PerspectiveCameraLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerspectiveCameraLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="position">The position of the camera in the scene.</param>
        /// <param name="rotation">The rotation of the camera in the scene.</param>
        /// <param name="backgroundColor">The background color for the camera.</param>
        /// <param name="fov">The field of view in degrees.</param>
        /// <param name="width">The horizontal resolution of the camera.</param>
        /// <param name="height">The vertical resolution of the camera.</param>
        public PerspectiveCameraLoader(Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height)
            : base(position, rotation, backgroundColor, width, height)
        {
            this.fov = fov;
            hasCameraFoVBeenConverted = true;
        }

        /// <summary>
        /// Creates an instance of <see cref="PerspectiveCamera"/> within the specified parent scene.
        /// Converts field of view from degrees to radians if it has not yet been converted.
        /// </summary>
        /// <param name="parentScene">The parent scene in which this camera instance will be created.</param>
        /// <returns>A new instance of <see cref="PerspectiveCamera"/> configured with the loader's settings.</returns>
        public override SceneObject CreateInstance(Scene parentScene)
        {
            if (!hasCameraFoVBeenConverted)
                fov = fov / 180 * MathF.PI;

            return new PerspectiveCamera(parentScene, new Point3D(position), rotation, backgroundColor, fov, width, height);
        }
    }
}
