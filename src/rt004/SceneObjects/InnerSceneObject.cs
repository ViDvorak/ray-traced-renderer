using OpenTK.Mathematics;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004.SceneObjects
{
    public class InnerSceneObject : SceneObject
    {
        private HashSet<SceneObject> children = new HashSet<SceneObject>();

        public InnerSceneObject(Scene parentScene, Point3D position, Vector3 rotation)
            : this(parentScene, position, rotation, new HashSet<SceneObject>()) { }

        public InnerSceneObject (Scene parentScene, Point3D position, Vector3 rotation, HashSet<SceneObject> children) : base(parentScene, position, rotation)
        {
            this.children.UnionWith(children);
        }

        /// <summary>
        /// Gets a shallow copy of an array with children of this InnerSceneObject.
        /// </summary>
        /// <returns>Returns a copy of an array with all children of this InnerSceneObject</returns>
        public SceneObject[] GetChildren() => children.ToArray();

        public override Scene ParentScene
        {
            get => parentScene;
            set
            {
                parentScene = value;
                foreach(SceneObject child in children)
                {
                    child.ParentScene = value;
                }
            }
        }


        /// <summary>
        /// Adds SceneObject to list of children.
        /// </summary>
        /// <param name="child">SceneObject to register as child</param>
        /// <returns>Retruns True if child is succasfully added in and was not registerd before. Otherwise returns False</returns>
        public bool RegisterChild(SceneObject child)
        {
            return children.Add(child);
        }

        /// <summary>
        /// Removes SceneObject from list of children.
        /// </summary>
        /// <param name="child">SceneObject to unregister as child</param>
        /// <returns>Retruns True if child is succasfully removed and was not unregisterd before. Otherwise returns False</returns>
        public bool UnRegisterChild(SceneObject child)
        {
            return children.Remove(child);
        }

        /// <summary>
        /// Checks if an SceneObject is a child of this SceneObject.
        /// </summary>
        /// <param name="sceneObject">A SceneObject to check if it is a child</param>
        /// <returns>True if the sceneObject is a child</returns>
        public bool IsChild(SceneObject sceneObject)
        {
            return children.Contains(sceneObject);
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class InnerSceneObjectLoader : SceneObjectLoader
    {
        public SceneObjectLoader[] children = new SceneObjectLoader[0];


        public InnerSceneObjectLoader() : base() { }


        public InnerSceneObjectLoader(Point3D position, Vector3 rotation) : base(position, rotation)
        {

        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            HashSet<SceneObject> sceneObjectChildren = new HashSet<SceneObject>();

            foreach (SceneObjectLoader child in children)
            {
                var tmp = child.CreateInstance(parentScene);
                sceneObjectChildren.Add(tmp);
            }
            var sceneObject = new InnerSceneObject(parentScene, new Point3D(position), rotation, sceneObjectChildren);

            foreach(SceneObject child in sceneObjectChildren)
            {
                parentScene.AddSceneObject(sceneObject, child);
            }
            return sceneObject;
        }
    }
}