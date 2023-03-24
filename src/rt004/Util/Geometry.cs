using OpenTK.Mathematics;

namespace rt004.Util
{
    /// <summary>
    /// Represents line parametricly in 3D
    /// </summary>
    public record struct Line
    {
        public readonly Vector3 Position;
        /// <summary>
        /// Direction of line represented as vector with lenght of 1
        /// </summary>
        public readonly Vector3 Direction;

        public Line(Vector3 position, Vector3 direction)
        {
            this.Position = position;
            this.Direction = direction / direction.Length;
        }

        /// <summary>
        /// Computes point on the line with specified parameter
        /// </summary>
        /// <param name="parameter">Multiplier of direction vector</param>
        /// <returns>Point on the line</returns>
        public Vector3 GetPointOnLine(float parameter)
        {
            return Position + Direction * parameter;
        }
    }

    /// <summary>
    /// represents Plane in 3D
    /// </summary>
    public record struct PlaneStruct
    {
        public readonly Vector3 PointOnPlane;
        /// <summary>
        /// Normal vector represented as vector with lenght of 1
        /// </summary>
        public readonly Vector3 Normal;

        public PlaneStruct(Vector3 position, Vector3 normal)
        {
            this.PointOnPlane = position;
            this.Normal = normal / normal.LengthFast;
        }

        /// <summary>
        /// computes constant d from general equation of a plane (aX + bY + cZ + d = 0)
        /// </summary>
        /// <returns>Returns constant d</returns>
        public float GetD(){
            var vector = Normal * PointOnPlane;
            return - (vector.X + vector.Y + vector.Z);
        }
    }

    /// <summary>
    /// Contains Funstions for computation intersections in 3D space.
    /// </summary>
    internal static class Geometry
    {
        public static bool TryToIntresect(PlaneStruct plane1, PlaneStruct plane2, out Line intersection)
        {
            if (Vector3.Dot(plane1.Normal, plane2.Normal) == plane1.Normal.Length * plane2.Normal.Length)
            {
                intersection = new Line();
                return false;
            }
            Vector3 direction = Vector3.Cross(plane1.Normal, plane2.Normal);
            float x = (plane1.GetD() * plane2.Normal.Y - plane1.Normal.Y * plane2.GetD()) / (plane1.Normal.X * plane2.Normal.Y - plane1.Normal.Y * plane2.Normal.X);
            float y = (plane2.GetD() - plane2.Normal.X * x ) / plane2.Normal.Y;

            intersection = new Line( new Vector3(x,y,0), direction);

            return true;
        }

        public static bool TryToIntersect(Line line1, Line line2, out Vector3 intersection)
        {
            float a1, x1, b1, y1, c1, z1;
            a1 = line1.Direction.X;
            b1 = line1.Direction.Y;
            c1 = line1.Direction.Z;
            
            x1 = line1.Position.X;
            y1 = line1.Position.Y;
            z1 = line1.Position.Z;

            float a2, x2, b2, y2, c2, z2;
            a2 = line2.Direction.X;
            b2 = line2.Direction.Y;
            c2 = line2.Direction.Z;

            x2 = line2.Position.X;
            y2 = line2.Position.Y;
            z2 = line2.Position.Z;

            float param2 = (x1 - x2 + a1 / b1 * y2) / (a2 + a1 / b1 * b2);
            float param1 = (x2 - x1 + a1 * param2) / a1;

            var position1 = line1.Position + line1.Direction * param1;
            var position2 = line2.Position + line2.Direction * param2;

            intersection = position1;
            return position1.isVectorEquals(position2);
        }

        public static bool TryToIntersect(Line line, PlaneStruct plane, out float distance)
        {
            float aLine, xLine, bLine, yLine, cLine, zLine;
            aLine = line.Direction.X;
            bLine = line.Direction.Y;
            cLine = line.Direction.Z;

            xLine = line.Position.X;
            yLine = line.Position.Y;
            zLine = line.Position.Z;

            float aPlane, bPlane, cPlane;
            aPlane = plane.Normal.X;
            bPlane = plane.Normal.Y;
            cPlane = plane.Normal.Z;


            float cosineOfAngle = Vector3.Dot(-plane.Normal, line.Direction);
            // line is paralel to the Plane
            if (cosineOfAngle.isFloatEquals(0f) || cosineOfAngle < 0)
            {
                distance = 0;
                return false;
            }

            distance = (Vector3.Dot(plane.Normal, line.Position) + plane.GetD()) / cosineOfAngle;
            //distance = (aPlane * xLine + bPlane * yLine + cPlane * zLine + plane.GetD()) / (aPlane * aLine + bPlane * bLine + cPlane * cLine);
            return true;
        }
    }
}
