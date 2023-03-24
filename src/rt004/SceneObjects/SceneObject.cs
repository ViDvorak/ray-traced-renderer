using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace rt004.SceneObjects
{
    public abstract class SceneObject
    {
        private Scene parentScene;

        public Vector3 Position { get; set; }
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

        public SceneObject(Scene parentScene, Vector3 position, Vector3 rotation)
        {
            this.parentScene = parentScene;
            Position = position;
            Rotation = new Quaternion( rotation);
        }

        public Vector3 GetHeding()
        {
            return Vector3.Transform( Vector3.UnitX, Rotation);
        }

        public Vector3 ConvertFromObjectToWorldSpace(Vector3 vectorInObjectSpace, bool isPosition)
        {
            if (isPosition)
                return Vector3.Add(Position, vectorInObjectSpace);
            else
                return Vector3.Transform(vectorInObjectSpace, Rotation);
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

        public SceneObjectLoader(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }



        /// <summary>
        /// Cteates Instance of corresponding SceneObject type
        /// </summary>
        /// <returns>instanciated object</returns>
        abstract public SceneObject CreateInstance(Scene parentScene);
    }
}
