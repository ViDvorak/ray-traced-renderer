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


        /// <summary>
        /// Computes diffuse intensity of light from this light at specified point
        /// </summary>
        /// <param name="point">point to compute intensity at</param>
        /// <returns>Returns the intensity</returns>
        public override float DiffuseLightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            Vector3D lightToPoint = point - Position;
            double intensity = diffuseFactor * this.LightPower / (param1 * lightToPoint.LengthSquared + param2 * lightToPoint.Length + param3);
            bool isPathClear = true;
            if (areShadowsEnabled)
                isPathClear = !ParentScene.CastRay(new Ray(Position, lightToPoint), out double param, lightToPoint.Length, 0.001f);
            return isPathClear ? (float)intensity : 0f;
        }

        /// <summary>
        /// Computes specular intensity of light from this light at specified point
        /// </summary>
        /// <param name="point">point to compute intensity at</param>
        /// <returns>Returns the intensity</returns>
        public override float SpecularLightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            Vector3D pointToLight = Position - point;
            double intensity = specularFactor * this.LightPower / (param1 * pointToLight.LengthSquared + param2 * pointToLight.Length + param3);
            bool isPathClear = true;
            if (areShadowsEnabled)
                isPathClear = !ParentScene.CastRay(new Ray(Position, pointToLight), out double param, pointToLight.Length, 0.001f);
            return isPathClear ? (float)intensity : 0f;
        }

        /// <summary>
        /// Computes Diffuse and Specular light insnsity at specific point.
        /// </summary>
        /// <param name="point">position where to compute intensity</param>
        /// <returns>Returns two light intensities (Diffuse, Specular)</returns>
        public override (float, float) LightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            Vector3D pointToLight = Position - point;
            float intensity = (float)(LightPower / (param1 * pointToLight.LengthSquared + param2 * pointToLight.Length + param3));
            bool isPathClear = true;
            if (areShadowsEnabled)
                isPathClear = !ParentScene.CastRay(new Ray(point, pointToLight), out double param);

            var result = (diffuseFactor * intensity, specularFactor * intensity);
            if (!isPathClear)
                result = (0, 0);

            return result;
            //return isPathClear ? ( diffuseFactor * intensity, specularFactor * intensity) : (0f, 0f);
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
