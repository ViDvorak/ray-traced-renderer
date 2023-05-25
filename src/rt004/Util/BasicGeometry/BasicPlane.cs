

using OpenTK.Mathematics;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace rt004.Util
{
    /// <summary>
    /// represents Plane in 3D
    /// </summary>
    public record struct BasicPlane
    {
        public readonly Point3D PointOnPlane;
        /// <summary>
        /// Normal vector represented as vector with lenght of 1
        /// </summary>
        public readonly Vector3D Normal;

        public BasicPlane(Point3D pointOnPlane, Vector3D normal)
        {
            this.PointOnPlane = pointOnPlane;
            normal.Normalize();
            this.Normal = normal;
        }

        /// <summary>
        /// computes constant d from general equation of a plane (aX + bY + cZ + d = 0)
        /// </summary>
        /// <returns>Returns constant d</returns>
        public double GetD()
        {
            return - Vector3d.Dot((Vector3d)Normal, (Vector3d)PointOnPlane);
        }

        public (Vector3D axisA, Vector3D axisB) GetAxisOnPlane()
        {
            var vector1 = Vector3D.OneX + Vector3D.OneZ;
            var vector2 = new Vector3D(1,1,1);

            var axis1 = Normal;
            var axis2 = vector1 - AxisProjection(axis1, vector1);
            var axis3 = vector2 - AxisProjection(axis2, vector2) - AxisProjection(axis1, vector2);

            return (axis2, axis3);
        }

        private Vector3D AxisProjection(Vector3D axis, Vector3D projectToAxis)
        {
            return axis * (Vector3D.Dot(axis, projectToAxis) / Vector3D.Dot(axis, axis));
        }
    }
}
