using rt004.Bodies;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Util;

namespace rt004
{
    internal class Scene
    {
        private HashSet<Camera> cameras = new HashSet<Camera>();
        private HashSet<Body> bodies = new HashSet<Body>();

        public void AddCamera(Camera camera)
        {
            cameras.Add(camera);
        }

        public Camera[] GetCameras() {
            return cameras.ToArray();
        }

        public void RemoveCamera(Camera camera)
        {
            cameras.Remove(camera);
        }

        public void AddBody (Body body) => bodies.Add(body);

        public void RemoveBody (Body body) => bodies.Remove(body);

        public Body[] GetBodies() => bodies.ToArray();

        public FloatImage RenderScene() {
            Camera camera = GetCameras()[0];

            return camera.RenderImage(GetBodies());
        }

        public FloatImage[] RenderSceneWithAllCameras() {
            List<FloatImage> images = new List<FloatImage>();
            Body[] bodies = GetBodies();
            foreach (Camera camera in cameras)
            {
                images.Add(camera.RenderImage(bodies));
            }
            return images.ToArray();
        }
    }
}
