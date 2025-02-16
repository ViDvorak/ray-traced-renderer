using OpenTK.Mathematics;
using System.Xml.Serialization;
using rt004.Util;

namespace rt004.SceneObjects
{
    /// <summary>
    /// Represents an abstract base class for light sources in a scene, defining common properties and methods.
    /// </summary>
    public abstract class LightSource : SceneObject
    {
        /// <summary>
        /// The power of the light source, affecting the intensity of light emitted.
        /// </summary>
        float lightPower;

        /// <summary>
        /// The color of the light emitted by this source.
        /// </summary>
        Color4 lightColor;

        /// <summary>
        /// The factor controlling the diffuse light component for this light source.
        /// </summary>
        public float diffuseFactor;

        /// <summary>
        /// The factor controlling the specular light component for this light source.
        /// </summary>
        public float specularFactor;

        /// <summary>
        /// Gets or sets the intensity of the light source, ensuring a non-negative value.
        /// </summary>
        public float LightPower
        {
            get { return lightPower; }
            set { lightPower = Math.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the color of the light emitted by this light source.
        /// </summary>
        public Color4 Color
        {
            get { return lightColor; }
            set { lightColor = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightSource"/> class with specified parameters.
        /// </summary>
        /// <param name="parentScene">The parent scene to which this light source belongs.</param>
        /// <param name="position">The position of the light source in the scene.</param>
        /// <param name="rotation">The rotation of the light source in the scene.</param>
        /// <param name="color">The color of the light emitted by this source.</param>
        /// <param name="intensity">The intensity of the light source.</param>
        /// <param name="diffuseFactor">The factor for controlling diffuse light.</param>
        /// <param name="specularFactor">The factor for controlling specular light.</param>
        public LightSource(Scene parentScene, Point3D position, Vector3 rotation, Color4 color, float intensity, float diffuseFactor, float specularFactor)
            : base(parentScene, position, rotation)
        {
            Color = color;
            LightPower = intensity;
            this.diffuseFactor = diffuseFactor;
            this.specularFactor = specularFactor;
        }

        /// <summary>
        /// Computes the diffuse light intensity at a specified position in the scene.
        /// </summary>
        /// <param name="point">The position where the intensity is calculated, in global coordinates.</param>
        /// <param name="areShadowsEnabled">Indicates if shadows are considered in the calculation.</param>
        /// <returns>The computed diffuse light intensity.</returns>
        public abstract float DiffuseLightIntensityAt(Point3D point, bool areShadowsEnabled);

        /// <summary>
        /// Computes the specular light intensity at a specified position in the scene.
        /// </summary>
        /// <param name="point">The position where the intensity is calculated, in global coordinates.</param>
        /// <param name="areShadowsEnabled">Indicates if shadows are considered in the calculation.</param>
        /// <returns>The computed specular light intensity.</returns>
        public abstract float SpecularLightIntensityAt(Point3D point, bool areShadowsEnabled);

        /// <summary>
        /// Computes both diffuse and specular light intensities at a specified position.
        /// </summary>
        /// <param name="point">The position where the intensities are calculated, in global coordinates.</param>
        /// <param name="areShadowsEnabled">Indicates if shadows are considered in the calculation.</param>
        /// <returns>A tuple containing the diffuse and specular light intensities.</returns>
        public virtual (float, float) LightIntensityAt(Point3D point, bool areShadowsEnabled)
        {
            return (DiffuseLightIntensityAt(point, areShadowsEnabled), SpecularLightIntensityAt(point, areShadowsEnabled));
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    /// <summary>
    /// Abstract base class for loading light sources from serialized data, with common properties for light configuration.
    /// </summary>
    [XmlInclude(typeof(PointLightLoader))]
    public abstract class LightSourceLoader : SceneObjectLoader
    {
        /// <summary>
        /// The intensity of the light source, defaulting to 1.
        /// </summary>
        public float intensity = 1;

        /// <summary>
        /// The color of the light emitted by this source, defaulting to white.
        /// </summary>
        public Color4 lightColor = Color4.White;

        /// <summary>
        /// The diffuse factor for the light source, defaulting to 0.5.
        /// </summary>
        public float diffuseFactor = 0.5f;

        /// <summary>
        /// The specular factor for the light source, defaulting to 0.5.
        /// </summary>
        public float specularFactor = 0.5f;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightSourceLoader"/> class with default settings.
        /// </summary>
        public LightSourceLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightSourceLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="position">The position of the light source in the scene.</param>
        /// <param name="rotation">The rotation of the light source in the scene.</param>
        /// <param name="lightColor">The color of the light emitted by this source.</param>
        /// <param name="intensity">The intensity of the light source.</param>
        /// <param name="diffuseFactor">The factor for controlling diffuse light.</param>
        /// <param name="specularFactor">The factor for controlling specular light.</param>
        public LightSourceLoader(Point3D position, Vector3 rotation, Color4 lightColor, float intensity, float diffuseFactor, float specularFactor)
            : base(position, rotation)
        {
            this.intensity = intensity;
            this.lightColor = lightColor;
            this.diffuseFactor = diffuseFactor;
            this.specularFactor = specularFactor;
        }
    }
}
