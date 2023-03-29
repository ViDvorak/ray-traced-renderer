using OpenTK.Mathematics;
using rt004.SceneObjects;
using rt004.Util;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using Util;

namespace rt004.SceneObjects
{
    public class Camera : SceneObject
    {
        public Color4 backgroundColor = RendererSettings.defaultBacgroundColor;

        private const float maxFoV = float.Pi - float.Pi / 180; // 179° in radians
        private float fov;


        public float FoV
        {
            get
            {
                return fov;
            }
            set
            {
                if (value >= maxFoV || value < 0) { throw new ArgumentException($"FoV not in range (0, PI) value was {value}"); }
                fov = value;
            }
        }

        public uint width, height;

        private float maxRenderingDistanceSquered = MathF.Sqrt(float.MaxValue);
        public float MaxRenderingDistance
        {
            get
            {
                return MathF.Sqrt(maxRenderingDistanceSquered);
            }
            set
            {
                maxRenderingDistanceSquered = value * value;
            }
        }

        public Camera(Scene parentScene, Vector3 position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height) : base(parentScene, position, rotation)
        {
            this.backgroundColor = backgroundColor;
            FoV = fov;
            this.width = width;
            this.height = height;
        }

        public void SetResolution(uint width, uint height)
        {
            this.width = width;
            this.height = height;
        }


        /// <summary>
        /// Renderes image from scene
        /// </summary>
        /// <returns>Returns FlaotImage containing redered image for scene.</returns>
        public FloatImage RenderImage()
        {
            FloatImage image = new FloatImage((int)width, (int)height, 3);

            float pixelDensity = width / (2 * MathF.Tan(fov / 2));

            for (int z = 0; z < height; ++z)
            {
                for (int y = 0; y < width; ++y)
                {
                    Color4 pixelColor = backgroundColor;
                    float distanceOfIntersectionSquared = maxRenderingDistanceSquered;

                    Vector3 pixelVectorInObjectSpace = new Vector3(1, (y - width / 2) / pixelDensity, -(z - height / 2) / pixelDensity); // position == rayVector in object space
                    Vector3 pixelRayDirection = ConvertFromObjectToWorldSpace(pixelVectorInObjectSpace, false);

                    Line ray = new Line(Position, pixelRayDirection);

                    foreach (var solid in ParentScene.GetSolids())
                    {
                        if (solid.TryGetRayIntersection(ray, out Vector3 intersection))
                        {
                            var cameraToIntersection = intersection - Position;
                            if (cameraToIntersection.LengthSquared <= distanceOfIntersectionSquared)
                            {
                                distanceOfIntersectionSquared = cameraToIntersection.LengthSquared;

                                Vector4 pixelColorVector = (Vector4)Color4.Black;
                                float intensity = 0;
                                foreach (var light in ParentScene.GetLightSources())
                                {
                                    intensity =+ light.LightIntensityAt(intersection);

                                    Vector3 intersectionToLight = (intersection - light.Position);
                                    intersectionToLight.Normalize();
                                    
                                    float angle = Vector3.Dot(-solid.GetNormalAt(intersection), intersectionToLight);
                                    pixelColorVector += intensity * (Vector4)solid.color * MathF.Max(angle, 0);
                                }

                                pixelColor = (Color4)pixelColorVector;
                            }
                        }
                    }
                    image.PutPixel(y, z, new float[] { pixelColor.R, pixelColor.G, pixelColor.B });
                }
            }
            return image;
        }


        /*
        (float, float) GetPointLight(PointLight light, Vector3 pos3D, Vector3 viewDir, Vector3 normal)
        {
            float specular, defuse;
            if (light.diffusePower > 0)
            {
                Vector3 lightDir = light.Position - pos3D; //3D position in space of the surface
                float distance = length(lightDir);
                lightDir = lightDir / distance; // = normalize(lightDir);
                distance = distance * distance; //This line may be optimised using Inverse square root

                //Intensity of the diffuse light. Saturate to keep within the 0-1 range.
                float NdotL = dot(normal, lightDir);
                float intensity = saturate(NdotL);

                // Calculate the diffuse light factoring in light color, power and the attenuation
                OUT.Diffuse = intensity * light.diffuseColor * light.diffusePower / distance;

                //Calculate the half vector between the light vector and the view vector.
                //This is typically slower than calculating the actual reflection vector
                // due to the normalize function's reciprocal square root
                float3 H = normalize(lightDir + viewDir);

                //Intensity of the specular light
                float NdotH = dot(normal, H);
                intensity = pow(saturate(NdotH), specularHardness);

                //Sum up the specular light factoring
                OUT.Specular = intensity * light.specularColor * light.specularPower / distance;
            }
            return OUT;
        }
        */
    }
}

namespace rt004.SceneObjects.Loading
{
    public class CameraLoader : SceneObjectLoader
    {
        public Color4 backgroundColor;
        public float fov;

        public uint width, height;

        public CameraLoader() { }

        public CameraLoader(Vector3 position, Vector3 rotation, Color4 backgroundColor, float fov, uint width, uint height) : base(position, rotation)
        {
            this.backgroundColor = backgroundColor;
            this.fov = fov;
            this.width = width;
            this.height = height;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Camera(parentScene, position, rotation, backgroundColor, fov, width, height);
        }
    }
}
