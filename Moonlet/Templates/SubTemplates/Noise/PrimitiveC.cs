using HarmonyLib;
using LibNoiseDotNet.Graphics.Tools.Noise;
using Moonlet.LibNoiseExtension;
using Moonlet.LibNoiseExtension.Primitives;
using Moonlet.Utils;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class PrimitiveC : IShadowTypeBase<ExtendedPrimitive>, INoiseBase
	{
		public string Primative { get; set; } // Klei by typo, keeping for backwards compat. maps to Primitive
		public string Primitive { get; set; }
		public string Quality { get; set; }
		public IntNumber Seed { get; set; }
		public FloatNumber Offset { get; set; }
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }
		public TextureEntry Texture { get; set; }

		public PrimitiveC()
		{
			Pos = new Vector2FC(0, 0);
			Primative = NoisePrimitive.ImprovedPerlin.ToString();
			Quality = NoiseQuality.Best.ToString();
		}

		private static Dictionary<string, NoisePrimitive> additionalPrimitives = new Dictionary<string, NoisePrimitive>()
		{
			{ "MoonletTexture2D", PrimitiveMod.texture2D },
		};

		public ExtendedPrimitive Convert(Action<string> log)
		{
			var quality = NoiseQuality.Best;
			if (!Quality.IsNullOrWhiteSpace())
			{
				if (!Enum.TryParse(Quality, out quality))
					log($"{Quality} is not a valid Quality. Options are: {Enum.GetNames(typeof(NoiseQuality)).Join()}");
			}

			var primitiveStr = Primative ?? Primitive;
			var noisePrimative = NoisePrimitive.ImprovedPerlin;

			if (!primitiveStr.IsNullOrWhiteSpace())
			{
				if (!EnumUtils.TryParse(primitiveStr, out noisePrimative, additionalPrimitives))
					log($"{primitiveStr} is not a valid NoisePrimitive. Options are: {Enum.GetNames(typeof(NoisePrimitive)).Join()}");
			}

			return new ExtendedPrimitive()
			{
				primative = noisePrimative,
				quality = quality,
				seed = Seed.CalculateOrDefault(0),
				offset = Offset.CalculateOrDefault(1),
				name = Name,
				pos = Pos.ToVector2f(),
				texture = Texture?.LoadTexture<UnityEngine.Texture2D>("Moonlet", "noise_patterns", false) // TODO
			};
		}
	}
}
