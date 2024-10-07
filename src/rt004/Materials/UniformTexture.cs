using OpenTK.Mathematics;

namespace rt004.Materials
{
    /// <summary>
    /// Represents texture with only one color at every position
    /// </summary>
    public class UniformTexture : Texture
    {
        Color4 color { get; set; }

        public UniformTexture(Color4 color) : base(1,1)
        {
            this.color = color;
        }

        public override Color4 GetColorAt(float u, float v)
        {
            return color;
        }
    }
}

namespace rt004.Materials.Loading
{
    public class UniformTextureLoader : TextureLoader
    {
        public Color4 color;

        public UniformTextureLoader()
        {
            color = Color4.White;
        }
        public UniformTextureLoader(Color4 color)
        {
            this.color = color;
        }

        public override Texture GetInstance()
        {
            return new UniformTexture(color);
        }
    }
}