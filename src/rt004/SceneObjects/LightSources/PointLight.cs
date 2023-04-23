using OpenTK.Mathematics;
using rt004.Util;

namespace rt004.SceneObjects
{
    public class PointLight : LightSource
    {
        private const float param1 = 0.002f;
        private const float param2 = 0.02f;
        private const float param3 = 0.1f;


        public PointLight(Scene parentScene, Point3D position, Vector3 rotation, Color4 color, float intensity, float diffuseFactor, float specularFactor) :
                   base(parentScene, position, rotation, color, intensity, diffuseFactor, specularFactor) { }

        public override float DiffuseLightIntensityAt(Point3D point)
        {
            Vector3D lightToPoint = point - Position;
            double intensity = diffuseFactor * this.LightPower / (param1 * lightToPoint.LengthSquared + param2 * lightToPoint.Length + param3);
            bool isPathClear = !ParentScene.CastRay(new Ray(Position, lightToPoint), out double param, 1);
            return isPathClear ? (float)intensity : 0f;
        }

        public override float SpecularLightIntensityAt(Point3D point)
        {
            Vector3D lightToPoint = point - Position;
            double intensity = specularFactor * this.LightPower / (param1 * lightToPoint.LengthSquared + param2 * lightToPoint.Length + param3);
            bool isPathClear = !ParentScene.CastRay(new Ray(Position, lightToPoint), out double param, 1);
            return isPathClear ? (float)intensity : 0f;
        }

        /// <summary>
        /// Computes Diffuse and Specular light insnsity at specific point.
        /// </summary>
        /// <param name="point">position where to compute intensity</param>
        /// <returns>Returns two light intensities (Diffuse, Specular)</returns>
        public override (float, float) LightIntensityAt(Point3D point)
        {
            Vector3D lightToPoint = point - Position;
            float intensity = (float)(LightPower / (param1 * lightToPoint.LengthSquared + param2 * lightToPoint.Length + param3));
            bool isPathClear = !ParentScene.CastRay(new Ray(Position, lightToPoint), out double param, 1);
            return isPathClear ? ( diffuseFactor * intensity, specularFactor * intensity) : (0f, 0f);
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class PointLightLoader : LightSourceLoader
    {
        public PointLightLoader() { }

        public PointLightLoader(Point3D position, Vector3 rotation, Color4 color,
                                float intensity, float diffuseFactor, float specularFactor )
            :base(position, rotation, color, intensity, diffuseFactor, specularFactor)
        {

        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new PointLight(parentScene, new Point3D (position), rotation, lightColor, intensity, diffuseFactor, specularFactor);
        }
    }
}
