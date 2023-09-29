using OpenTK.Mathematics;
using System.Xml.Serialization;
using rt004.Util;
using System.Dynamic;

namespace rt004.SceneObjects
{
    public abstract class SceneObject
    {
        protected Scene parentScene;
        private InnerSceneObject parentObject;

        private Point3D position;
        private Point3D globalPosition;

        protected bool dirtyGlobalPosition = true;

        public Point3D Position
        {
            get => position; 
            set {
                position = value;
                dirtyGlobalPosition = true;

                if (this is InnerSceneObject parent)
                {
                    foreach (SceneObject child in parent.GetChildren())
                    {
                        child.dirtyGlobalPosition = true;
                    }
                }
            }
        }

        public Point3D GlobalPosition
        {
            set => dirtyGlobalPosition = true;
            get {
                if (dirtyGlobalPosition)
                {
                    globalPosition = GetGlobalPosition();
                    dirtyGlobalPosition = false;
                }
                return globalPosition;
            }
        }

        public Quaternion Rotation { get; set; }

        public virtual Scene ParentScene
        {
            get => parentScene;
            set => parentScene = value;
        }

        public InnerSceneObject ParentObject
        {
            get => parentObject;
            set => parentObject = value;
        }

        public SceneObject(Scene parentScene, Point3D position, Vector3 rotation)
        {
            this.parentScene = parentScene;
            Position = position;
            Rotation = new Quaternion(rotation);
        }

        public Vector3d GetLocalHeding()
        {
            return Vector3d.TransformPosition( Vector3d.UnitX, GetLocalTransformMatrix());
        }

        public Vector3d GetGlobalHeading()
        {
            return Vector3d.TransformPosition(Vector3d.UnitX, GetLocalToWorldTransformMatrix());
        }

        public Point3D GetGlobalPosition()
        {
            Matrix4d transformMatrix = GetLocalToWorldTransformMatrix();
            
            return new Point3D (transformMatrix.ExtractTranslation());
        }

        /// <summary>
        /// Gets transform matrix from local to world space 
        /// </summary>
        /// <returns>Matrix containing the transformation</returns>
        public virtual Matrix4d GetLocalToWorldTransformMatrix()
        {
            Matrix4d matrix;
            if (parentObject is not null)
                matrix = GetLocalTransformMatrix() * parentObject.GetLocalToWorldTransformMatrix();
            else
                matrix = GetLocalTransformMatrix();
            return matrix;
        }

        /// <summary>
        /// Gets transform matrix from world to local space 
        /// </summary>
        /// <returns>Matrix containing the transformation</returns>
        public virtual Matrix4d GetWorldToLocalTransformMatrix() => GetLocalToWorldTransformMatrix().Inverted();

        public Matrix4d GetLocalTransformMatrix()
        {
            return Matrix4d.CreateTranslation((float)Position.X, (float)Position.Y, (float)Position.Z) * Matrix4d.CreateFromQuaternion(new Quaterniond(Rotation.ToEulerAngles()));
        }

        public Matrix4d GetLocalInverseTransformMatrix() => GetLocalTransformMatrix().Inverted();


        public Vector3D ToWorldSpace(Vector3D vectorInObjectSpace)
        {
            return Vector3D.Transform(vectorInObjectSpace, (Matrix4d)GetLocalToWorldTransformMatrix());
        }

        public Point3D ToWorldSpace(Point3D pointInObjectSpace)
        {
            return Point3D.Transform(pointInObjectSpace, GetLocalToWorldTransformMatrix());
        }
    }
}

namespace rt004.SceneObjects.Loading {

    /// <summary>
    /// Abstract class used for loading of SceneObject. SceneObjectLoader inheratance structure must be parallel to the SceneObject one.
    /// </summary>
    [XmlInclude(typeof(CameraLoader)), XmlInclude(typeof(SolidLoader)), XmlInclude(typeof(LightSourceLoader)), XmlInclude(typeof(InnerSceneObjectLoader))]
    public abstract class SceneObjectLoader
    {
        public Vector3 position;
        public Vector3 rotation;

        public SceneObjectLoader() { }

        public SceneObjectLoader(Point3D position, Vector3 rotation)
        {
            this.position = (Vector3)position;
            this.rotation = rotation;
        }



        /// <summary>
        /// Creates instance of corresponding SceneObject type
        /// </summary>
        /// <returns>instanciated object</returns>
        abstract public SceneObject CreateInstance(Scene parentScene);
    }
}
