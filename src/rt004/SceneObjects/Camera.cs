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

        public FloatImage RenderImage(Solid[] RenderedBodies)
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

                    foreach (var body in RenderedBodies)
                    {
                        if (body.TryGetRayIntersection(ray, out Vector3 intersection))
                        {
                            var cameraToIntersection = intersection - Position;
                            if (cameraToIntersection.LengthSquared <= distanceOfIntersectionSquared)
                            {
                                distanceOfIntersectionSquared = cameraToIntersection.LengthSquared;

                                Vector4 pixelColorVector = (Vector4)Color4.Black;
                                float intensity = 0;
                                foreach (var light in ParentScene.lights)
                                {
                                    intensity =+ light.LightIntensityAt(intersection);
                                    intensity /= 1.5f;
                                    float angle = Vector3.Dot(-body.GetNormalAt(intersection), (intersection - light.Position).Normalized());
                                    pixelColorVector += intensity * (Vector4)body.color * MathF.Max(angle, 0);
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
        private Color4 ComputePixelColor(Vector3 position, LightSource light, Vector3 cameraPosition, Vector3 normal)
        {
            Color4 OUT;
            if (light.Intensity > 0)
            {
                
                Vector3 lightDirection = light.Position - cameraPosition; //3D position in space of the surface
                float distance = lightDirection.Length;
                lightDirection.Normalize();
                distance = distance * distance;

                //Intensity of the diffuse light. Saturate to keep within the 0-1 range.
                float NdotL = Vector3.Dot(normal, lightDirection);
                float intensity = saturate(NdotL);

                // Calculate the diffuse light factoring in light color, power and the attenuation
                OUT.Diffuse = intensity * light.diffuseColor * light.diffusePower / distance;

                //Calculate the half vector between the light vector and the view vector.
                //This is typically slower than calculating the actual reflection vector
                // due to the normalize function's reciprocal square root
                float3 H = normalize(lightDirection + viewDir);

                //Intensity of the specular light
                float NdotH = dot(normal, H);
                intensity = pow(saturate(NdotH), specularHardness);

                //Sum up the specular light factoring
                OUT.Specular = intensity * light.specularColor * light.specularPower / distance;
            }
            return OUT;
        }*/
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
