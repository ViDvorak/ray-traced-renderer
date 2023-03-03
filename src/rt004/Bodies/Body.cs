using OpenTK.Mathematics;
using System.Net.Http.Headers;

namespace rt004.Bodies
{
    internal abstract class SceneObject
    {
        private Vector3 position;
        private Vector3 rotation;

        private Matrix4 transformMatrix;

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                transformMatrix = CreateTransformMatrix();
            }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                // modulo 2Pi
                Vector3 tmp = value / (2*MathF.PI);
                tmp.X = MathF.Floor(tmp.X);
                tmp.Y = MathF.Floor(tmp.Y);
                tmp.Z = MathF.Floor(tmp.Z);
                rotation = value - 2 * MathF.PI * tmp;

                transformMatrix = CreateTransformMatrix();
            }
        }

        public SceneObject(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            Rotation = rotation;

            transformMatrix = CreateTransformMatrix();
        }

        public Vector3 ConvertFromObjectToWorldSpace( Vector3 vectorInObjectSpace)
        {
            return Vector3.TransformVector(vectorInObjectSpace, transformMatrix);
        }

        private Matrix4 CreateTransformMatrix()
        {
            var rotationMatrix = Matrix4.CreateFromQuaternion(new Quaternion(rotation));
            var translationMatrix = Matrix4.CreateTranslation(position);
            return rotationMatrix * translationMatrix;
        }
    }

    internal abstract class Body : SceneObject
    {
        public Color4 color;

        public Body(Vector3 position, Vector3 rotation, Color4 color) : base(position, rotation)
        {
            this.color = color;
        }

        abstract public bool TryGetRayIntersection(Vector3 point, Vector3 rayDirection, out Vector3 intersection);

        public bool TryGetRayIntersection(Vector3 point, Vector3 rayDirection)
        {
            return TryGetRayIntersection(point, rayDirection, out Vector3 intersection);
        }
    }
}
