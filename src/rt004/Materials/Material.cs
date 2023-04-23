
using OpenTK.Mathematics;
using rt004.Util;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.Xml.Schema;

namespace rt004.Materials
{
    public class Material
    {

        Texture BaseColorTexture
        {
            get;
            set;
        }

        MonochromeTexture DiffuseTexture
        {
            get;
            set;
        }

        MonochromeTexture SpecularTexture
        {
            get;
            set;
        }

        MonochromeTexture ShininessTexture
        {
            get;
            set;
        }

        public Material(Texture? baseColor, MonochromeTexture? diffuseIntensity, MonochromeTexture? specularIntensity, MonochromeTexture? shininess)
        {
            BaseColorTexture = baseColor ?? new UniformTexture( RendererSettings.defaultSolidColor);
            DiffuseTexture = diffuseIntensity ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialDiffuseFactor);
            SpecularTexture = specularIntensity ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialSpecularFactor);
            ShininessTexture = shininess ?? new MonochromeUniformTexture(RendererSettings.defaultMaterialShininessFactor);
        }



        public Color4 GetBaseColor(Point2D point) => GetBaseColor(point.X, point.Y);

        public Color4 GetBaseColor(float u, float v)
        {
            return BaseColorTexture.GetColorAt(u, v);
        }



        public float GetDiffuseFactor(Point2D point) => GetDiffuseFactor(point.X, point.Y);

        public float GetDiffuseFactor(float u, float v)
        {
            return DiffuseTexture.GetFactorAt(u, v);
        }



        public float GetSpecularFactor(Point2D point) => GetSpecularFactor(point.X, point.Y);

        public float GetSpecularFactor(float u, float v)
        {
            return SpecularTexture.GetFactorAt(u, v);
        }



        public float GetShininessFactor(Point2D point) => GetShininessFactor(point.X, point.Y);

        public float GetShininessFactor(float u, float v)
        {
            return ShininessTexture.GetFactorAt(u, v);
        }
    }
}

namespace rt004.Materials.Loading
{
    public class MaterialLoader 
    {   
        public TextureLoader baseColor;
        public MonochromeTextureLoader diffuseValue;
        public MonochromeTextureLoader specularValue;
        public MonochromeTextureLoader shininess;

        public MaterialLoader()
        {
            this.baseColor = new UniformTextureLoader(RendererSettings.defaultSolidColor);
            this.diffuseValue = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialDiffuseFactor);
            this.specularValue = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialSpecularFactor);
            this.shininess = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialShininessFactor);
        }

        public MaterialLoader(Color4 baseColor, float diffuseValue, float specularValue, float shininess)
        {
            this.baseColor = new UniformTextureLoader( baseColor);
            this.diffuseValue = new MonochromeUniformTextureLoader( diffuseValue);
            this.specularValue = new MonochromeUniformTextureLoader(specularValue);
            this.shininess = new MonochromeUniformTextureLoader(shininess);
        }   

        public Material CreateInstance()
        {
            return new Material( baseColor.GetInstance(),
                                (MonochromeTexture)diffuseValue.GetInstance(),
                                (MonochromeTexture)specularValue.GetInstance(),
                                (MonochromeTexture)shininess.GetInstance()
                               );
        }
    }
}
