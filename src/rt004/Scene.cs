using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using Util;

namespace rt004
{
    public class Scene
    {
        private Camera? mainCamera;

        public Color4 AmbientLightColor { get; set; }
        public float AmbientLightIntensity { get; set; }

        private readonly HashSet<Camera> cameras        = new HashSet<Camera>();
        private readonly HashSet<Solid> solids          = new HashSet<Solid>();
        private readonly HashSet<LightSource> lights    = new HashSet<LightSource>();

        public readonly InnerSceneObject rootSceneObject;


        public Scene(out InnerSceneObject rootHierarchyNode)
            : this(out rootHierarchyNode, RendererSettings.defaultAmbientLightColor, RendererSettings.defaultAmbientLightFactor)
        { }


        public Scene(out InnerSceneObject rootHierarchyNode, Color4 ambientLightColor, float ambientLightIntensity)
        {
            rootHierarchyNode = new InnerSceneObject(this, Point3D.Zero, Vector3.Zero);
            rootSceneObject = rootHierarchyNode;

            var allLeafs = new List<SceneObject>();
            rt004.Loading.SceneLoader.ExtractChildren(rootHierarchyNode, in allLeafs);


            foreach (SceneObject leaf in allLeafs)
            {
                AddSceneObject(leaf.ParentObject, leaf);
                if (leaf is Camera camera && mainCamera == null)
                    mainCamera = camera;// here is used field instead of Property because HashSets are NOT INICIALISED yet.
            }

            this.AmbientLightColor = ambientLightColor;
            this.AmbientLightIntensity = ambientLightIntensity;
        }

        /// <summary>
        /// Gets root object of the scene.
        /// </summary>
        public InnerSceneObject RootObject {  get { return rootSceneObject; } }

        /// <summary>
        /// Main Camera to render from by default
        /// </summary>
        public Camera? MainCamera
        {
            get
            {
                return mainCamera;
            }
            set
            {
                if (value is not null)
                {
                    if (!cameras.Contains(value))
                    {
                        throw new KeyNotFoundException("Camera is not inhierarchy. You must add it to hierarchy before assignement as mainCamera");
                    }
                    mainCamera = value;
                }
            }
        }

        /// <summary>
        /// Adds camera to scene
        /// </summary>
        /// <param name="parentSceneObject">Parent object of this camera in scene hierarchy</param>
        /// <param name="camera">camrera to add</param>
        /// <param name="setAsMain">defines if the camera should be set as mainCamera or not(it is set automaticly if there is no MainCamera)</param>
        public void AddCamera(InnerSceneObject parentSceneObject, Camera camera, bool setAsMain = false)
        {
            cameras.Add(camera);

            if (setAsMain || mainCamera == null)
            {
                mainCamera = camera;
            }
        }

        /// <summary>
        /// Gets shallow copy of array with all cameras in the scene.
        /// </summary>
        /// <returns>returns array of cameras</returns>
        public Camera[] GetCameras() {
            return cameras.ToArray();
        }

        /// <summary>
        /// Remove camera from scene
        /// </summary>
        /// <param name="camera">camera to remove</param>
        public void RemoveCamera(Camera camera)
        {
            cameras.Remove(camera);

            if ( Object.ReferenceEquals(mainCamera, camera)) {
                if (cameras.Count > 0)
                    mainCamera = cameras.First<Camera>();
                else
                    MainCamera = null;
            }
        }
        
        /// <summary>
        /// Adds solid to scene
        /// </summary>
        /// <param name="solid">solid to add</param>
        public void AddSolid(InnerSceneObject parentSceneObject, Solid solid)
        {
            solids.Add(solid);
        }

        /// <summary>
        /// Removes an solid from the scene
        /// </summary>
        /// <param name="solid">Solid to remove</param>
        public void RemoveSolid(Solid solid) {
            solids.Remove(solid);
        }

        /// <summary>
        /// Gets all solids in scene
        /// </summary>
        /// <returns>Returns array of all solids in scene</returns>
        public Solid[] GetSolids() => solids.ToArray();

        /// <summary>
        /// Add LightSource from the scene
        /// </summary>
        /// <param name="light">LightSource to add</param>
        public void AddLight(InnerSceneObject parentObject, LightSource light)
        {
            lights.Add(light);
        }

        /// <summary>
        /// Removes LightSource from the scene
        /// </summary>
        /// <param name="light">LightSource to remove</param>
        public void RemoveLight(LightSource light)
        {
            lights.Remove(light);
        }


        /// <summary>
        /// Gets array of all light sources from scene
        /// </summary>
        /// <returns>Returns array of light sources</returns>
        public LightSource[] GetLightSources() => lights.ToArray();


