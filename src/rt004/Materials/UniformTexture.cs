using OpenTK.Mathematics;

namespace rt004.Materials
{
    /// <summary>
    /// Represents a uniform texture with a single, consistent color across its entire surface.
    /// </summary>
    public class UniformTexture : Texture
    {
        /// <summary>
        /// The color of the texture, which is consistent across the entire surface.
        /// </summary>
        private Color4 color { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniformTexture"/> class with a specified color.
        /// </summary>
        /// <param name="color">The uniform color to be applied across the texture.</param>
        public UniformTexture(Color4 color) : base(1, 1)
        {
            this.color = color;
        }

        /// <summary>
        /// Gets the color of the texture at any specified UV coordinates.
        /// Since this is a uniform texture, the color is the same across the entire texture.
        /// </summary>
        /// <param name="u">The horizontal coordinate (ignored in this class).</param>
        /// <param name="v">The vertical coordinate (ignored in this class).</param>
        /// <returns>The uniform color of the texture.</returns>
        public override Color4 GetColorAt(float u, float v)
        {
            return color;
        }
    }
}

namespace rt004.Materials.Loading
{
    /// <summary>
    /// Represents a loader for creating <see cref="UniformTexture"/> instances with a specified color.
    /// </summary>
    public class UniformTextureLoader : TextureLoader
    {
        /// <summary>
        /// The color used to initialize the <see cref="UniformTexture"/>.
        /// </summary>
        public Color4 color;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniformTextureLoader"/> class with a default color (white).
        /// </summary>
        public UniformTextureLoader()
        {
            color = Color4.White;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniformTextureLoader"/> class with a specified color.
        /// </summary>
        /// <param name="color">The color to apply to the <see cref="UniformTexture"/>.</param>
        public UniformTextureLoader(Color4 color)
        {
            this.color = color;
        }

        /// <summary>
        /// Creates a new instance of <see cref="UniformTexture"/> with the specified color.
        /// </summary>
        /// <returns>A new <see cref="UniformTexture"/> instance initialized with the specified color.</returns>
        public override Texture GetInstance()
        {
            return new UniformTexture(color);
        }
    }
}
