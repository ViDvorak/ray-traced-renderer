using OpenTK.Mathematics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace rt004.Util
{
    /// <summary>
    /// Contains Funstions for computation intersections in 3D space.
    /// </summary>
    internal static class Geometry
    {

        /// <summary>
        /// Computes intersection of two BasicPlanes. resulting in ray.
        /// </summary>
        /// <param name="plane1"></param>
        /// <param name="plane2"></param>
        /// <param name="intersection"></param>
        /// <returns></returns>
        public static bool TryToIntersect(BasicPlane plane1, BasicPlane plane2, out Ray intersection)
        {
            if (Vector3D.Dot(plane1.Normal, plane2.Normal).isFloatEqual(plane1.Normal.Length * plane2.Normal.Length))
            {
                intersection = new Ray();
                return false;
            }
            Vector3D direction = Vector3D.Cross(plane1.Normal, plane2.Normal);

            double x = (plane1.GetD() * plane2.Normal.Y - plane1.Normal.Y * plane2.GetD()) /
                            (plane1.Normal.X * plane2.Normal.Y - plane1.Normal.Y * plane2.Normal.X);

            double y = (plane2.GetD() - plane2.Normal.X * x ) / plane2.Normal.Y;

            intersection = new Ray( new Point3D( (float)x, (float)y, 0f), direction);

            return true;
        }

        /// <summary>
        /// Computes intersection of two rays.
        /// </summary>
        /// <param name="line1">first ray</param>
        /// <param name="line2">second ray</param>
        /// <param name="intersection">point of intersection</param>
        /// <returns>returns true if intersection exists, otherwise false</returns>
        public static bool TryToIntersect(Ray line1, Ray line2, out Point3D intersection)
        {
            double a1, x1, b1, y1, c1, z1;
            a1 = line1.Direction.X;
            b1 = line1.Direction.Y;
            c1 = line1.Direction.Z;
            
            x1 = line1.Origin.X;
            y1 = line1.Origin.Y;
            z1 = line1.Origin.Z;

            double a2, x2, b2, y2, c2, z2;
            a2 = line2.Direction.X;
            b2 = line2.Direction.Y;
            c2 = line2.Direction.Z;

            x2 = line2.Origin.X;
            y2 = line2.Origin.Y;
            z2 = line2.Origin.Z;

            double param2 = (x1 - x2 + a1 / b1 * y2) / (a2 + a1 / b1 * b2);
            double param1 = (x2 - x1 + a1 * param2) / a1;

            var position1 = line1.Origin + line1.Direction * param1;
            var position2 = line2.Origin + line2.Direction * param2;

            intersection = position1;
            return position1 == position2;
        }

        /// <summary>
        /// Computes distance of ray origin point to an intesection of the ray and BasicPlane
        /// </summary>
        /// <param name="ray">ray to intersect</param>
        /// <param name="plane">plane to intersect with the ray</param>
        /// <param name="distance">disatance of the intersection to the ray origin point</param>
        /// <returns>True if the ray has an intersection, otherwise false</returns>
        public static bool TryToIntersect(Ray ray, BasicPlane plane, out double distance)
        {
            double cosineOfAngle = Vector3D.Dot(plane.Normal, -ray.Direction);
            
            distance = (Vector3d.Dot((Vector3d)plane.Normal, (Vector3d)ray.Origin) + plane.GetD()) / cosineOfAngle;
            
            // line is paralel to the Plane
            if (cosineOfAngle.isFloatEqual(1f))
            {
                distance = 0;
                (var axis1, var axis2) = plane.GetAxisOnPlane();
                // is ray parallel and is origin point on plane
                return IsLinearCombination(plane.PointOnPlane, axis1, axis2, ray.Origin, out Vector2d scale);
            }
            return !double.IsNaN(distance) && distance >= 0d && !distance.isFloatEqual(0d);
        }

        /// <summary>
        /// Compures scale of two vectors such that scaled vectors added to startPoint will reach terget
        /// </summary>
        /// <param name="startPoint">point to add vectors to</param>
        /// <param name="vector1">vector to scale</param>
        /// <param name="vector2">vector to scale</param>
        /// <param name="target">point to reach by adding scaled vectors to startPoint</param>
        /// <returns>Returns vector representing scale of vector1 and vector2</returns>
        public static Vector2d LinearCombination(Point3D startPoint, Vector3D vector1, Vector3D vector2, Point3D target)
        {
            var scale = new Vector2d();
            scale.X = (float)((target.X - startPoint.X - vector2.X * (target.Y - startPoint.Y) / vector2.Y) / (vector1.X * (vector1.Y + vector1.X * vector2.Y)));
            scale.Y = (float)((target.Y - startPoint.Y - scale.X * vector1.Y) / vector2.Y);
            
            if (double.IsNaN(scale.X)){
                scale.X = 0;
            }
            if (double.IsNaN(scale.Y))
            {
                scale.Y = 0;
            }
            return scale;
        }

        /// <summary>
        /// Compures scale of two vectors such that scaled vectors added together will reach terget.
        /// </summary>
        /// <param name="vector1">vector to scale</param>
        /// <param name="vector2">vector to scale</param>
        /// <param name="target">point to reach by adding scaled vectors to startPoint</param>
        /// <returns>Returns vector representing scale of vector1 and vector2</returns>
        public static Vector2d LinearCombination(Vector3D vector1, Vector3D vector2, Point3D target) => LinearCombination(Point3D.Zero, vector1, vector2, target);

        /// <summary>
        /// Checks if target point can be reached by scaling vectors and adding them up with startPoint
        /// </summary>
        /// <param name="startPoint">Point to add scaled vectors to</param>
        /// <param name="vector1">vector to scale</param>
        /// <param name="vector2">vector to scale</param>
        /// <param name="target">Point to reach by adding scaled vector to startPoint</param>
        /// <param name="scale">returns vector with computed scale factors</param>
        /// <returns>true if target can be reached else false</returns>
        public static bool IsLinearCombination(Point3D startPoint, Vector3D vector1, Vector3D vector2, Point3D target, out Vector2d scale)
        {
            scale = LinearCombination(startPoint, vector1, vector2, target);
            return (scale.X * vector1.Z + scale.Y * vector2.Z + startPoint.Z).isFloatEqual( target.Z );// check if Z coordiante is equal to the resut linear combination
        }

        /// <summary>
        /// Checks if target point can be reached with scaled vectors by adding them up 
        /// </summary>
        /// <param name="startPoint">Point to add scaled vectors to</param>
        /// <param name="vector1">vector to scale</param>
        /// <param name="vector2">vector to scale</param>
        /// <param name="target">Point to reach by adding scaled vector to startPoint</param>
        /// <param name="scale">returns vector with computed scale factors</param>
        /// <returns>true if target can be reached else false</returns>
        public static bool IsLinearCombination(Vector3D vector1, Vector3D vector2, Point3D target, out Vector2d scale) => IsLinearCombination(Point3D.Zero , vector1, vector2, target, out scale);
    }
}
