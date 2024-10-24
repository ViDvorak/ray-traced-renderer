﻿using OpenTK.Mathematics;
using rt004.Util;
using rt004.Util.LightModels;
using Util;

namespace rt004.SceneObjects
{
    public class PerspectiveCamera : Camera
    {
        public Color4 backgroundColor = RendererSettings.defaultBacgroundColor;

        private const float maxFoV = float.Pi - float.Pi / 180; // 179° in radians
        private float fov;

        private double pixelDensity;

        /// <summary>
        /// Field of view in radians
        /// </summary>
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



        public PerspectiveCamera(Scene parentScene, Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height ) 
            : base(parentScene, position, rotation, width, height)
        {
            this.backgroundColor = backgroundColor;
            FoV = fov;
            SetResolution(width, height);//in this case there must be set FoV before pixelDensity computation
        }


        /// <summary>
        /// Sets resolution of the camera in pixels
        /// </summary>
        /// <param name="width">number of pixels horizontally</param>
        /// <param name="height">number of pixels vertically</param>
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

            FloatImage image = new FloatImage((int)width, (int)height, 3);

            LightModelComputation lightModel = RendererSettings.lightModel.GetLightModelComputation();

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Vector4 pixelColor = Vector4.Zero;

                    // anti-aliasing
                    double step = 1f / RendererSettings.AntialiasingFraquency;
                    int oddFactor = (RendererSettings.AntialiasingFraquency - 1) % 2;

                    for (int i = 0; i < RendererSettings.AntialiasingFraquency; ++i)
                    {
                        for (int j = 0; j < RendererSettings.AntialiasingFraquency; ++j)
                        {
                            Ray ray = GetRayFromScreenSpace(x + i * step + oddFactor * step / 2, y + j * step + oddFactor * step / 2);
                            // sum pixles
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
            Console.WriteLine($"max specular intensity is {maxIntensity}");
            return image;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class PerspectiveCameraLoader : CameraLoader
    {
        public float fov; //loaded as degrees

        public string lightModelName = "";

        private bool hasCameraFoVBeenConverted = false;

        public PerspectiveCameraLoader() { }

        public PerspectiveCameraLoader(Point3D position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height)
            : base(position, rotation, backgroundColor, width, height)
        {
            this.fov = fov; hasCameraFoVBeenConverted = true;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            if (!hasCameraFoVBeenConverted)
                fov = fov / 180 * MathF.PI;
            return new PerspectiveCamera(parentScene, new Point3D(position), rotation, backgroundColor, fov, width, height);
        }
    }
}
