using OpenTK.Mathematics;

namespace rt004.Materials
{
    /// <summary>
    /// Represents a monochrome uniform texture with a single grayscale value across its surface.
    /// </summary>
    public class MonochromeUniformTexture : MonochromeTexture
    {
        /// <summary>
        /// The grayscale value of the texture, consistent across the entire surface.
        /// </summary>
        public float value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonochromeUniformTexture"/> class with a specified grayscale value.
        /// </summary>
        /// <param name="value">The grayscale intensity value for the texture.</param>
        public MonochromeUniformTexture(float value) : base(1, 1)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the color representation of the grayscale value at any UV coordinates.
        /// Since this is a uniform texture, the color is the same at all coordinates.
        /// </summary>
        /// <param name="u">The horizontal coordinate (ignored in this class).</param>
        /// <param name="v">The vertical coordinate (ignored in this class).</param>
        /// <returns>The color representation of the grayscale value, encoded as (value, value, value, 1).</returns>
        public override Color4 GetColorAt(float u, float v)
        {
            return new Color4(value, value, value, 1);
        }

        /// <summary>
        /// Gets the grayscale intensity factor of the texture at any UV coordinates.
        /// </summary>
        /// <param name="u">The horizontal coordinate (ignored in this class).</param>
        /// <param name="v">The vertical coordinate (ignored in this class).</param>
        /// <returns>The grayscale intensity factor.</returns>
        public override float GetFactorAt(float u, float v)
        {
            return value;
        }
    }
}

namespace rt004.Materials.Loading
{
    /// <summary>
    /// A loader class for creating instances of <see cref="MonochromeUniformTexture"/> with a specified grayscale value.
    /// </summary>
    public class MonochromeUniformTextureLoader : MonochromeTextureLoader
    {
        /// <summary>
        /// The grayscale value to be used for initializing the <see cref="MonochromeUniformTexture"/>.
        /// </summary>
        public float value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonochromeUniformTextureLoader"/> class with a default value of 0.
        /// </summary>
        public MonochromeUniformTextureLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonochromeUniformTextureLoader"/> class with a specified grayscale value.
        /// </summary>
        /// <param name="value">The grayscale value for the texture.</param>
        public MonochromeUniformTextureLoader(float value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MonochromeUniformTexture"/> with the specified grayscale value.
        /// </summary>
        /// <returns>A new <see cref="MonochromeUniformTexture"/> instance with the configured grayscale value.</returns>
        public override Texture GetInstance()
        {
            return new MonochromeUniformTexture(value);
        }
    }
}
