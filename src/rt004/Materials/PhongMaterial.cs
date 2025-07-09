using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Materials.Loading;
using rt004.Util;

namespace rt004.Materials
{
    /// <summary>
    /// Represents a Phong material, which defines various surface properties for rendering 
    /// such as color, shininess, transparency, and index of refraction.
    /// </summary>
    internal class PhongMaterial : Material
    {
        /// <summary>
        /// The base color texture of the material, defining the color of the object at each point.
        /// </summary>
        Texture BaseColor;

        /// <summary>
        /// The specular reflection texture, defining the secularity (shininess/meatless) of the material.
        /// </summary>
        MonochromeTexture SpecularTexture;

        /// <summary>
        /// The shininess texture, defining how sharp the specular highlights appear on the material surface.
        /// </summary>
        MonochromeTexture ShininessTexture;

        /// <summary>
        /// The diffuse intensity texture, defining the intensity of diffuse reflection at each point on the material.
        /// </summary>
        MonochromeTexture DiffuseIntensityTexture;

        /// <summary>
        /// The transparency texture, defining how transparent the object is at each point.
        /// </summary>
        MonochromeTexture TransparencyTexture;

        /// <summary>
        /// The index of refraction texture, defining how much the light bends when passing through the object.
        /// </summary>
        MonochromeTexture IndexOfRefraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhongMaterial"/> class with default settings.
        /// </summary>
        public PhongMaterial() : this(null, null, null, null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhongMaterial"/> class with specified textures for each property.
        /// </summary>
        /// <param name="baseColor">The base color texture.</param>
        /// <param name="specularIntensity">The specular reflection texture.</param>
        /// <param name="diffuseIntensity">The diffuse intensity texture.</param>
        /// <param name="shininess">The shininess texture.</param>
        /// <param name="transparency">The transparency texture.</param>
        /// <param name="iOR">The index of refraction texture.</param>
        public PhongMaterial(
            Texture? baseColor,
            MonochromeTexture? specularIntensity,
            MonochromeTexture? diffuseIntensity,
            MonochromeTexture? shininess,
            MonochromeTexture? transparency,
            MonochromeTexture? iOR
            )
        {
            BaseColor = baseColor ?? new UniformTexture(RendererSettings.defaultSolidColor);
            SpecularTexture = specularIntensity ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialSpecularFactor);
            DiffuseIntensityTexture = diffuseIntensity ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialDiffuseFactor);
            ShininessTexture = shininess ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialShininessFactor);
            TransparencyTexture = transparency ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialTransparencyFactor);
            IndexOfRefraction = iOR ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialIndexOfRefraction);
        }

        /// <summary>
        /// Gets the base color at the specified point in 2D space.
        /// </summary>
        /// <param name="point">The 2D point on the surface of the object.</param>
        /// <returns>The base color at the given point.</returns>
        public Color4 GetBaseColor(Point2D point) => GetBaseColor((float)point.X, (float)point.Y);

        /// <summary>
        /// Gets the base color at the specified UV coordinates.
        /// </summary>
        /// <param name="u">The U coordinate of the texture.</param>
        /// <param name="v">The V coordinate of the texture.</param>
        /// <returns>The base color at the given UV coordinates.</returns>
        public Color4 GetBaseColor(float u, float v)
        {
            return BaseColor.GetColorAt(u, v);
        }

        /// <summary>
        /// Gets the specular reflection factor at the specified point in 2D space.
        /// </summary>
        /// <param name="point">The 2D point on the surface of the object.</param>
        /// <returns>The specular factor at the given point.</returns>
        public float GetSpecularFactor(Point2D point) => GetSpecularFactor((float)point.X, (float)point.Y);

        /// <summary>
        /// Gets the specular reflection factor at the specified UV coordinates.
        /// </summary>
        /// <param name="u">The U coordinate of the texture.</param>
        /// <param name="v">The V coordinate of the texture.</param>
        /// <returns>The specular factor at the given UV coordinates.</returns>
        public float GetSpecularFactor(float u, float v)
        {
            return SpecularTexture.GetFactorAt(u, v);
        }

        /// <summary>
        /// Gets the shininess factor at the specified point in 2D space.
        /// </summary>
        /// <param name="point">The 2D point on the surface of the object.</param>
        /// <returns>The shininess factor at the given point.</returns>
        public float GetShininessFactor(Point2D point) => GetShininessFactor((float)point.X, (float)point.Y);

        /// <summary>
        /// Gets the shininess factor at the specified UV coordinates.
        /// </summary>
        /// <param name="u">The U coordinate of the texture.</param>
        /// <param name="v">The V coordinate of the texture.</param>
        /// <returns>The shininess factor at the given UV coordinates.</returns>
        public float GetShininessFactor(float u, float v)
        {
            return ShininessTexture.GetFactorAt(u, v);
        }

        /// <summary>
        /// Gets the diffuse reflection factor at the specified point in 2D space.
        /// </summary>
        /// <param name="point">The 2D point on the surface of the object.</param>
        /// <returns>The diffuse reflection factor at the given point.</returns>
        public float GetDiffuseFactor(Point2D point) => GetDiffuseFactor((float)point.X, (float)point.Y);

        /// <summary>
        /// Gets the diffuse reflection factor at the specified UV coordinates.
        /// </summary>
        /// <param name="u">The U coordinate of the texture.</param>
        /// <param name="v">The V coordinate of the texture.</param>
        /// <returns>The diffuse reflection factor at the given UV coordinates.</returns>
        public float GetDiffuseFactor(float u, float v)
        {
            return DiffuseIntensityTexture.GetFactorAt(u, v);
        }

        /// <summary>
        /// Gets the transparency factor at the specified point in 2D space.
        /// </summary>
        /// <param name="point">The 2D point on the surface of the object.</param>
        /// <returns>The transparency factor at the given point.</returns>
        public float GetTransparencyFactor(Point2D point) => GetTransparencyFactor((float)point.X, (float)point.Y);

        /// <summary>
        /// Gets the transparency factor at the specified UV coordinates.
        /// </summary>
        /// <param name="u">The U coordinate of the texture.</param>
        /// <param name="v">The V coordinate of the texture.</param>
        /// <returns>The transparency factor at the given UV coordinates.</returns>
        public float GetTransparencyFactor(float u, float v)
        {
            return TransparencyTexture.GetFactorAt(u, v);
        }

        /// <summary>
        /// Gets the index of refraction at the specified point in 2D space.
        /// </summary>
        /// <param name="point">The 2D point on the surface of the object.</param>
        /// <returns>The index of refraction at the given point.</returns>
        public float GetIndexOfRefraction(Point2D point) => GetIndexOfRefraction((float)point.X, (float)point.Y);

        /// <summary>
        /// Gets the index of refraction at the specified UV coordinates.
        /// </summary>
        /// <param name="u">The U coordinate of the texture.</param>
        /// <param name="v">The V coordinate of the texture.</param>
        /// <returns>The index of refraction at the given UV coordinates.</returns>
        public float GetIndexOfRefraction(float u, float v)
        {
            return IndexOfRefraction.GetFactorAt(u, v);
        }

        /// <summary>
        /// Determines whether this material is suitable for the Phong lighting model.
        /// </summary>
        /// <param name="lightModel">The lighting model to check.</param>
        /// <returns>True if the material is for the Phong model; otherwise, false.</returns>
        public override bool IsCorrectMaterialFor(LightModel lightModel)
        {
            return lightModel == LightModel.PhongModel;
        }
    }
}

