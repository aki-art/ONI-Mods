using System;
using System.IO;
using UnityEngine;
using FUtility;

public class BitmapGenerator
{
    public static void Test()
    {
        FastNoise cellular = new FastNoise("CellularDistance");
        cellular.Set("ReturnType", "Index0Add1");
        cellular.Set("DistanceIndex0", 2);

        FastNoise fractal = new FastNoise("FractalFBm");
        fractal.Set("Source", new FastNoise("Simplex"));
        fractal.Set("Gain", 0.3f);
        fractal.Set("Lacunarity", 0.6f);

        FastNoise addDim = new FastNoise("AddDimension");
        addDim.Set("Source", cellular);
        addDim.Set("NewDimensionPosition", 0.5f);
        // or
        // addDim.Set("NewDimensionPosition", new FastNoise("Perlin"));

        FastNoise maxSmooth = new FastNoise("MaxSmooth");
        maxSmooth.Set("LHS", fractal);
        maxSmooth.Set("RHS", addDim);

        Console.WriteLine("SIMD Level " + maxSmooth.GetSIMDLevel());

        GenerateBitmap(maxSmooth, Path.Combine(Utils.ModPath, "testMetadata"));

        // Simplex fractal ENT
        FastNoise nodeTree = FastNoise.FromEncodedNodeTree("DQAFAAAAAAAAQAgAAAAAAD8AAAAAAA==");

        // Encoded node trees can be invalid and return null
        if (nodeTree != null)
        {
            GenerateBitmap(nodeTree, "testENT");
        }
    }

    static void GenerateBitmap(FastNoise fastNoise, string filename, ushort size = 512)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(filename + ".bmp", FileMode.Create)))
        {
            const uint imageDataOffset = 14u + 12u + (256u * 3u);

            // File header (14)
            writer.Write('B');
            writer.Write('M');
            writer.Write(imageDataOffset + (uint)(size * size)); // file size
            writer.Write(0); // reserved
            writer.Write(imageDataOffset); // image data offset
            // Bmp Info Header (12)
            writer.Write(12u); // size of header
            writer.Write(size); // width
            writer.Write(size); // height
            writer.Write((ushort)1); // color planes
            writer.Write((ushort)8); // bit depth
            // Colour map
            for (int i = 0; i < 256; i++)
            {
                writer.Write((byte)i);
                writer.Write((byte)i);
                writer.Write((byte)i);
            }
            // Image data
            float[] noiseData = new float[size * size];
            FastNoise.OutputMinMax minMax = fastNoise.GenUniformGrid2D(noiseData, 0, 0, size, size, 0.02f, 1337);

            float scale = 255.0f / (minMax.max - minMax.min);

            foreach (float noise in noiseData)
            {
                //Scale noise to 0 - 255
                int noiseI = (int)Math.Round((noise - minMax.min) * scale);

                writer.Write((byte)Mathf.Clamp(noiseI, 0, 255));
            }
        }

        Console.WriteLine("Created " + filename + ".bmp " + size + "x" + size);
    }
}