using Microsoft.VisualBasic;
using rt004.SceneObjects;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;


namespace rt004.Util.LightModels
{
    public class NonRefractionPhongModel : ILightModel
    {
        public Vector4 ComputeLightColor(Ray viewDirection, Point3D intersection, Point2D uvCoordinates, Solid solid, LightSource light)
        {
            Vector4 baseColor = (Vector4)solid.material.GetBaseColor(uvCoordinates);
            float intensity = 0;
            (var diffusePower, var specularPower) = light.LightIntensityAt(intersection);

            Vector3D intersectionToLightSource = (light.Position - intersection).Normalized();
            Vector3D intersectionToCamera = (light.Position - intersection).Normalized();
            Vector3D normal = solid.GetNormalAt(intersection);


            //diffuse light part
            intensity = diffusePower * solid.material.GetDiffuseFactor(uvCoordinates);

            float cosOfAngleLightSourceToNormal = (float)Vector3D.Dot(normal, intersectionToLightSource);
            Vector4 diffuseColor = baseColor * intensity * MathF.Max(0, cosOfAngleLightSourceToNormal);


            //specluar light part
            Vector3D halfVector = (intersectionToCamera + intersectionToLightSource);
            halfVector.Normalize();

            Vector3D reflectedLightDirection = normal * (Vector3D.Dot(normal, intersectionToLightSource) * 2) - intersectionToLightSource;

            intensity = specularPower * solid.material.GetSpecularFactor(uvCoordinates);
            float shininess = (float)solid.material.GetShininessFactor(uvCoordinates);

            Vector4 specularColor = (Vector4)light.Color * intensity * MathF.Pow((float)Math.Max(Vector3D.Dot(intersectionToCamera, reflectedLightDirection), 0), shininess);

            return diffuseColor + specularColor;
        }
    }
}
