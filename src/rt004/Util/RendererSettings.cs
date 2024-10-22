using OpenTK.Mathematics;
using rt004.Materials;
using rt004.Util;
using rt004.Util.LightModels;

namespace rt004.Util
{
    internal static class RendererSettings
    {
        public static float         epsilon = 0.001f;

        public static Color4        defaultBacgroundColor               = Color4.Gray;
        public static Color4        defaultSolidColor                   = Color4.Lime;
        public static uint          defaultCameraWidth                  = 600;
        public static uint          defaultCameraHeight                 = 480;
               
        //Phong material default values
        public static float         defaultMaterialSpecularFactor       = 0f;
        public static float         defaultMaterialTransparencyFactor   = 0f;
        public static float         defaultMaterialDiffuseFactor        = 1f;
        public static float         defaultMaterialShininessFactor      = 2f;
        public static float         defaultMaterialIndexOfRefraction    = 0.9f;

        public static float         defaultMaterialRoughnessFactor      = 1f;


        public static float         defaultAmbientLightFactor           = 0.1f;
        public static Color4        defaultAmbientLightColor            = Color4.White;
        public static float         defaultMaterialReflectionIntensity  = 0.9f;


        //rendering settings
        public static LightModel    lightModel                          = LightModel.PhongModel;
        public static uint          maxReflectionDepth                  = 8;
        public static bool          shadows                             = true;
        public static bool          reflections                         = true;
        public static bool          refractions                         = false;
        public static float         minRayContribution                  = 0.001f;
        public static int           AntialiasingFraquency               = 3;
    }



    public enum LightModel
    {
        PhongModel
    }

    public static class LightModelExtensions
    {
        public static LightModelComputation GetLightModelComputation(this LightModel lightModel)
        {
            LightModelComputation model;
            switch (lightModel)
            {
                case LightModel.PhongModel:
                    model = new PhongModel();
                    break;
                default:
                    throw new NotImplementedException("Light model is not known");
            }

            return model;
        }
    }
}

namespace rt004.UtilLoading
{
    public class RendererSettingsLoader
    {
        public float epsilon = RendererSettings.epsilon;

        public Color4 defaultBacgroundColor = RendererSettings.defaultBacgroundColor;
        public Color4 defaultSolidColor = RendererSettings.defaultSolidColor;
        public static uint defaultCameraWidth = RendererSettings.defaultCameraWidth;
        public static uint defaultCameraHeight = RendererSettings.defaultCameraHeight;


        public float defaultSpecularFactor = RendererSettings.defaultMaterialSpecularFactor;
        public float defaultDiffuseFactor = RendererSettings.defaultMaterialDiffuseFactor;
        public float defaultTransparencyFactor = RendererSettings.defaultMaterialTransparencyFactor;
        public float defaultRoughnessFactor = RendererSettings.defaultMaterialRoughnessFactor;
        public float defaultShininessFactor = RendererSettings.defaultMaterialShininessFactor;
        public float defaultIndexOfRefraction = RendererSettings.defaultMaterialIndexOfRefraction;


        public float defaultAmbientLightFactor = RendererSettings.defaultAmbientLightFactor;
        public Color4 defaultAmbientLightColor = RendererSettings.defaultAmbientLightColor;
        
        
        public string lightModel = "PhongModel";
        public uint maxReflectionDepth = RendererSettings.maxReflectionDepth;
        public float minRayContribution = RendererSettings.minRayContribution;
        public bool shadows = RendererSettings.shadows;
        public bool reflections = RendererSettings.reflections;
        public bool refractions = RendererSettings.refractions;



        public void SaveLoadedSettings()
        {
            RendererSettings.epsilon = epsilon;

            RendererSettings.defaultBacgroundColor = defaultBacgroundColor;
            RendererSettings.defaultSolidColor = defaultSolidColor;
            RendererSettings.defaultCameraWidth = defaultCameraWidth;
            RendererSettings.defaultCameraHeight = defaultCameraHeight;

            RendererSettings.defaultMaterialSpecularFactor = defaultSpecularFactor;
            RendererSettings.defaultMaterialDiffuseFactor = defaultDiffuseFactor;
            RendererSettings.defaultMaterialRoughnessFactor = defaultRoughnessFactor;
            RendererSettings.defaultMaterialShininessFactor = defaultShininessFactor;
            RendererSettings.defaultMaterialIndexOfRefraction = defaultIndexOfRefraction;

            RendererSettings.defaultAmbientLightColor = defaultAmbientLightColor;
            RendererSettings.defaultAmbientLightFactor = defaultAmbientLightFactor;


            RendererSettings.lightModel = (LightModel)Enum.Parse(typeof(LightModel), lightModel);
            RendererSettings.shadows = shadows;
            RendererSettings.reflections = reflections;
            RendererSettings.refractions = refractions;
            RendererSettings.maxReflectionDepth = maxReflectionDepth;
            RendererSettings.minRayContribution = minRayContribution;
        }
    }
}