namespace rt004.MaterialLoading
{
    /// <summary>
    /// A loader class for creating instances of <see cref="PhongMaterial"/>.
    /// </summary>
    public class PhongMaterialLoader : MaterialLoader
    {
        /// <summary>
        /// The loader for the base color texture.
        /// </summary>
        public TextureLoader baseColor = new UniformTextureLoader(RendererSettings.defaultSolidColor);

        /// <summary>
        /// The loader for the specular texture.
        /// </summary>
        public MonochromeTextureLoader specularTexture = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialSpecularFactor);

        /// <summary>
        /// The loader for the shininess texture.
        /// </summary>
        public MonochromeTextureLoader shininessTexture = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialShininessFactor);

        /// <summary>
        /// The loader for the diffuse texture.
        /// </summary>
        public MonochromeTextureLoader diffuseTexture = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialDiffuseFactor);

        /// <summary>
        /// The loader for the transparency texture.
        /// </summary>
        public MonochromeTextureLoader transparencyTexture = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialTransparencyFactor);

        /// <summary>
        /// The loader for the index of refraction texture.
        /// </summary>
        public MonochromeTextureLoader indexOfRefractionTexture = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialIndexOfRefraction);

        /// <summary>
        /// Initializes a new instance of the <see cref="PhongMaterialLoader"/> class with default textures.
        /// </summary>
        public PhongMaterialLoader() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhongMaterialLoader"/> class with specified parameters.
        /// </summary>
        /// <param name="baseColor">The base color texture.</param>
        /// <param name="specular">The specular intensity.</param>
        /// <param name="diffuse">The diffuse intensity.</param>
        /// <param name="shininess">The shininess factor.</param>
        /// <param name="transparency">The transparency factor.</param>
        /// <param name="indexOfRefraction">The index of refraction.</param>
        public PhongMaterialLoader(
            Color4 baseColor,
            float specular,
            float diffuse,
            float shininess,
            float transparency,
            float indexOfRefraction
            )
        {
            this.baseColor = new UniformTextureLoader(baseColor);
            this.specularTexture = new MonochromeUniformTextureLoader(specular);
            this.shininessTexture = new MonochromeUniformTextureLoader(shininess);
            this.diffuseTexture = new MonochromeUniformTextureLoader(diffuse);
            this.transparencyTexture = new MonochromeUniformTextureLoader(transparency);
            this.indexOfRefractionTexture = new MonochromeUniformTextureLoader(indexOfRefraction);
        }

        /// <summary>
        /// Creates an instance of <see cref="PhongMaterial"/> using the loaded textures and parameters.
        /// </summary>
        /// <returns>A new instance of <see cref="PhongMaterial"/>.</returns>
        public override Material CreateInstance()
        {
            return new PhongMaterial(
                baseColor.GetInstance(),
                (MonochromeTexture)specularTexture.GetInstance(),
                (MonochromeTexture)diffuseTexture.GetInstance(),
                (MonochromeTexture)shininessTexture.GetInstance(),
                (MonochromeTexture)transparencyTexture.GetInstance(),
                (MonochromeTexture)indexOfRefractionTexture.GetInstance()
                );
        }
    }
}
