using OpenTK.Mathematics;
using System.Dynamic;
using System.Xml.Serialization;

namespace rt004.Materials
{
    /// <summary>
    /// Represents texture with only one color chennel per pixel (gray-scale texture)
    /// </summary>
    abstract public class MonochromeTexture : Texture
    {
        public MonochromeTexture(uint u, uint v) : base(u, v) { }

        /// <summary>
        /// Gets the value at position
        /// </summary>
        /// <param name="u">horizontal coord</param>
        /// <param name="v">vertical coord</param>
        /// <returns>returns float value at the position</returns>
        abstract public float GetFactorAt(float u, float v);

        abstract public override Color4 GetColorAt(float u, float v);
    }
}

namespace rt004.Materials.Loading
{
    /// <summary>
    /// Abstract support class used for loading a monochrome texture.
    /// </summary>
    [XmlInclude(typeof(MonochromeImageTextureLoader)), XmlInclude(typeof(MonochromeUniformTextureLoader))]
    abstract public class MonochromeTextureLoader : TextureLoader
    {
        public MonochromeTextureLoader() { }
    }
}

