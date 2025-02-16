using OpenTK.Mathematics;
using rt004.Util;

namespace rt004
{
    public static class Extensions
    {
        /// <summary>
        /// Compares two double values with +- RendererSettings.epsilon precision.
        /// </summary>
        /// <param name="value">Value to compare with</param>
        /// <param name="other">Value to compare</param>
        /// <returns>Returns true is other is in value +- epsilon range, else returns false</returns>
        public static bool IsFloatEqual(this double value, double other)
        {
            return value + RendererSettings.epsilon > other && value - RendererSettings.epsilon < other;
        }

        /// <summary>
        /// Compares two float values with +- RendererSettings.epsilon precision.
        /// </summary>
        /// <param name="value">Value to compare with</param>
        /// <param name="other">Value to compare</param>
        /// <returns>Returns true is other is in value +- epsilon range, else returns false</returns>
        public static bool IsFloatEqual(this float value, float other)
        {
            return value + RendererSettings.epsilon > other && value - RendererSettings.epsilon < other;
        }

        /// <summary>
        /// Compares two 3D vectors with +- RendererSettings.epsilon precision.
        /// 
        /// By calling Double.IsFloatEqual on each of its components comparing with apropriate value from other vector.
        /// </summary>
        /// <param name="value">Vector to compare with</param>
        /// <param name="other">Vector to compare</param>
        /// <returns>Returns true is other is in value +- epsilon range, else returns false</returns>
        public static bool IsVectorEquals(this Vector3d vector, Vector3d other)
        {
            return  vector[0].IsFloatEqual(other[0]) &&
                    vector[1].IsFloatEqual(other[1]) &&
                    vector[2].IsFloatEqual(other[2]);
        }

        /// <summary>
        /// Creates rotation matrix from euler angles in X,Y,Z order.
        /// </summary>
        /// <param name="eulerAngles">Vector defining euler angles in X,Y,Z order</param>
        /// <returns>Returns rotation matrix</returns>
        public static Matrix4d RotationMatrix(Vector3d eulerAngles)
        {
            return Matrix4d.RotateX(eulerAngles.X) * Matrix4d.RotateY(eulerAngles.Y) * Matrix4d.RotateZ(eulerAngles.Z);
        }
    }
}
