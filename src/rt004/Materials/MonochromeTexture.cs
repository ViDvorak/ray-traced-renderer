using OpenTK.Mathematics;
using System.Dynamic;
using System.Xml.Serialization;

namespace rt004.Materials
{
    abstract public class MonochromeTexture : Texture
    {
        public MonochromeTexture(uint u, uint v) : base(u, v) { }

        abstract public float GetFactorAt(float u, float v);

        abstract public override Color4 GetColorAt(float u, float v);
    }
}

namespace rt004.Materials.Loading
{
    [XmlInclude(typeof(MonochromeImageTextureLoader)), XmlInclude(typeof(MonochromeUniformTextureLoader))]
    abstract public class MonochromeTextureLoader : TextureLoader
    {
        public MonochromeTextureLoader() { }
    }
}

