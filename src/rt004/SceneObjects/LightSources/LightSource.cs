using OpenTK.Mathematics;
using System.Xml.Serialization;
using rt004.Util;

namespace rt004.SceneObjects
{
    public abstract class LightSource : SceneObject
    {
        float lightPower;
        Color4 lightColor;

        public float diffuseFactor;
        public float specularFactor;

        public float LightPower {
            get { return lightPower; }
            set { lightPower = Math.Abs(value); }
        }

        public Color4 Color {
            get { return lightColor; }
            set { lightColor = value; }
        }

        public LightSource(Scene parentScene, Point3D position, Vector3 rotation, Color4 color, float intensity, float diffuseFactor, float specularFactor) : base(parentScene, position, rotation)
        {
            Color = color;
            LightPower = intensity;

            this.diffuseFactor = diffuseFactor;
            this.specularFactor = specularFactor;
        }

        /// <summary>
        /// Computes diffuse light intensity at specified position.
        /// </summary>
        /// <param name="point">Position where to compute the intensity, in global coordinates.</param>
        /// <returns>returns diffuse intensity</returns>
        abstract public float DiffuseLightIntensityAt(Point3D point, bool areShadowsEnabled);

        /// <summary>
        /// Computes specular light intensity at specified position.
        /// </summary>
        /// <param name="point">Position where to compute the intensity, in global coordinates.</param>
        /// <returns>returns Specular intensity</returns>
        abstract public float SpecularLightIntensityAt(Point3D point, bool areShadowsEnabled);

        /// <summary>
        /// Computes Diffuse and Specular light insnsity at specific point.
        /// </summary>
        /// <param name="point">position where to compute intensity, in global coordinates.</param>
        /// <returns>Returns two light intensities (Diffuse, Specular)</returns>
        public virtual (float, float) LightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            return (DiffuseLightIntensityAt(point, areShadowsEnabled), SpecularLightIntensityAt(point, areShadowsEnabled));
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    [XmlInclude(typeof(PointLightLoader))]
    public abstract class LightSourceLoader : SceneObjectLoader
    {
        public float intensity = 1;
        public Color4 lightColor = Color4.White;

        public float diffuseFactor = 0.5f;
        public float specularFactor = 0.5f;

        public LightSourceLoader() { }

        public LightSourceLoader(Point3D position, Vector3 rotation, Color4 lightColor,
                                 float intensity, float diffuseFactor, float specularFactor)
            : base(position, rotation)
        {
            this.intensity = intensity;
            this.lightColor = lightColor;
            this.diffuseFactor = diffuseFactor;
            this.specularFactor = specularFactor;
        }
    }
}
