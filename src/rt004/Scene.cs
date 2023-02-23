using rt004.Bodies;
using System.Collections.Generic;
using Util;

namespace rt004
{
    internal class Scene
    {
        public void AddCamera()
        {
            throw new NotImplementedException();
        }

        public Camera[] GetCameras() { throw new NotImplementedException(); }
        Camera RemoveCamera()
        {
            throw new NotImplementedException();
        }

        public void AddBody (Body body) { throw new NotImplementedException();}
        public void RemoveBody (Body body) { throw new NotImplementedException(); }
        public FloatImage RenderScene() { throw new NotImplementedException(); }
        public FloatImage RenderSceneWithAllCameras() { throw new NotImplementedException();}

    }
}
