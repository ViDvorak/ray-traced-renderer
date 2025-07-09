using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using Util;

namespace rt004
{
    /// <summary>
    /// Represents a 3D scene containing cameras, solid objects, and light sources.
    /// Provides functionality for managing scene hierarchy, rendering, and ray-object intersection testing.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// The main camera used for rendering when no specific camera is specified.
        /// </summary>
        private Camera? mainCamera;

        /// <summary>
        /// Gets or sets the ambient light color that illuminates all objects in the scene uniformly.
        /// </summary>
        public Color4 AmbientLightColor { get; set; }
        
        /// <summary>
        /// Gets or sets the intensity factor for ambient lighting in the scene.
        /// </summary>
        public float AmbientLightIntensity { get; set; }

        /// <summary>
        /// Collection of all cameras in the scene.
        /// </summary>
        private readonly HashSet<Camera> cameras        = new HashSet<Camera>();
        
        /// <summary>
        /// Collection of all solid objects in the scene that can be intersected by rays.
        /// </summary>
        private readonly HashSet<Solid> solids          = new HashSet<Solid>();
        
        /// <summary>
        /// Collection of all light sources in the scene.
        /// </summary>
        private readonly HashSet<LightSource> lights    = new HashSet<LightSource>();

        /// <summary>
        /// The root object of the scene hierarchy tree.
        /// </summary>
        public readonly InnerSceneObject rootSceneObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class with default ambient light settings.
        /// </summary>
        /// <param name="rootHierarchyNode">When this method returns, contains the root hierarchy node of the scene.</param>
        public Scene(out InnerSceneObject rootHierarchyNode)
            : this(out rootHierarchyNode, RendererSettings.defaultAmbientLightColor, RendererSettings.defaultAmbientLightFactor)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class with specified ambient light settings.
        /// </summary>
        /// <param name="rootHierarchyNode">When this method returns, contains the root hierarchy node of the scene.</param>
        /// <param name="ambientLightColor">The ambient light color for the scene.</param>
        /// <param name="ambientLightIntensity">The ambient light intensity for the scene.</param>
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
        /// Gets the root object of the scene hierarchy.
        /// </summary>
        public InnerSceneObject RootObject {  get { return rootSceneObject; } }

