using rt004.SceneObjects;
using OpenTK.Mathematics;

namespace rt004.Util.LightModels
{
    abstract public class LightModelComputation
    {
        /// <summary>
        /// Computes color at the intersection
        /// </summary>
        /// <param name="intersection">Intersection properties of ray - scene interersection</param>
        /// <param name="backgroundColor">background color of the scene</param>
        /// <param name="lights">light sources to compute lighting with</param>
        /// <returns>Returns color at the intersection position</returns>
        abstract public Color4 ComputeLightColor(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights);



        /// <summary>
        /// Contains apropriate LightModelComputation for each LightModel.
        /// 
        /// Note: When creating new LightModelComputation they needs to be registered here.
        /// </summary>
        private static Dictionary<LightModel, LightModelComputation> LightModelConversion = new Dictionary<LightModel, LightModelComputation>()
        {
            { LightModel.PhongModel, new PhongModel() },

        };

        /// <summary>
        /// Gets apropriate light computation model based on enum
        /// </summary>
        /// <param name="lightModel"></param>
        /// <returns>Returns LightModelComputation apropriate for lightModel</returns>
        public static LightModelComputation GetCorrectLightModel(LightModel lightModel)
        {
            return LightModelConversion[lightModel];
        }
    }
}
