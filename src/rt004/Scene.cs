using rt004.SceneObjects;
using System.Collections.Generic;
using Util;
using OpenTK.Mathematics;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004
{
    public class Scene
    {
        private Camera mainCamera;

        private readonly HashSet<Camera> cameras;
        private readonly HashSet<Solid> solids;
        private readonly HashSet<LightSource> lights;

        public Scene()
        {
            mainCamera = null;
            this.cameras = new HashSet<Camera>();
            this.solids = new HashSet<Solid>();
            this.lights = new HashSet<LightSource>();
        }

        public Scene(Camera[] cameras, Solid[] solids, LightSource[] lights)
        {
            if (cameras.Length > 0)
                mainCamera = cameras[0];
            this.cameras = new HashSet<Camera>(cameras);
            this.solids  = new HashSet<Solid>(solids);
            this.lights  = new HashSet<LightSource>(lights);
        }

        public Camera MainCamera
        {
            get
            {
                return mainCamera;
            }
            set
            {
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
                throw new NotImplementedException("not known class derived from SceneObject");
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
                throw new NotImplementedException("not known class derived from SceneObject");
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
        public bool CastRay(Line line, out float distance)
        {
            return CastRay(line, out distance, out Solid body);
        }

        public bool CastRay(Line line, out float distance, float maxDistance, float minDistance = 0)
        {
            var isIntersecting = CastRay(line, out distance);
            isIntersecting = isIntersecting && (distance > minDistance || distance.isFloatEquals(minDistance)) && distance < maxDistance;
            return isIntersecting;
        }

        public bool CastRay(Line line, out float distance, out Solid intersectedBody)
        {
            float closestIntersection = float.MaxValue;
            bool hasIntersected = false;
            intersectedBody = null;

            foreach (Solid body in solids)
            {
                if (hasIntersected = body.TryGetRayIntersection(line, out distance))
                {
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

        public bool CastRay( Line line, out Vector3 closestIntersection)
        {
            bool hasIntersected = CastRay(line, out float parameter);
            closestIntersection = line.GetPointOnLine(parameter);
            return hasIntersected;
        }

        public bool CastRay(Line line, out Vector3 closestIntersection, out Solid intersectedBody)
        {
            bool hasIntersected = CastRay(line, out float parameter, out intersectedBody);
            closestIntersection = line.GetPointOnLine(parameter);
            return hasIntersected;
        }

        #endregion IntersectionsWithScene
    }
}


namespace rt004.SceneObjects.Loading
{
    public class SceneLoader
    {
        public List<CameraLoader> cameraLoaders = new List<CameraLoader>();
        public List<SolidLoader> solidLoaders = new List<SolidLoader>();
        public List<LightSourceLoader> lightLoaders = new List<LightSourceLoader>();

        public Scene CreateInstance()
        {
            Scene scene = new Scene();

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