        /// <summary>
        /// Gets or sets the main camera used for rendering by default.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when attempting to set a camera that is not in the scene hierarchy.</exception>
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
                        throw new KeyNotFoundException("Camera is not in hierarchy. You must add it to hierarchy before assignment as mainCamera");
                    }
                    mainCamera = value;
                }
            }
        }

        /// <summary>
        /// Adds a camera to the scene and optionally sets it as the main camera.
        /// </summary>
        /// <param name="parentSceneObject">The parent object of this camera in the scene hierarchy.</param>
        /// <param name="camera">The camera to add to the scene.</param>
        /// <param name="setAsMain">If true, sets this camera as the main camera. If false, sets as main only if no main camera exists.</param>
        public void AddCamera(InnerSceneObject parentSceneObject, Camera camera, bool setAsMain = false)
        {
            cameras.Add(camera);

            if (setAsMain || mainCamera == null)
            {
                mainCamera = camera;
            }
        }

        /// <summary>
        /// Gets a shallow copy of an array containing all cameras in the scene.
        /// </summary>
        /// <returns>An array of all cameras in the scene.</returns>
        public Camera[] GetCameras() {
            return cameras.ToArray();
        }

        /// <summary>
        /// Removes a camera from the scene and updates the main camera if necessary.
        /// </summary>
        /// <param name="camera">The camera to remove from the scene.</param>
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
        /// Adds a solid object to the scene.
        /// </summary>
        /// <param name="parentSceneObject">The parent object of this solid in the scene hierarchy.</param>
        /// <param name="solid">The solid object to add to the scene.</param>
        public void AddSolid(InnerSceneObject parentSceneObject, Solid solid)
        {
            solids.Add(solid);
        }

        /// <summary>
        /// Removes a solid object from the scene.
        /// </summary>
        /// <param name="solid">The solid object to remove from the scene.</param>
        public void RemoveSolid(Solid solid) {
            solids.Remove(solid);
        }

        /// <summary>
        /// Gets a shallow copy of an array containing all solid objects in the scene.
        /// </summary>
        /// <returns>An array of all solid objects in the scene.</returns>
        public Solid[] GetSolids() => solids.ToArray();

        /// <summary>
        /// Adds a light source to the scene.
        /// </summary>
        /// <param name="parentObject">The parent object of this light source in the scene hierarchy.</param>
        /// <param name="light">The light source to add to the scene.</param>
        public void AddLight(InnerSceneObject parentObject, LightSource light)
        {
            lights.Add(light);
        }

        /// <summary>
        /// Removes a light source from the scene.
        /// </summary>
        /// <param name="light">The light source to remove from the scene.</param>
        public void RemoveLight(LightSource light)
        {
            lights.Remove(light);
        }

        /// <summary>
        /// Gets a shallow copy of an array containing all light sources in the scene.
        /// </summary>
        /// <returns>An array of all light sources in the scene.</returns>
        public LightSource[] GetLightSources() => lights.ToArray();

        /// <summary>
        /// Adds a scene object to the scene, automatically determining its type and adding it to the appropriate collection.
        /// </summary>
        /// <param name="parentSceneObject">The parent object to add the scene object to as a child.</param>
        /// <param name="sceneObject">The scene object to add to the scene.</param>
        /// <exception cref="NotImplementedException">Thrown when the scene object is not a recognized type (Camera, Solid, LightSource, or InnerSceneObject).</exception>
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

            BasicAddProcedure(parentSceneObject, sceneObject);
        }

        /// <summary>
        /// Registers an object to its new parent and unregisters it from its previous parent.
        /// </summary>
        /// <param name="parent">The new parent object to register the child to.</param>
        /// <param name="child">The child object to register.</param>
        private void BasicAddProcedure(InnerSceneObject parent, SceneObject child)
        {
            if (child.ParentObject is not null)
                child.ParentObject.UnRegisterChild(child);
            child.ParentObject = parent;
            parent.RegisterChild(child);
            //AddSceneObject(parent, child);

            child.ParentScene = parent.ParentScene;
        }

        /// <summary>
        /// Performs basic removal operations for a scene object from the scene hierarchy.
        /// </summary>
        /// <param name="sceneObject">The scene object to remove.</param>
        /// <exception cref="InvalidOperationException">Thrown when the scene object does not have an assigned parent scene or parent object.</exception>
        private void BasicRemoveProcedure(SceneObject sceneObject)
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
        /// Removes a scene object from the scene, automatically determining its type and removing it from the appropriate collection.
        /// </summary>
        /// <param name="sceneObject">The scene object to remove from the scene.</param>
        /// <exception cref="NotImplementedException">Thrown when the scene object is not a recognized type.</exception>
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
            BasicRemoveProcedure(sceneObject);
        }

        /// <summary>
        /// Renders an image from the main camera's perspective.
        /// </summary>
        /// <returns>A rendered image from the main camera.</returns>
        /// <exception cref="ArgumentNullException">Thrown when no main camera exists and no cameras are available in the scene.</exception>
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
        /// Renders images from all cameras in the scene.
        /// </summary>
        /// <returns>An array of rendered images, one from each camera in the scene.</returns>
        public FloatImage[] RenderSceneWithAllCameras() {
            List<FloatImage> images = new List<FloatImage>();
            Solid[] bodies = GetSolids();
            foreach (Camera camera in cameras)
            {
                images.Add(camera.RenderImage());
            }
            return images.ToArray();
        }

        #region Ray-Scene Intersection Methods
        /// <summary>
        /// Contains methods for testing ray intersections with objects in the scene.
        /// These methods are used for rendering, shadow calculation, and collision detection.
        /// </summary>

        /// <summary>
        /// Casts a ray in the scene and checks for intersections with solid objects.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="distance">When this method returns, contains the distance from the ray origin to the closest intersection point.</param>
        /// <returns>True if any object is intersected by the ray; otherwise, false.</returns>
        public bool CastRay(Ray ray, out double distance)
        {
            return CastRay(ray, out distance, out Solid intersectedBody);
        }

        /// <summary>
        /// Casts a ray in the scene and checks for intersections with solid objects within specified distance limits.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="distance">When this method returns, contains the distance from the ray origin to the closest intersection point.</param>
        /// <param name="maxDistance">The maximum distance to consider for intersections.</param>
        /// <param name="minDistance">The minimum distance to consider for intersections.</param>
        /// <returns>True if any object is intersected by the ray within the specified distance range; otherwise, false.</returns>
        public bool CastRay(Ray ray, out double distance, double maxDistance, double minDistance = 0d)
        {
            var isIntersecting = CastRay(ray, out distance);

            isIntersecting = isIntersecting &&
                             (distance >= minDistance || distance.IsFloatEqual(minDistance)) &&
                             (distance <= maxDistance || distance.IsFloatEqual(maxDistance));
            return isIntersecting;
        }

        /// <summary>
        /// Casts a ray in the scene and checks for intersections with solid objects, returning the intersected object.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="distance">When this method returns, contains the distance from the ray origin to the closest intersection point.</param>
        /// <param name="intersectedBody">When this method returns, contains the closest intersected solid object.</param>
        /// <returns>True if any object is intersected by the ray; otherwise, false.</returns>
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
        /// Casts a ray in the scene and returns detailed intersection properties for the closest intersection.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="properties">When this method returns, contains the detailed properties of the closest intersection.</param>
        /// <returns>True if an intersection is found; otherwise, false.</returns>
        public bool CastRay(Ray ray, out IntersectionProperties properties)
        {
            return CastRay(ray, out properties, double.MaxValue);
        }

        /// <summary>
        /// Casts a ray in the scene and returns detailed intersection properties for the closest intersection within distance limits.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="properties">When this method returns, contains the detailed properties of the closest intersection.</param>
        /// <param name="maxDistance">The maximum distance from the ray origin to consider for intersections.</param>
        /// <param name="minDistance">The minimum distance from the ray origin to consider for intersections.</param>
        /// <returns>True if an intersection is found within the specified distance range; otherwise, false.</returns>
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
        /// Casts a ray in the scene and returns the closest intersection point.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="closestIntersection">When this method returns, contains the closest intersection point. Valid only when the method returns true.</param>
        /// <returns>True if an intersection is found; otherwise, false.</returns>
        public bool CastRay( Ray ray, out Point3D closestIntersection)
        {
            bool hasIntersected = CastRay(ray, out double parameter);
            closestIntersection = ray.GetPointOnRay(parameter);
            return hasIntersected;
        }

        /// <summary>
        /// Casts a ray in the scene and returns the closest intersection point and the intersected object.
        /// </summary>
        /// <param name="ray">The ray to cast in the scene.</param>
        /// <param name="closestIntersection">When this method returns, contains the closest intersection point. Valid only when the method returns true.</param>
        /// <param name="intersectedBody">When this method returns, contains the closest intersected solid object. Valid only when the method returns true.</param>
        /// <returns>True if an intersection is found; otherwise, false.</returns>
        public bool CastRay(Ray ray, out Point3D closestIntersection, out Solid intersectedBody)
        {
            bool hasIntersected = CastRay(ray, out double parameter, out intersectedBody);
            closestIntersection = ray.GetPointOnRay(parameter);
            return hasIntersected;
        }

        #endregion Ray-Scene Intersection Methods
    }
}


namespace rt004.Loading
{
    using rt004.SceneObjects.Loading;

    /// <summary>
    /// Provides functionality for loading and creating scene instances from configuration data.
    /// Handles the creation of scene objects and their hierarchy from loaded data.
    /// </summary>
    public class SceneLoader
    {
        /// <summary>
        /// Gets or sets the ambient light color for the scene.
        /// </summary>
        public Color4 ambientLightColor;
        
        /// <summary>
        /// Gets or sets the ambient light intensity for the scene.
        /// </summary>
        public float ambientLightIntensity;

        /// <summary>
        /// Gets or sets the list of scene object loaders that define the objects to be created in the scene.
        /// </summary>
        public List<SceneObjectLoader> sceneObjectLoaders = new List<SceneObjectLoader>();

        /// <summary>
        /// Creates a new <see cref="Scene"/> instance from the loaded configuration data.
        /// </summary>
        /// <returns>A fully configured scene with all objects and hierarchy established.</returns>
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
        /// Recursively extracts all children from a scene object hierarchy and adds them to a flat list.
        /// </summary>
        /// <param name="parentObject">The parent scene object to extract children from.</param>
        /// <param name="allChildren">The list to add all extracted children to.</param>
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