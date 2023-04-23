using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace rt004.Util
{
    internal static class RendererSettings
    {
        public const float epsilon = 0.000_001f;

        public static readonly Color4 defaultBacgroundColor = Color4.Gray;
        public static readonly Color4 defaultSolidColor = Color4.Lime;
               

        public const float defaultMaterialSpecularFactor = 0f;
        public const float defaultMaterialDiffuseFactor = 1f;
        public const float defaultMaterialShininessFactor = 2f;


        public const float defaultAmbientLightFactor = 0.1f;
        public static readonly Color4 defaultAmbientLightColor = Color4.White;
    }
}
