﻿using OpenTK.Mathematics;
using rt004.Util;
using System.Xml.Serialization;

namespace rt004.Materials
{
    public abstract class Texture
    {
        public readonly uint width, height;
        public Texture(uint width, uint height)
        {
            this.width = width;
            this.height = height;
        }

        public abstract Color4 GetColorAt(float u, float v);
    }
}

namespace rt004.Materials.Loading
{
    [XmlInclude(typeof(UniformTextureLoader)), XmlInclude(typeof(MonochromeTextureLoader))]
    public abstract class TextureLoader
    {
        public uint width, height;

        public abstract Texture GetInstance();
    }
}