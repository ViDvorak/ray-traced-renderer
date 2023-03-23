using OpenTK.Mathematics;

namespace rt004.SceneObjects
{
    public class PointLight : LightSource
    {
        public PointLight(Scene parentScene, Vector3 position, Vector3 rotation, Color4 color, float intensity) : base(parentScene, position, rotation, color, intensity) { }

        public override float LightIntensityAt(Vector3 point)
        {
            float intensity = this.Intensity / (Position - point).Length;
            bool isIntersecting = !ParentScene.CastRay(new Util.Line(Position, point - Position), out float param, 1);
            return isIntersecting ? intensity : 0f;
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class PointLightLoader : LightSourceLoader
    {
        public PointLightLoader()
        {
            
        }

        public PointLightLoader(Vector3 position, Vector3 rotation, Color4 color, float intensity):base(position, rotation, color, intensity)
        {

        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new PointLight(parentScene, position, rotation, lightColor, intensity);
        }
    }
}
