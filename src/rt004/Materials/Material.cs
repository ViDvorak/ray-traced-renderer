using OpenTK.Mathematics;
using rt004.MaterialLoading;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004.Materials
{
    /// <summary>
    /// Represents an abstract base class for different types of materials used in rendering.
    /// </summary>
    public abstract class Material
    {
        /// <summary>
        /// Determines if the material is compatible with a specified lighting model.
        /// </summary>
        /// <param name="lightModel">The lighting model to check for compatibility.</param>
        /// <returns><c>true</c> if the material is compatible with the specified lighting model; otherwise, <c>false</c>.</returns>
        public abstract bool IsCorrectMaterialFor(LightModel lightModel);

        /// <summary>
        /// Selects the appropriate material for a specified lighting model.
        /// </summary>
        /// <param name="lightModel">The lighting model for which to select the material.</param>
        /// <returns>A <see cref="Material"/> instance that is compatible with the specified lighting model.</returns>
        /// <exception cref="NotImplementedException">Thrown if there is no defined material for the specified lighting model.</exception>
        public static Material GetMaterialFor(LightModel lightModel)
        {
            Material material;
            switch (lightModel)
            {
                case LightModel.PhongModel:
                    material = new PhongMaterial();
                    break;
                default:
                    throw new NotImplementedException("Material conversion not implemented for the specified lighting model.");
            }
            return material;
        }
    }
}

namespace rt004.Materials.Loading
{
    /// <summary>
    /// Abstract base class for loading and creating instances of <see cref="Material"/>.
    /// Supports XML serialization for different types of material loaders.
    /// </summary>
    [XmlInclude(typeof(PhongMaterialLoader))]
    public abstract class MaterialLoader
    {
        /// <summary>
        /// Creates a new instance of a material based on the loader's configuration.
        /// </summary>
        /// <returns>A new instance of a <see cref="Material"/>.</returns>
        public abstract Material CreateInstance();
    }
}
