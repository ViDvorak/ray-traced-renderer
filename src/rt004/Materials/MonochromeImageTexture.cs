using OpenTK.Mathematics;


namespace rt004.Materials
{
    /// <summary>
    /// Represents grayscale image texture
    /// </summary>
    public class MonochromeImageTexture : MonochromeTexture
    {
        float[,] data;

        public MonochromeImageTexture(uint u, uint v) : base(u,v)
        {
            data = new float[u, v];
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets bilinaerly interpolated value from near image pixel values.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>bilinearly interpolated value from image</returns>
        public override float GetFactorAt(float u, float v)
        {
            if ( u >= 0 && u <= data.GetLength(0) && v >= 0 && v <= data.GetLength(1))
            {
                throw new IndexOutOfRangeException("index is out of range");
            }

            var ulower = (int)u;
            var vlower = (int)v;

            var uupper = (int)(u + 1);
            var vupper = (int)(v + 1);

            var udiff = u - ulower;
            var vdiff = v - vlower;

            float linear1 = (udiff * data[ulower, vlower] + (1 - udiff) * data[ulower, vupper]);
            float linear2 = (udiff * data[uupper, vlower] + (1 - udiff) * data[uupper, vupper]);

            float bilinear = vdiff * linear1 + vdiff * linear2;

            return bilinear;
        }

        /// <summary>
        /// returns value represented in color
        /// </summary>
        /// <param name="u">horizontal coord</param>
        /// <param name="v">vertical coord</param>
        /// <returns>returns factor in Color4 represented as (value, value, value, 1)</returns>
        public override Color4 GetColorAt(float u, float v)
        {
            float intensity = GetFactorAt(u, v);
            return new Color4(intensity, intensity, intensity, 1);
        }
    }
}

namespace rt004.Materials.Loading
{
    public class MonochromeImageTextureLoader : MonochromeTextureLoader
    {
        string path = "";

        public MonochromeImageTextureLoader() { }

        public override Texture GetInstance()
        {
            throw new NotImplementedException("Monochrome image texture loading not implemented");
        }
    }
}
