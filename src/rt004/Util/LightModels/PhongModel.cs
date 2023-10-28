using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Materials;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace rt004.Util.LightModels
{
    internal class PhongModel : LightModelComputation
    {
        LightSource[] lights;

        public override Color4 ComputeLightColor(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights)
        {
            this.lights = lights;
            return (Color4)ReflectionLight( intersection, backgroundColor, RendererSettings.defaultMaterialIndexOfRefraction,
                                            1f, RendererSettings.maxReflectionDepth);
        }

        private Vector4 ReflectionLight( IntersectionProperties intersection, Color4 backgroundColor,
                                         float currentIOR, float contribution, uint iteration)
         {
            Solid intersectedSolid = intersection.intersectedSolid;
            PhongMaterial material = (PhongMaterial)intersectedSolid.material;
            Scene parentScene = intersection.intersectedSolid.ParentScene;

            Vector4 baseColor = (Vector4)material.GetBaseColor(intersection.uvCoordinates);
            Vector4 ambientColor = baseColor * (Vector4)parentScene.AmbientLightColor * parentScene.AmbientLightIntensity;
            Vector4 diffuseColor = Vector4.Zero, specularColor = Vector4.Zero;


            Vector3D intersectionToCamera = -intersection.incommingRay.Direction;
            Vector3D normal = intersection.normal;

            // compute color contribution by lights and base material.

            foreach (LightSource light in lights)
            {
                float specularIntensity = 0f;
                float diffuseIntensity = 0f;
                
                (var diffusePower, var specularPower) = light.LightIntensityAt(intersection.globalPosition, RendererSettings.shadows);

                Vector3D intersectionToLightSource = (light.GlobalPosition - intersection.globalPosition).Normalized();


                //diffuse light part
                diffuseIntensity = diffusePower * material.GetDiffuseFactor(intersection.uvCoordinates);

                float cosOfAngleLightSourceToNormal = (float)Vector3D.Dot(normal, intersectionToLightSource);
                diffuseColor += baseColor * diffuseIntensity * MathF.Max(0, cosOfAngleLightSourceToNormal);


                //specluar light part
                Vector3D halfVector = (intersectionToCamera + intersectionToLightSource);
                halfVector.Normalize();

                Vector3D reflectedLightDirection = normal * (Vector3D.Dot(normal, intersectionToLightSource) * 2) - intersectionToLightSource;

                specularIntensity = specularPower * material.GetSpecularFactor(intersection.uvCoordinates);
                float shininess = (float)material.GetShininessFactor(intersection.uvCoordinates);

                specularColor += (Vector4)light.Color * specularIntensity * MathF.Pow((float)Math.Max(Vector3D.Dot(intersectionToCamera, reflectedLightDirection), 0), shininess);
            }



            var transparency = 0f;

            // Reflections
            Vector4 reflectedColor = Vector4.Zero;
            if (RendererSettings.reflections)
            {
                transparency = material.GetTransparencyFactor(intersection.uvCoordinates);
                Vector4 reflectionColor;
                
                var reflectionIntensity = material.GetSpecularFactor(intersection.uvCoordinates);
                Ray reflectionRay = new Ray(intersection.globalPosition, normal * 2f * Vector3D.Dot(normal, intersectionToCamera) - intersectionToCamera);
                
                if (iteration > 0 && contribution > RendererSettings.minRayContribution &&
                    parentScene.CastRay(reflectionRay, out IntersectionProperties reflectedProperties,
                                        double.MaxValue, RendererSettings.epsilon))
                {
                    var nextContribution = contribution * reflectionIntensity * (1 - transparency);
                    reflectionColor = ReflectionLight(reflectedProperties, backgroundColor, currentIOR, nextContribution, --iteration);
                }
                else
                    reflectionColor = (Vector4)backgroundColor;

                reflectedColor = reflectionIntensity * reflectionColor;
            }

            

            // Refractions
            Vector4 refractedColor = Vector4.Zero;
            if (RendererSettings.refractions)
            {
                double distance;
                float ior = currentIOR / material.GetIndexOfRefraction(intersection.uvCoordinates);
                double dot = Vector3D.Dot(intersection.normal, intersectionToCamera);
                
                Vector3D t = intersection.normal * (ior * dot - Math.Sqrt(1 - ior * ior * (1 - dot * dot))) - intersectionToCamera * ior;
                Ray RefractedRay = new Ray(intersection.globalPosition, t);

                if (iteration > 0 && contribution > RendererSettings.minRayContribution &&
                    parentScene.CastRay(RefractedRay, out IntersectionProperties refractionProperties,
                                        double.MaxValue, RendererSettings.epsilon))
                {
                    transparency = contribution * transparency;
                    refractedColor = ReflectionLight(refractionProperties, backgroundColor, ior, contribution, iteration - 1);
                }
                else
                    refractedColor = (Vector4)backgroundColor;

                //var refractionIntensity = material.GetTransparencyFactor(intersection.uvCoordinates);
                // refractedColor = refractedColor * refractionIntensity;
            }

            return (1 - transparency) * (ambientColor + diffuseColor + specularColor + reflectedColor) + transparency * refractedColor;
        }
    }
}
