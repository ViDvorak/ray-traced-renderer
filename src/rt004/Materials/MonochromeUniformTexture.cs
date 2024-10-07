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
        /// <summary>
        /// Gets represented value coded to color.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns>Returns value coded to Color4 as (value, value, value, 0)</returns>
        public override Color4 GetColorAt(float u, float v)
        {
            return new Color4(value, value, value, 1);
        }

        /// <summary>
        /// Gets the represented value regardless of passed arguments
        /// </summary>
        /// <param name="u">horizonata coord</param>
        /// <param name="v">vertical coord</param>
        /// <returns></returns>
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
