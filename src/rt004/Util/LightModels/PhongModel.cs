using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Materials;

namespace rt004.Util.LightModels
{
    internal class PhongModel : LightModelComputation
    {
        int maxItereation = 10;

        public override Color4 ComputeLightColor(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights)
        {
            return (Color4)ReflectionLight(intersection, backgroundColor, lights, RendererSettings.maxReflectionDepth);
        }


        //TODO make shure that without intersection there is camera.beckgroundColor assigned and ambient color is needed to be added in each reflection

        private Vector4 ReflectionLight(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights, uint iteration)
         {
            Solid intersectedSolid = intersection.intersectedSolid;
            PhongMaterial material = (PhongMaterial)intersectedSolid.material;
            Scene parentScene = intersection.intersectedSolid.ParentScene;

            Vector4 baseColor = (Vector4)material.GetBaseColor(intersection.uvCoordinates);
            Vector4 ambientColor = baseColor * (Vector4)parentScene.AmbientLightColor * parentScene.AmbientLightIntensity;
            Vector4 diffuseColor = Vector4.Zero, specularColor = Vector4.Zero;


            Vector3D intersectionToCamera = -intersection.incommingRay.Direction;
            Vector3D normal = intersection.normal;

            foreach (LightSource light in lights)
            {
                float specularIntensity = 0f;
                float diffuseIntensity = 0f;
                
                (var diffusePower, var specularPower) = light.LightIntensityAt(intersection.position, RendererSettings.shadows);

                Vector3D intersectionToLightSource = (light.Position - intersection.position).Normalized();


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


            Ray reflectionRay = new Ray(intersection.position, normal * 2f * Vector3D.Dot(normal, intersectionToCamera) - intersectionToCamera);

            Vector4 reflectedColor = Vector4.Zero;
            if (RendererSettings.reflections)
            {
                Vector4 reflectionColor;
                if (iteration > 0 && parentScene.CastRay(reflectionRay, out IntersectionProperties reflectedProperties, double.MaxValue, RendererSettings.epsilon))
                    reflectionColor = ReflectionLight(reflectedProperties, backgroundColor, lights, --iteration);
                else
                    reflectionColor = (Vector4)backgroundColor;

                var reflectionIntensity = material.GetSpecularFactor(intersection.uvCoordinates);
                reflectedColor = reflectionIntensity * reflectionColor;
            }

            return ambientColor + diffuseColor + specularColor + reflectedColor;
        }
    }
}
