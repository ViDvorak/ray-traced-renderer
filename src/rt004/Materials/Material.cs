using OpenTK.Mathematics;
using rt004.MaterialLoading;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004.Materials
{
    abstract public class Material
    {
        abstract public bool IsCorrectMaterialFor(LightModel lightModel);

        public static Material GetMaterialFor(LightModel lightModel)
        {
            Material material;
            switch (lightModel)
            {
                case LightModel.PhongModel:
                    material = new PhongMaterial();
                    break;
                default:
                    throw new NotImplementedException("Material conversion not Implemented");
            }
            return material;
        }
    }
}

namespace rt004.Materials.Loading
{
    [XmlInclude(typeof(PhongMaterialLoader))]
    abstract public class MaterialLoader 
    {   
        abstract public Material CreateInstance();
    }
}
