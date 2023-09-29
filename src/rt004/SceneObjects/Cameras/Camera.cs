using OpenTK.Mathematics;
using rt004.Util;
using System.Xml.Serialization;
using Util;

namespace rt004.SceneObjects
{
    public abstract class Camera : SceneObject
    {
        protected uint width, height;

        public Camera(Scene parentScene, Point3D position, Vector3 rotation, uint width, uint height) : base(parentScene, position, rotation)
        {
            SetResolution(width, height);
        }

        public abstract FloatImage RenderImage();


        protected float maxRenderingDistanceSquered = float.MaxValue;
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

        public uint Width
        {
            get { return width; }
            set { SetResolution(value, height); }
        }

        public uint Height
        {
            get { return height; }
            set { SetResolution(width, value); }
        }

        /// <summary>
        /// sets resolution of the camera
        /// </summary>
        /// <param name="width">camera width in pixels</param>
        /// <param name="height">camera height in pixels</param>
        public abstract void SetResolution(uint width, uint height);

        /// <summary>
        /// gets camera resolution.
        /// </summary>
        /// <returns>returns tuple of uints representing width and height respectivly</returns>
        public (uint, uint) GetResolution()
        {
            return (width, height);
        }

    }
}

namespace rt004.SceneObjects.Loading
{
    [XmlInclude(typeof(PrespectiveCameraLoader))]
    public abstract class CameraLoader : SceneObjectLoader
    {
        public uint width, height;
        public Color4 backgroundColor;

        public CameraLoader()
        {

        }

        public CameraLoader(Point3D position, Vector3 rotation, Color4 backgroundColor, uint width, uint height) : base(position, rotation)
        {
            this.backgroundColor = backgroundColor;
            this.width = width;
            this.height = height;
        }
    }
}