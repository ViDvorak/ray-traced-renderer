using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace rt004.Materials
{
    /// <summary>
    /// Represents a base class for textures, providing common properties and methods for textures.
    /// </summary>
    public abstract class Texture
    {
        /// <summary>
        /// The width of the texture in pixels.
        /// </summary>
        public readonly uint width;

        /// <summary>
        /// The height of the texture in pixels.
        /// </summary>
        public readonly uint height;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class with specified width and height.
        /// </summary>
        /// <param name="width">The width of the texture in pixels.</param>
        /// <param name="height">The height of the texture in pixels.</param>
        public Texture(uint width, uint height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets the color at a specified position in the texture.
        /// </summary>
        /// <param name="u">The horizontal coordinate, typically in normalized UV space.</param>
        /// <param name="v">The vertical coordinate, typically in normalized UV space.</param>
        /// <returns>The color at the specified (u, v) coordinates.</returns>
        public abstract Color4 GetColorAt(float u, float v);
    }
}

namespace rt004.Materials.Loading
{
    /// <summary>
    /// Represents a base class for loading textures, with support for XML serialization.
    /// </summary>
    [XmlInclude(typeof(UniformTextureLoader)), XmlInclude(typeof(MonochromeTextureLoader))]
    public abstract class TextureLoader
    {
        /// <summary>
        /// The width of the texture to load in pixels.
        /// </summary>
        public uint width;

        /// <summary>
        /// The height of the texture to load in pixels.
        /// </summary>
        public uint height;

        /// <summary>
        /// Creates an instance of a <see cref="Texture"/> based on the current settings.
        /// </summary>
        /// <returns>A new instance of <see cref="Texture"/> configured with specified width and height.</returns>
        public abstract Texture GetInstance();
    }
}
