using OpenTK.Mathematics;
using rt004.Util;

namespace rt004
{
    public static class Extensions
    {
        public static bool isFloatEquals(this float value, float other)
        {
            return value + RendererSettings.floatPrecision > other && value - RendererSettings.floatPrecision < other;
        }

        public static bool isVectorEquals(this Vector3 vector, Vector3 other)
        {
            return  vector[0].isFloatEquals(other[0]) &&
                    vector[1].isFloatEquals(other[1]) &&
                    vector[2].isFloatEquals(other[2]);
        }
    }
}
