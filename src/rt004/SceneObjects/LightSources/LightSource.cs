using OpenTK.Mathematics;
using rt004.SceneObjects;
using System.Xml.Serialization;

namespace rt004.SceneObjects
{
    
    public abstract class LightSource : SceneObject
    {
        float intensity;
        Color4 lightColor;

        public float Intensity {
            get { return intensity; }
            set { intensity = Math.Abs(value); }
        }

        public Color4 Color {
            get { return lightColor; }
            set { lightColor = value; }
        }

        public LightSource(Vector3 position, Vector3 rotation, Color4 color, float intensity) : base(position, rotation)
        {
            Color = color;
            Intensity = intensity;
        }

        abstract public bool isPointIluminated(Vector3 point, out float intensity);
    }
}

namespace rt004.SceneObjects.Loading
{
    [XmlInclude(typeof(PointLightLoader))]
    public abstract class LightSourceLoader : SceneObjectLoader
    {
        public float intensity;
        public Color4 lightColor;

        public LightSourceLoader()
        {

        }

        public LightSourceLoader(Vector3 position, Vector3 rotation, Color4 lightColor, float intensity) : base(position, rotation)
        {
            this.intensity = intensity;
            this.lightColor = lightColor;
        }
    }
}
