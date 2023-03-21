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

        
        public readonly HashSet<Camera> cameras;
        public readonly HashSet<Solid> solids;
        public readonly HashSet<LightSource> lights;

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
            cameras.Add(camera);
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
            cameras.Remove(camera);

            if (mainCamera == camera) {
                var enumer = cameras.GetEnumerator();
                enumer.MoveNext();
                mainCamera = enumer.Current;
                enumer.Dispose();
            }
        }

        public void AddBody (Solid body) => solids.Add(body);
        public void RemoveBody (Solid body) => solids.Remove(body);

        public void AddLight(LightSource light ) => lights.Add(light);
        public void RemoveLight(LightSource light) => lights.Remove(light);

        public Solid[] GetBodies() => solids.ToArray();

        public FloatImage RenderScene() {
            if (mainCamera == null)
            {
                if(cameras.Any())
                    mainCamera = cameras.First();
                else
                    throw new ArgumentNullException("No main camera in scene");
            }
            return mainCamera.RenderImage(GetBodies());
        }

        public FloatImage[] RenderSceneWithAllCameras() {
            List<FloatImage> images = new List<FloatImage>();
            Solid[] bodies = GetBodies();
            foreach (Camera camera in cameras)
            {
                images.Add(camera.RenderImage(bodies));
            }
            return images.ToArray();
        }


        #region IntersectionsWithScene
        public bool TryToComputeIntersection(Line line, out float parameter)
        {
            return TryToComputeIntersection(line, out parameter, out Solid body);
        }

        public bool TryToComputeIntersection(Line line, out float parameter, out Solid intersectedBody)
        {
            float lowestParameter = float.MaxValue;
            bool hasIntersected = false;
            intersectedBody = null;

            foreach (Solid body in solids)
            {
                if (hasIntersected = body.TryGetRayIntersection(line, out parameter))
                {
                    if (lowestParameter > parameter)
                    {
                        lowestParameter = parameter;
                        intersectedBody = body;
                    }
                }
            }
            parameter = lowestParameter;

            return hasIntersected;
        }

        public bool TryToComputeIntersection( Line line, out Vector3 closestIntersection)
        {
            bool hasIntersected = TryToComputeIntersection(line, out float parameter);
            closestIntersection = line.GetPointOnLine(parameter);
            return hasIntersected;
        }

        public bool TryToComputeIntersection(Line line, out Vector3 closestIntersection, out Solid intersectedBody)
        {
            bool hasIntersected = TryToComputeIntersection(line, out float parameter, out intersectedBody);
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
            Camera[] cameras = new Camera[cameraLoaders.Count];
            Solid[] solids = new Solid[solidLoaders.Count];
            LightSource[] lights= new LightSource[lightLoaders.Count];
            
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i] = (Camera)cameraLoaders[i].CreateInstance();
            }
            for (int i = 0; i < solids.Length; i++)
            {
                solids[i] = (Solid)solidLoaders[i].CreateInstance();
            }
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i] = (LightSource)lightLoaders[i].CreateInstance();
            }

            Scene scene = new Scene(cameras, solids, lights);
            scene.MainCamera = scene.GetCameras()[0];
            return scene;
        }
    }
}