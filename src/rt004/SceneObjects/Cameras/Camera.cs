using OpenTK.Mathematics;
using rt004.Util;
using System.Xml.Serialization;
using Util;

namespace rt004.SceneObjects
{
    /// <summary>
    /// Represents an abstract base class for cameras within the scene, defining basic properties and methods.
    /// </summary>
    public abstract class Camera : SceneObject
    {
        /// <summary>
        /// The width of the camera in pixels.
        /// </summary>
        protected uint width;

        /// <summary>
        /// The height of the camera in pixels.
        /// </summary>
        protected uint height;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class with specified parameters.
        /// </summary>
        /// <param name="parentScene">The parent scene to which this camera belongs.</param>
        /// <param name="position">The position of the camera within the scene.</param>
        /// <param name="rotation">The rotation of the camera within the scene.</param>
        /// <param name="width">The width of the camera resolution in pixels.</param>
        /// <param name="height">The height of the camera resolution in pixels.</param>
        public Camera(Scene parentScene, Point3D position, Vector3 rotation, uint width, uint height) : base(parentScene, position, rotation)
        {
            SetResolution(width, height);
        }

        /// <summary>
        /// Renders an image of the scene from the camera's perspective.
        /// </summary>
        /// <returns>A <see cref="FloatImage"/> representing the rendered scene image.</returns>
        public abstract FloatImage RenderImage();

        /// <summary>
        /// The maximum rendering distance squared, defining how far the camera can render objects.
        /// </summary>
        protected float maxRenderingDistanceSquered = float.MaxValue;

        /// <summary>
        /// Gets or sets the maximum rendering distance of the camera.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the width of the camera's resolution.
        /// Setting this property updates the resolution.
        /// </summary>
        public uint Width
        {
            get { return width; }
            set { SetResolution(value, height); }
        }

        /// <summary>
        /// Gets or sets the height of the camera's resolution.
        /// Setting this property updates the resolution.
        /// </summary>
        public uint Height
        {
            get { return height; }
            set { SetResolution(width, value); }
        }

        /// <summary>
        /// Sets the resolution of the camera.
        /// </summary>
        /// <param name="width">The width of the resolution in pixels.</param>
        /// <param name="height">The height of the resolution in pixels.</param>
        public abstract void SetResolution(uint width, uint height);

        /// <summary>
        /// Gets the current resolution of the camera.
        /// </summary>
        /// <returns>A tuple of <c>uint</c> representing the width and height, respectively.</returns>
        public (uint, uint) GetResolution()
        {
            return (width, height);
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    /// <summary>
    /// Abstract base class for loading camera configurations from XML or other serialized formats.
    /// </summary>
    [XmlInclude(typeof(PerspectiveCameraLoader))]
    public abstract class CameraLoader : SceneObjectLoader
    {
        /// <summary>
        /// The width of the camera, defaulting to the settings defined in <see cref="RendererSettings"/>.
        /// </summary>
        public uint width = RendererSettings.defaultCameraWidth;

        /// <summary>
        /// The height of the camera, defaulting to the settings defined in <see cref="RendererSettings"/>.
        /// </summary>
        public uint height = RendererSettings.defaultCameraHeight;

        /// <summary>
        /// The background color of the camera view.
        /// </summary>
        public Color4 backgroundColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraLoader"/> class with default settings.
        /// </summary>
        public CameraLoader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="position">The position of the camera in the scene.</param>
        /// <param name="rotation">The rotation of the camera in the scene.</param>
        /// <param name="backgroundColor">The background color for the camera.</param>
        /// <param name="width">The width of the camera's resolution in pixels.</param>
        /// <param name="height">The height of the camera's resolution in pixels.</param>
        public CameraLoader(Point3D position, Vector3 rotation, Color4 backgroundColor, uint width, uint height) : base(position, rotation)
        {
            this.backgroundColor = backgroundColor;
            this.width = width;
            this.height = height;
        }
    }
}
