using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Materials;

namespace rt004.Util.LightModels
{
    /// <summary>
    /// Represents a Phong lighting model used to calculate the lighting color of an intersection point on a surface.
    /// </summary>
    internal class PhongModel : LightModelComputation
    {
        /// <summary>
        /// Array of light sources used to illuminate the scene.
        /// </summary>
        LightSource[]? lights;

        /// <summary>
        /// Computes the final color at the intersection point using the Phong lighting model.
        /// </summary>
        /// <param name="intersection">Properties of the intersection point on the surface.</param>
        /// <param name="backgroundColor">Background color used if no object is hit during reflection or refraction.</param>
        /// <param name="lights">Array of lights in the scene that affect the intersection point.</param>
        /// <returns>The computed color at the intersection point, including ambient, diffuse, specular, and reflection/refraction contributions.</returns>
        public override Color4 ComputeLightColor(IntersectionProperties intersection, Color4 backgroundColor, LightSource[] lights)
        {
            this.lights = lights;
            return (Color4)ReflectionLight(intersection, backgroundColor, RendererSettings.defaultMaterialIndexOfRefraction,
                                           1f, RendererSettings.maxReflectionDepth);
        }

        /// <summary>
        /// Recursively computes the color at the intersection point, considering reflection, refraction, and Phong lighting components.
        /// </summary>
        /// <param name="intersection">The intersection properties at the surface point.</param>
        /// <param name="backgroundColor">Background color for areas without intersections.</param>
        /// <param name="currentIOR">Current index of refraction of the material.</param>
        /// <param name="contribution">Current ray's contribution factor to the final color.</param>
        /// <param name="iteration">Current recursion depth for reflection and refraction computations.</param>
        /// <returns>The calculated color at the intersection point, including reflection, refraction, and Phong lighting contributions.</returns>
        private Vector4 ReflectionLight(IntersectionProperties intersection, Color4 backgroundColor,
                                        float currentIOR, float contribution, uint iteration)
        {
            Solid intersectedSolid = intersection.intersectedSolid;
            PhongMaterial material = (PhongMaterial)intersectedSolid.material;
            Scene parentScene = intersection.intersectedSolid.ParentScene;

            Vector4 baseColor = (Vector4)material.GetBaseColor(intersection.uvCoordinates);
            Vector4 ambientColor = baseColor * (Vector4)parentScene.AmbientLightColor * parentScene.AmbientLightIntensity;
            Vector4 diffuseColor = Vector4.Zero, specularColor = Vector4.Zero;

            Vector3D intersectionToCamera = -intersection.incomingRay.Direction;
            Vector3D normal = intersection.normal;

            // Computes color contribution from each light source
            if (lights is not null)
            {
                foreach (LightSource light in lights)
                {
                    float specularIntensity = 0f;
                    float diffuseIntensity = 0f;

                    (var diffusePower, var specularPower) = light.LightIntensityAt(intersection.globalPosition, RendererSettings.shadows);

                    Vector3D intersectionToLightSource = (light.GlobalPosition - intersection.globalPosition).Normalized();

                    // Diffuse light contribution
                    diffuseIntensity = diffusePower * material.GetDiffuseFactor(intersection.uvCoordinates);
                    float cosOfAngleLightSourceToNormal = (float)Vector3D.Dot(normal, intersectionToLightSource);
                    diffuseColor += baseColor * diffuseIntensity * MathF.Max(0, cosOfAngleLightSourceToNormal);

                    // Specular light contribution
                    Vector3D halfVector = (intersectionToCamera + intersectionToLightSource).Normalized();
                    Vector3D reflectedLightDirection = normal * (Vector3D.Dot(normal, intersectionToLightSource) * 2) - intersectionToLightSource;

                    specularIntensity = specularPower * material.GetSpecularFactor(intersection.uvCoordinates);
                    float shininess = (float)material.GetShininessFactor(intersection.uvCoordinates);
                    specularColor += (Vector4)light.Color * specularIntensity * (float)Math.Pow(Math.Max(Vector3D.Dot(intersectionToCamera, reflectedLightDirection), 0), shininess);
                }
            }

            // Reflection component
            var transparency = 0f;
            Vector4 reflectedColor = Vector4.Zero;
            if (RendererSettings.reflections)
            {
                transparency = material.GetTransparencyFactor(intersection.uvCoordinates);
                Vector4 reflectionColor;
                var reflectionIntensity = material.GetSpecularFactor(intersection.uvCoordinates);

                Ray reflectionRay = new Ray(intersection.globalPosition, normal * 2f * Vector3D.Dot(normal, intersectionToCamera) - intersectionToCamera);

                if (iteration > 0 && contribution > RendererSettings.minRayContribution &&
                    parentScene.CastRay(reflectionRay, out IntersectionProperties reflectedProperties, double.MaxValue, RendererSettings.epsilon))
                {
                    var nextContribution = contribution * reflectionIntensity * (1 - transparency);
                    reflectionColor = ReflectionLight(reflectedProperties, backgroundColor, currentIOR, nextContribution, --iteration);
                }
                else
                {
                    reflectionColor = (Vector4)backgroundColor;
                }

                reflectedColor = reflectionIntensity * reflectionColor;
            }

            // Refraction component
            Vector4 refractedColor = Vector4.Zero;
            if (RendererSettings.refractions)
            {
                float ior = currentIOR / material.GetIndexOfRefraction(intersection.uvCoordinates);
                double dot = Vector3D.Dot(intersection.normal, intersectionToCamera);
                Vector3D t = intersection.normal * (ior * dot - Math.Sqrt(1 - ior * ior * (1 - dot * dot))) - intersectionToCamera * ior;

                Ray refractedRay = new Ray(intersection.globalPosition, t);

                if (iteration > 0 && contribution > RendererSettings.minRayContribution &&
                    parentScene.CastRay(refractedRay, out IntersectionProperties refractionProperties, double.MaxValue, RendererSettings.epsilon))
                {
                    transparency *= contribution;
                    refractedColor = ReflectionLight(refractionProperties, backgroundColor, ior, contribution, iteration - 1);
                }
                else
                {
                    refractedColor = (Vector4)backgroundColor;
                }
            }

            // Combine components for the final color
            return (1 - transparency) * (ambientColor + diffuseColor + specularColor + reflectedColor) + transparency * refractedColor;
        }
    }
}
