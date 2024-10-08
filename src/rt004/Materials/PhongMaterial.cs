﻿using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Materials.Loading;
using rt004.Util;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;

namespace rt004.Materials
{
    internal class PhongMaterial : Material
    {
        Texture BaseColor;
        MonochromeTexture SpecularTexture;
        MonochromeTexture ShininessTexture;
        MonochromeTexture DiffuseIntensityTexture;
        MonochromeTexture TransparencyTexture;
        MonochromeTexture IndexOfRefraction;
        //MonochromeTexture ambientLightIntensityTexture;

        public PhongMaterial() : this(null, null, null, null, null, null) { }

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
            IndexOfRefraction = iOR ?? new MonochromeUniformTexture( RendererSettings.defaultMaterialIndexOfRefraction);
        }

        public Color4 GetBaseColor(Point2D point) => GetBaseColor(point.X, point.Y);

        public Color4 GetBaseColor(float u, float v)
        {
            return BaseColor.GetColorAt(u, v);
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


        public float GetDiffuseFactor(Point2D point) => GetDiffuseFactor(point.X, point.Y);
        public float GetDiffuseFactor(float u, float v)
        {
            return DiffuseIntensityTexture.GetFactorAt(u,v);
        }

        public float GetTransparencyFactor(Point2D point) => GetTransparencyFactor(point.X, point.Y);
        public float GetTransparencyFactor(float u, float v)
        {
            return TransparencyTexture.GetFactorAt(u, v);
        }

        
        public float GetIndexOfRefraction(Point2D point) => GetIndexOfRefraction(point.X, point.Y);
        public float GetIndexOfRefraction(float u, float v)
        {
            return IndexOfRefraction.GetFactorAt(u, v);
        }

        public override bool IsCorrectMaterialFor(LightModel lightModel)
        {
            return lightModel == LightModel.PhongModel;
        }
    }
}

namespace rt004.MaterialLoading
{
    public class PhongMaterialLoader : MaterialLoader
    {
        public TextureLoader           baseColor                    = new UniformTextureLoader(RendererSettings.defaultSolidColor);
        public MonochromeTextureLoader specularTexture              = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialSpecularFactor);
        public MonochromeTextureLoader shininessTexture             = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialShininessFactor);
        public MonochromeTextureLoader diffuseTexture               = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialDiffuseFactor);
        public MonochromeTextureLoader transparencyTexture         = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialTransparencyFactor);
        public MonochromeTextureLoader indexOfRefractionTexture    = new MonochromeUniformTextureLoader(RendererSettings.defaultMaterialIndexOfRefraction);

        public PhongMaterialLoader()
        {

        }

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