        /// <summary>
        /// Adds an SceneObject to scene.
        /// </summary>
        /// <param name="parentSceneObject">Object to add to sceneObject as child</param>
        /// <param name="sceneObject">Scene object to add</param>
        /// <exception cref="NotImplementedException">thrown when sceneObject is not derived from Camera, Solid or Light</exception>
        public void AddSceneObject(InnerSceneObject parentSceneObject, SceneObject sceneObject)
        {
            if (sceneObject is Camera camera)
            {
                AddCamera(parentSceneObject, camera);
            }

            else if (sceneObject is Solid solid)
            {
                AddSolid(parentSceneObject, solid);
            }

            else if (sceneObject is LightSource light)
            {
                AddLight(parentSceneObject, light);
            }

            else if (sceneObject is InnerSceneObject innerSceneObject)
            {
                foreach (SceneObject child in innerSceneObject.GetChildren())
                {
                    AddSceneObject(innerSceneObject, child);
                }
            }

            else
            {
                throw new NotImplementedException("not known class derived from SceneObject" + sceneObject.GetType());
            }

            BasicAddPrecedure(parentSceneObject, sceneObject);
        }

        /// <summary>
        /// registers an object to their new parent a unregisters it from previus
        /// </summary>
        /// <param name="parent">new parent object to register to</param>
        /// <param name="child">child to register</param>
        private void BasicAddPrecedure(InnerSceneObject parent, SceneObject child)
        {
            if (child.ParentObject is not null)
                child.ParentObject.UnRegisterChild(child);
            child.ParentObject = parent;
            parent.RegisterChild(child);
            //AddSceneObject(parent, child);

            child.ParentScene = parent.ParentScene;
        }


        /// <summary>
        /// Runs basic removal of SceneObject from Scene
        /// </summary>
        /// <param name="sceneObject">The sceneObject to remove</param>
        /// <exception cref="InvalidOperationException"></exception>
        private void BasicRemovePrecedure(SceneObject sceneObject)
        {
            if (sceneObject.ParentScene is null)
                throw new InvalidOperationException("The SceneObject does not have assigned parentScene");

            if (sceneObject.ParentObject is null)
                throw new InvalidOperationException("The parentObject was not assigned to any parentObject");


            sceneObject.ParentObject.UnRegisterChild(sceneObject);
            sceneObject.ParentObject = null;
            // unregister all children deeply

            sceneObject.ParentScene = null;
        }


        /// <summary>
        /// Removes an SceneObject from scene.
        /// </summary>
        /// <param name="sceneObject">Scene object to remove</param>
        /// <exception cref="NotImplementedException">thrown when sceneObject is not derived from Camera, Solid or Light</exception>
        public void RemoveSceneObject(SceneObject sceneObject)
        {
            if (sceneObject is Camera camera)
            {
                RemoveCamera(camera);
            }

            else if (sceneObject is Solid solid)
            {
                RemoveSolid(solid);
            }

            else if (sceneObject is LightSource light)
            {
                RemoveLight(light);
            }

            else if (sceneObject is InnerSceneObject) { }

            else
            {
                throw new NotImplementedException("Not known class, derived from SceneObject");
            }
            BasicRemovePrecedure(sceneObject);
        }



        /// <summary>
        /// Renders image from main camera
        /// </summary>
        /// <returns>Returns rendered image</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public FloatImage RenderScene() {
            if (mainCamera == null)
            {
                if(cameras.Any())
                    mainCamera = cameras.First();
                else
                    throw new ArgumentNullException("No main camera in scene");
            }
            return mainCamera.RenderImage();
        }

        /// <summary>
        /// Renderes images from all cameras in scene
        /// </summary>
        /// <returns>Array of rendered images</returns>
        public FloatImage[] RenderSceneWithAllCameras() {
            List<FloatImage> images = new List<FloatImage>();
            Solid[] bodies = GetSolids();
            foreach (Camera camera in cameras)
            {
                images.Add(camera.RenderImage());
            }
            return images.ToArray();
        }


        #region IntersectionsWithScene

        /// <summary>
        /// Casts ray in a scene and checks for intersections with Solids.
        /// </summary>
        /// <param name="ray">ray to cast</param>
        /// <param name="distance">return value of distance from ray origin</param>
        /// <returns>true when any object is intersected by ray and distance is in beatween min and max distances</returns>
        public bool CastRay(Ray ray, out double distance)
        {
            return CastRay(ray, out distance, out Solid intersectedBody);
        }

        /// <summary>
        /// Casts ray in a scene and checks for intersections with Solids.
        /// </summary>
        /// <param name="ray">ray to cast</param>
        /// <param name="distance">return value of distance from ray origin</param>
        /// <param name="maxDistance">maximal value of distance</param>
        /// <param name="minDistance">minimal distance of value</param>
        /// <returns>true when any object is intersected by ray and distance is in beatween min and max distances otherwise retunrs false</returns>
        public bool CastRay(Ray ray, out double distance, double maxDistance, double minDistance = 0d)
        {
            var isIntersecting = CastRay(ray, out distance);

            isIntersecting = isIntersecting &&
                             (distance >= minDistance || distance.IsFloatEqual(minDistance)) &&
                             (distance <= maxDistance || distance.IsFloatEqual(maxDistance));
            return isIntersecting;
        }


