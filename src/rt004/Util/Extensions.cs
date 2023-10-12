using OpenTK.Mathematics;
using rt004.Util;

namespace rt004
{
    public static class Extensions
    {
        public static bool isFloatEqual(this double value, double other)
        {
            return value + RendererSettings.epsilon > other && value - RendererSettings.epsilon < other;
        }

        public static bool isFloatEqual(this float value, float other)
        {
            return value + RendererSettings.epsilon > other && value - RendererSettings.epsilon < other;
        }


        public static bool isVectorEquals(this Vector3d vector, Vector3d other)
        {
            return  vector[0].isFloatEqual(other[0]) &&
                    vector[1].isFloatEqual(other[1]) &&
                    vector[2].isFloatEqual(other[2]);
        }

        public static Matrix4d RotationMatrix(Vector3 eulerAngles)
        {
            return Matrix4d.RotateX(eulerAngles.X) * Matrix4d.RotateY(eulerAngles.Y) * Matrix4d.RotateZ(eulerAngles.Z);
        }
    }
}
