using Microsoft.VisualBasic;
using rt004.SceneObjects;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using rt004.Materials;

namespace rt004.Util.LightModels
{
    /*
    public class NonReflectionPhongModel : LightModelComputation
    {
        public Color4 ComputeLightColor(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights)
        {
            Scene parentScene = intersection.intersectedSolid.ParentScene;
            PhongMaterial material = (PhongMaterial)intersection.intersectedSolid.material;

            Vector3D intersectionToCamera = -intersection.incommingRay.Direction;
            Vector3D normal = intersection.intersectedSolid.GetNormalAt(intersection.position);

            Vector4 baseColor = (Vector4)material.GetBaseColor(intersection.uvCoordinates);
            Vector4 ambientColor = baseColor * (Vector4)parentScene.AmbientLightColor * parentScene.AmbientLightIntensity;
            Vector4 diffuseColor = Vector4.Zero, specularColor = Vector4.Zero;


            foreach (var light in lights)
            {
                float intensity = 0;
                (var diffusePower, var specularPower) = light.LightIntensityAt(intersection.position);

                Vector3D intersectionToLightSource = (light.Position - intersection.position).Normalized();


                //diffuse light part
                intensity = diffusePower * material.GetDiffuseFactor(intersection.uvCoordinates);

                float cosOfAngleLightSourceToNormal = (float)Vector3D.Dot(normal, intersectionToLightSource);
                diffuseColor += baseColor * intensity * MathF.Max(0, cosOfAngleLightSourceToNormal);


                //specluar light part
                Vector3D halfVector = (intersectionToCamera + intersectionToLightSource);
                halfVector.Normalize();

                Vector3D reflectedLightDirection = normal * (Vector3D.Dot(normal, intersectionToLightSource) * 2) - intersectionToLightSource;

                intensity = specularPower * material.GetSpecularFactor(intersection.uvCoordinates);
                float shininess = (float)material.GetShininessFactor(intersection.uvCoordinates);

                //specularColor += (Vector4)light.Color * intensity * MathF.Pow((float)Math.Max(Vector3D.Dot(intersectionToCamera, reflectedLightDirection), 0), shininess);
                specularColor += (Vector4)light.Color * intensity * MathF.Pow((float)Math.Max(Vector3D.Dot(normal, halfVector), 0), shininess);
            }

            return (Color4)( diffuseColor + specularColor + ambientColor );
        }
    }
    */
}