        /// <summary>
        /// Casts ray in a scene and checks for intersections with Solids.
        /// </summary>
        /// <param name="ray">ray to cast</param>
        /// <param name="distance">return value of distance from ray origin</param>
        /// <param name="intersectedBody">returns the closest intersected solid</param>
        /// <returns>true when any object is intersected by ray</returns>
        public bool CastRay(Ray ray, out double distance, out Solid intersectedBody)
        {
            double closestIntersection = double.MaxValue;
            bool hasIntersected = false;
            intersectedBody = null;

            foreach (Solid solid in solids)
            {
                if (solid.TryGetRayIntersection(ray, out distance))
                {
                    hasIntersected = true;
                    if (closestIntersection > distance)
                    {
                        closestIntersection = distance;
                        intersectedBody = solid;
                    }
                }
            }
            distance = closestIntersection;

            return hasIntersected;
        }




        /// <summary>
        /// Casts ray in the scene and checks for closest intersections with a SceneObject without distance limit
        /// </summary>
        /// <param name="ray">Ray to cast</param>
        /// <param name="properties">Returned properties of the closest intersection</param>
        /// <returns>Returns true if an intersection is found else false</returns>
        public bool CastRay(Ray ray, out IntersectionProperties properties)
        {
            return CastRay(ray, out properties, double.MaxValue);
        }

        /// <summary>
        /// Casts ray in the scene and checks for closest intersections with a SceneObject without distance limit
        /// </summary>
        /// <param name="ray">Ray to cast</param>
        /// <param name="properties">Returned properties of the closest intersection</param>
        /// <param name="maxDistance">max distance of the intersection from camera postion</param>
        /// <param name="minDistance">min distance of the intersection from camera position</param>
        /// <returns>Returns true if an intersection is found else false</returns>
        public bool CastRay(Ray ray, out IntersectionProperties properties, double maxDistance, double minDistance = 0)
        {
            double closestIntersection = double.MaxValue;
            bool hasIntersected = false;

            properties = new IntersectionProperties();

            //foreach (Solid solid in solids.Where(solid => (solid.GlobalPosition - ray.Origin).Length < maxDistance && (solid.GlobalPosition - ray.Origin).Length > minDistance))
            foreach (Solid solid in solids)
            {
                if (solid.TryGetRayIntersection(ray, out IntersectionProperties currentPropertes))
                {
                    if (closestIntersection > currentPropertes.distance)
                    {
                        hasIntersected = true;
                        closestIntersection = currentPropertes.distance;
                        properties = currentPropertes; 
                    }
                }
            }

            return hasIntersected;
        }


        /// <summary>
        /// Casts ray in scene and intersect with scene objects.
        /// </summary>
        /// <param name="ray">Ray to cast</param>
        /// <param name="closestIntersection">ClosestIntersection point with the closest intersected object in scene. Sensible value only when function returns true</param>
        /// <returns>Returns true if intersection has been found, otherwise false.</returns>
        public bool CastRay( Ray ray, out Point3D closestIntersection)
        {
            bool hasIntersected = CastRay(ray, out double parameter);
            closestIntersection = ray.GetPointOnRay(parameter);
            return hasIntersected;
        }

        /// <summary>
        /// Casts ray in scene and intersect with scene objects.
        /// </summary>
        /// <param name="ray">Ray to cast</param>
        /// <param name="closestIntersection">ClosestIntersection point with the closest intersected object in scene. Sensible value only when function returns true</param>
        /// <param name="intersectedBody">The closest intersected body, Sansible value only when function retruns true</param>
        /// <returns>Returns true if intersection has been found, otherwise false.</returns>
        public bool CastRay(Ray ray, out Point3D closestIntersection, out Solid intersectedBody)
        {
            bool hasIntersected = CastRay(ray, out double parameter, out intersectedBody);
            closestIntersection = ray.GetPointOnRay(parameter);
            return hasIntersected;
        }

        #endregion IntersectionsWithScene
    }
}


namespace rt004.Loading
{
    using rt004.SceneObjects.Loading;

    public class SceneLoader
    {
        public Color4 ambientLightColor;
        public float ambientLightIntensity;

        public List<SceneObjectLoader> sceneObjectLoaders = new List<SceneObjectLoader>();


        /// <summary>
        /// Creates Scene from loaded data
        /// </summary>
        /// <returns></returns>
        public Scene CreateInstance()
        {
            InnerSceneObject rootSceneObject;
            Scene scene = new Scene(out rootSceneObject);

            scene.AmbientLightIntensity = ambientLightIntensity;
            scene.AmbientLightColor = ambientLightColor;


            foreach( SceneObjectLoader loader in sceneObjectLoaders)
            {
                SceneObject rootChild = loader.CreateInstance(scene);
                scene.AddSceneObject(rootSceneObject, rootChild);
            }

            return scene;
        }

        /// <summary>
        /// Recursivly loads children and adds them to the parentObject
        /// </summary>
        /// <param name="parentObject">SceneObject to add children to</param>
        /// <param name="allChildren">Children to add</param>
        public static void ExtractChildren(InnerSceneObject parentObject, in List<SceneObject> allChildren)
        {
            foreach (SceneObject child in parentObject.GetChildren())
                if (child is InnerSceneObject innerNodeChild)
                {
                    allChildren.Add(innerNodeChild);
                    ExtractChildren(innerNodeChild, in allChildren);
                }
                else
                    allChildren.Add(child);
        }
    }
}