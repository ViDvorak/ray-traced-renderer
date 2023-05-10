using rt004.SceneObjects;
using OpenTK.Mathematics;

namespace rt004.Util.LightModels
{
    public interface ILightModel
    {
        Vector4 ComputeLightColor(Ray viewDirection, Point3D intersection, Point2D uvCoordinates, Solid solid, LightSource light);
    }
}
