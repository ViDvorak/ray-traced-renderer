using OpenTK.Mathematics;
using System.Xml.Serialization;
using rt004.Util;

namespace rt004.SceneObjects
{
    public abstract class SceneObject
    {
        private Scene parentScene;

        public Point3D Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Scene ParentScene
        {
            get => parentScene;
            set {
                Scene previusScene = parentScene;
                parentScene = value;
                if (previusScene is not null)
                    previusScene.RemoveSceneObject(this);

                if (value is not null)
                    value.AddSceneObject(this);
            }
        }

        public SceneObject(Scene parentScene, Point3D position, Vector3 rotation)
        {
            this.parentScene = parentScene;
            Position = position;
            Rotation = new Quaternion(rotation);
        }

        public Vector3 GetHeding()
        {
            return Vector3.Transform( Vector3.UnitX, Rotation);
        }

        public Vector3D ConvertFromObjectToWorldSpace(Vector3D vectorInObjectSpace)
        {
            return Vector3D.Transform(vectorInObjectSpace, Rotation);
        }

        public Point3D ConvertFromObjectToWorldSpace(Point3D pointInObjectSpace)
        {
            return new Point3D((Vector3)Position + (Vector3)pointInObjectSpace);
        }
    }
}

namespace rt004.SceneObjects.Loading {

    /// <summary>
    /// Abstract class used for loading of SceneObject. SceneObjectLoader inheratance structure must be parallel to the SceneObject one.
    /// </summary>
    [XmlInclude(typeof(CameraLoader)), XmlInclude(typeof(SolidLoader)), XmlInclude(typeof(LightSourceLoader))]
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
        /// Cteates Instance of corresponding SceneObject type
        /// </summary>
        /// <returns>instanciated object</returns>
        abstract public SceneObject CreateInstance(Scene parentScene);
    }
}
