using LibNoiseDotNet.Graphics.Tools.Noise;
using Moonlet.LibNoiseExtension.Primitives;
using ProcGen.Noise;

namespace Moonlet.LibNoiseExtension
{
	internal class PrimitiveMod
	{
		// TODO this would clash if another mod wanted to add these, but seeing as no modd did for 6 years, relatively safe for now
		public static NoisePrimitive texture2D = (NoisePrimitive)9;

		public static bool OnCreatePrimitives(Primitive instance, int globalSeed, NoiseQuality quality, int seed, ref IModule3D resultOverride)
		{
			if (instance.primative == texture2D)
			{
				var result = new Texture2D((instance as ExtendedPrimitive).texture)
				{
					Quality = quality,
					Seed = seed
				};

				resultOverride = result;
				return true;
			}

			return false;
		}
	}
}
