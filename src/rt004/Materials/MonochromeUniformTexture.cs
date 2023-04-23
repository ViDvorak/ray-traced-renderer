using OpenTK.Mathematics;


namespace rt004.Materials
{
    public class MonochromeUniformTexture : MonochromeTexture
    {
        public float value;

        public MonochromeUniformTexture(float value) : base(1, 1)
        {
            this.value = value;
        }

        public override Color4 GetColorAt(float u, float v)
        {
            return new Color4(value, value, value, 1);
        }

        public override float GetFactorAt(float u, float v)
        {
            return value;
        }
    }
}

namespace rt004.Materials.Loading
{
    public class MonochromeUniformTextureLoader : MonochromeTextureLoader
    {
        public float value;

        public MonochromeUniformTextureLoader() { }

        public MonochromeUniformTextureLoader(float value)
        {
            this.value = value;
        }

        public override Texture GetInstance()
        {
            return new MonochromeUniformTexture(value);
        }
    }
}
