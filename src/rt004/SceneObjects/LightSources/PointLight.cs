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
        /// <param name="point">point to compute intensity at, in global coord</param>
        /// <returns>Returns the intensity</returns>
        public override float DiffuseLightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            Vector3D pointToLight = GlobalPosition - point;
            double intensity = diffuseFactor * this.LightPower / (param1 * pointToLight.LengthSquared + param2 * pointToLight.Length + param3);
            bool isPathClear = true;
            
            if (areShadowsEnabled)
                isPathClear = !ParentScene.CastRay(new Ray(point, pointToLight), out double param, pointToLight.Length, RendererSettings.epsilon);
            
            return isPathClear ? (float)intensity : 0f;
        }

        /// <summary>
        /// Computes specular intensity of light from this light at specified point
        /// </summary>
        /// <param name="point">point to compute intensity at</param>
        /// <returns>Returns the intensity</returns>
        public override float SpecularLightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            Vector3D pointToLight = GlobalPosition - point;
            double intensity = specularFactor * this.LightPower / (param1 * pointToLight.LengthSquared + param2 * pointToLight.Length + param3);
            bool isPathClear = true;

            if (areShadowsEnabled)
                isPathClear = !ParentScene.CastRay(new Ray(point, pointToLight), out double param, pointToLight.Length, RendererSettings.epsilon);
            
            return isPathClear ? (float)intensity : 0f;
        }

        /// <summary>
        /// Computes Diffuse and Specular light insnsity at specific point.
        /// </summary>
        /// <param name="point">Position where to compute intensity. In global coordinates.</param>
        /// <returns>Returns two light intensities (Diffuse, Specular)</returns>
        public override (float, float) LightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            Vector3D pointToLight = GlobalPosition - point;
            float intensity = (float)(LightPower / (param1 * pointToLight.LengthSquared + param2 * pointToLight.Length + param3));
            bool isPathClear = true;

            if (areShadowsEnabled)
                isPathClear = !ParentScene.CastRay(new Ray(point, pointToLight), out double param, pointToLight.Length, RendererSettings.epsilon);

            var result = (diffuseFactor * intensity, specularFactor * intensity);
            if (!isPathClear)
                result = (0, 0);

            return result;
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
