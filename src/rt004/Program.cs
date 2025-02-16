using OpenTK.Mathematics;
using rt004.Util;
using System.Text.Json;
using System.Xml;
using Util;

namespace rt004;

internal class Program
{
    static void Main(string[] args)
    {
        Scene scene;
        string output;
        try
        {
            (scene, output) = ArgumentParser.ParseScene(args);
            Console.WriteLine("the output file is " + output);
        }
        catch (Exception e)
        {
            Console.WriteLine("An error ocurred during comand line argument parsing:");
            Console.WriteLine(e.ToString());
            return;
        }

        // render HDR image.
        try { 
            FloatImage image = scene.RenderScene();

            image.SavePFM(output);

            Console.WriteLine("HDR image is finished.");
        }
        catch (Exception e)
        {
            Console.WriteLine("An error ocurred:");
            Console.WriteLine(e.ToString());
        }
    }



    /// <summary>
    /// Writes to the image 2D cosinus function
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
