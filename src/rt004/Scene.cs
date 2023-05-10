using rt004.SceneObjects;
using System.Collections.Generic;
using Util;
using OpenTK.Mathematics;
using rt004.Util;
using System.Xml.Serialization;
using System.Collections.Specialized;

namespace rt004
{
    public class Scene
    {
        private Camera? mainCamera;

        public Color4 AmbientLightColor { get; set; }
        public float AmbientLightIntensity { get; set; }

        private readonly HashSet<Camera> cameras;
        private readonly HashSet<Solid> solids;
        private readonly HashSet<LightSource> lights;


        public Scene()
        { 
            cameras = new HashSet<Camera>();
            solids = new HashSet<Solid>();
            lights = new HashSet<LightSource>();
        }

        public Scene(Camera[]? cameras, Solid[]? solids, LightSource[]? lights, Color4 ambientLightColor, float ambientLightIntensity = RendererSettings.defaultAmbientLightFactor)
        {
            if (cameras != null)
            {
                this.cameras = new HashSet<Camera>(cameras);
                if (cameras.Length > 0)
                    mainCamera = cameras[0];
            }
            else
                this.cameras = new HashSet<Camera>();


            this.solids  = solids == null ? new HashSet<Solid>() : new HashSet<Solid>(solids);
            this.lights  = lights == null ? new HashSet<LightSource>() : new HashSet<LightSource>(lights);

            this.AmbientLightColor = ambientLightColor;
            this.AmbientLightIntensity = ambientLightIntensity;
        }

        public Camera? MainCamera
        {
            get
            {
                return mainCamera;
            }
            set
            {
                if (mainCamera != null)
                    mainCamera.ParentScene = this;
                if (value != null)
                {
                    mainCamera = value;
                }
            }
        }

        public void AddCamera(Camera camera, bool setAsMain = false)
        {
            if (cameras.Add(camera))
                camera.ParentScene = this;
            if (setAsMain || mainCamera == null)
            {
                mainCamera = camera;
            }
        }

        public Camera[] GetCameras() {
            return cameras.ToArray();
        }

        public void RemoveCamera(Camera camera)
        {
            if (cameras.Remove(camera))
                camera.ParentScene = null;

            if (mainCamera == camera) {
                var enumer = cameras.GetEnumerator();
                enumer.MoveNext();
                mainCamera = enumer.Current;
                enumer.Dispose();
            }
        }

        public void AddSolid(Solid solid)
        {
            if (solids.Add(solid))
                solid.ParentScene = this;
        }
        public void RemoveSolid(Solid solid) {
            if (solids.Remove(solid))
                solid.ParentScene = null;
        }
        public Solid[] GetSolids() => solids.ToArray();

        public void AddLight(LightSource light)
        {
            if (lights.Add(light))
                light.ParentScene = this;
        }

        public void RemoveLight(LightSource light)
        {
            if (lights.Remove(light))
                light.ParentScene = null;
        }

        public void AddSceneObject(SceneObject sceneObject)
        {
            if (sceneObject is Camera camera)
            {
                AddCamera(camera);
            }

            else if(sceneObject is Solid solid)
            {
                AddSolid(solid);
            }

            else if (sceneObject is LightSource light)
            {
                AddLight(light);
            }

            else
            {
                throw new NotImplementedException("not known class derived from SceneObject" + sceneObject.GetType());
            }
        }

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

            else
            {
                throw new NotImplementedException("Not known class derived from SceneObject");
            }
        }

        public LightSource[] GetLightSources() => lights.ToArray();

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
                             (distance >= minDistance || distance.isFloatEqual(minDistance)) &&
                             (distance <= maxDistance || distance.isFloatEqual(maxDistance));
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

            foreach (Solid body in solids)
            {
                if (body.TryGetRayIntersection(ray, out distance))
                {
                    hasIntersected = true;
                    if (closestIntersection > distance)
                    {
                        closestIntersection = distance;
                        intersectedBody = body;
                    }
                }
            }
            distance = closestIntersection;

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


namespace rt004.SceneObjects.Loading
{
    public class SceneLoader
    {
        public Color4 ambientLightColor;
        public float ambientLightIntensity;

        public List<CameraLoader> cameraLoaders = new List<CameraLoader>();
        public List<SolidLoader> solidLoaders = new List<SolidLoader>();
        public List<LightSourceLoader> lightLoaders = new List<LightSourceLoader>();

        public Scene CreateInstance()
        {
            Scene scene = new Scene();

            scene.AmbientLightIntensity = ambientLightIntensity;
            scene.AmbientLightColor = ambientLightColor;

            Camera[] cameras = new Camera[cameraLoaders.Count];
            Solid[] solids = new Solid[solidLoaders.Count];
            LightSource[] lights= new LightSource[lightLoaders.Count];
            
            for (int i = 0; i < cameras.Length; i++)
            {
                scene.AddCamera((Camera)cameraLoaders[i].CreateInstance(scene));
            }
            for (int i = 0; i < solids.Length; i++)
            {
                scene.AddSolid((Solid)solidLoaders[i].CreateInstance(scene));
            }
            for (int i = 0; i < lights.Length; i++)
            {
                scene.AddLight((LightSource)lightLoaders[i].CreateInstance(scene));
            }

            return scene;
        }
    }
}