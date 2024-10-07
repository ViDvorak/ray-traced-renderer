using OpenTK.Mathematics;
using rt004.MaterialLoading;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004.Materials
{
    abstract public class Material
    {
        /// <summary>
        /// Checks if the Material is usable for the specific LightModel
        /// </summary>
        /// <param name="lightModel">Light Model to check </param>
        /// <returns>true if usable else false</returns>
        abstract public bool IsCorrectMaterialFor(LightModel lightModel);

        /// <summary>
        /// Selects correct material for specific LightModel
        /// </summary>
        /// <param name="lightModel">light model to select material for</param>
        /// <returns>Material correct for the LightModel</returns>
        /// <exception cref="NotImplementedException">thrown is there is not defined LightModel cornversion to apropriate material</exception>
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
