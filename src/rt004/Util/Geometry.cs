using OpenTK.Mathematics;

namespace rt004.Util
{
    record struct Line
    {
        public readonly Vector3 position;
        public readonly Vector3 direction;
    }

    record struct Plane
    {
        public readonly Vector3 PointOnPlane;
        public readonly Vector3 normal;
    }

    internal static class Geometry
    {
        public static bool TryToIntresect(Plane plane1, Plane plane2, out Line intersection)
        {
            throw new NotImplementedException();
        }

        public static bool TryToIntersect(Line line1, Line line2, out Vector3 intersection)
        {
            throw new NotImplementedException();
        }

        public static bool TryToIntersect(Line line, Plane plane, out Vector3 intersection)
        {
            throw new NotImplementedException();
        }
    }
}
