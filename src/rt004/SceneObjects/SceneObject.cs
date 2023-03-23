using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace rt004.SceneObjects
{
    public abstract class SceneObject
    {
        private Vector3 position;
        private Quaternion rotation;

        private Scene parentScene;

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Quaternion Rotation { get; set; }

        public Scene ParentScene
        {
            get { return parentScene; }
            set {
                if (value == null) { throw new ArgumentNullException(); }
                parentScene = value;
            }
        }

        public SceneObject(Scene parentScene, Vector3 position, Vector3 rotation)
        {
            this.parentScene = parentScene;
            this.position = position;
            Rotation = new Quaternion( rotation);
        }

        public Vector3 GetHeding()
        {
            return Vector3.Transform( Vector3.UnitX, rotation);
        }

        public Vector3 ConvertFromObjectToWorldSpace(Vector3 vectorInObjectSpace, bool isPosition)
        {
            if (isPosition)
                return Vector3.Add(position, vectorInObjectSpace);
            else
                return Vector3.Transform(vectorInObjectSpace, Rotation);
        }

        private Matrix4 CreateTransformMatrix()
        {
            var rotationMatrix = Matrix4.CreateFromQuaternion(rotation);
            var translationMatrix = Matrix4.CreateTranslation(position);
            return rotationMatrix * translationMatrix;
        }
    }

}

namespace rt004.SceneObjects.Loading {
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
