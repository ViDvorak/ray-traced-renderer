using rt004.SceneObjects;
using OpenTK.Mathematics;

namespace rt004.Util.LightModels
{
    abstract public class LightModelComputation
    {
        abstract public Color4 ComputeLightColor(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights);




        static Dictionary<LightModel, LightModelComputation> LightModelConversion = new Dictionary<LightModel, LightModelComputation>() {
            { LightModel.PhongModel, new PhongModel() }
        };
        public static LightModelComputation GetCorrectLightModel(LightModel lightModel)
        {
            return LightModelConversion[lightModel];
        }
    }
}
