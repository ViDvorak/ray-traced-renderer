using rt004.Util;
using Util;
//using System.Numerics;

namespace rt004;

internal class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, string> config = ArgumentParser.ParseParameters(args);

        // HDR image.
        FloatImage image = new FloatImage(int.Parse(config["width"]), int.Parse(config["height"]), 3);

        image = GenerateCosImage(image);

        //fi.SaveHDR(fileName);   // Doesn't work well yet...
        image.SavePFM(config["output"]);

        Console.WriteLine("HDR image is finished.");
    }



    /// <summary>
    /// Writes to the image 2D consinus function
    /// </summary>
    /// <param name="image">image to write to</param>
    private static FloatImage GenerateCosImage(FloatImage image)
    {
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                float pixelValue = (float)((Math.Cos(x / 10.0f) * Math.Cos(y / 10.0f) + 1) * image.Width);
                float[] pixel = new float[3] { pixelValue, pixelValue, pixelValue };
                image.PutPixel(x, y, pixel);
            }
        }
        return image;
    }
}
