using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Console.Commands
{
	public class LoadPngIntoMapCommand() : BaseConsoleCommand("loadpngmap")
	{
		public override void SetupArguments()
		{
			expectedArgumentVariations = [
				[
					new ArgumentInfo("pngpath", "Png to load", false),
					new ArgumentInfo("size", "Resize image", true)
				]
			];
		}

		private static readonly Dictionary<Color, BiomeEntry> biomes = new()
		{
			//desert
			{ Util.ColorFromHex("f8cc08"), new BiomeEntry(ZoneType.Sandstone, SimHashes.SandStone)},
			// savanna
			{ Util.ColorFromHex("f89808"), new BiomeEntry(ZoneType.MagmaCore, SimHashes.Dirt)},
			// jungle
			{ Util.ColorFromHex("08f81a"), new BiomeEntry(ZoneType.Swamp, SimHashes.WoodLog)},
			// forest
			{ Util.ColorFromHex("08c24f"), new BiomeEntry(ZoneType.Forest, SimHashes.Dirt)},
			// evergreens
			{ Util.ColorFromHex("0c7859"), new BiomeEntry(ZoneType.Forest, SimHashes.SedimentaryRock)},
			// steppe
			{ Util.ColorFromHex("e7f878"), new BiomeEntry(ZoneType.Moo, SimHashes.Clay)},
			// tundra
			{ Util.ColorFromHex("98f8f8"), new BiomeEntry(ZoneType.CarrotQuarry, SimHashes.Granite)},
			// arctic
			{ Util.ColorFromHex("ffffff"), new BiomeEntry(ZoneType.IceCaves, SimHashes.Ice)},
			// ocean
			{ Util.ColorFromHex("80c4fe"), new BiomeEntry(ZoneType.Metallic, SimHashes.Water)}
		};

		private class BiomeEntry
		{
			public ZoneType zoneType;
			public SimHashes element;

			public BiomeEntry(ZoneType zoneType, SimHashes element)
			{
				this.zoneType = zoneType;
				this.element = element;
			}
		}

		public override CommandResult Run()
		{
			if (GetStringArgument(0, out var pngPath))
			{
				var texture = FUtility.Assets.LoadTexture(pngPath, true);
				if (texture == null)
					texture = FUtility.Assets.LoadTexture("C:/Users/Aki/Documents/Klei/OxygenNotIncluded/mods/dev/Moonlet_dev/test_worldgen/earth_sample.png", true);

				if (texture == null)
					return CommandResult.Error("Texture not found at path. " + pngPath);

				var pixels = texture.GetPixels();
				for (int i = 0; i < pixels.Length; i++)
				{
					if (!Grid.IsValidCell(i))
						continue;

					var color = pixels[i];
					/*if (biomes.TryGetValue(color, out var biome))
					{
						var element = ElementLoader.FindElementByHash(biome.element);
						SimMessages.ReplaceElement(i, biome.element, CellEventLogger.Instance.DebugTool, element.defaultValues.mass);
					}*/

					if (color.a > 0)
					{
						SimMessages.ReplaceElement(i, SimHashes.Algae, CellEventLogger.Instance.DebugTool, 800);
					}
					else
					{
						SimMessages.ReplaceElement(i, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0);
					}
				}
			}

			return CommandResult.Success();
		}
	}
}
