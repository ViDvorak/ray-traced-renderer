using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004.SceneObjects
{
    public class PointLight : LightSource
    {
        public PointLight() : base(Vector3.Zero, Vector3.Zero, Color4.Gray, 1)
        {

        }

        public PointLight(Vector3 position, Vector3 rotation, Color4 color, float intensity) : base(position, rotation, color, intensity) { }

        public override bool isPointIluminated(Vector3 point, out float intensity)
        {
            intensity = this.Intensity;
            return !ParentScene.TryToComputeIntersection(new Util.Line(Position, point - Position), out float param);
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class PointLightLoader : LightSourceLoader
    {
        public PointLightLoader() { }

        public PointLightLoader(Vector3 position, Vector3 rotation, Color4 color, float intensity):base(position, rotation, color, intensity)
        {

        }

        public override SceneObject CreateInstance()
        {
            return new PointLight(position, rotation, lightColor, intensity);
        }
    }
}
