using FUtility.SaveData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Backwalls.Settings
{
	public class Config : IUserSetting
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public WallLayer Layer { get; set; } = WallLayer.Automatic;

		public string DefaultPattern { get; set; } = "Tile";

		public bool ShowHiddenToggles { get; set; }
		public HashSet<string> HiddenPatterns { get; set; } = [];

		public List<FavoritePreset> FavoritedPatterns { get; set; } = [];

		public string DefaultColor { get; set; } = new Color(0.47058824f, 0.47058824f, 0.47058824f).ToHexString();

		public ShinySetting Shiny { get; set; } = ShinySetting.On;

		[JsonIgnore] public bool EnableShinyTilesGlobal => Shiny != ShinySetting.Off;

		public enum WallLayer
		{
			Automatic,
			BehindPipes,
			HidePipes
		}

		public enum ShinySetting
		{
			On,
			Off,
			Dull
		}

		public WallConfig DecorativeWall = new WallConfig()
		{
			ConstructionMass = new[] { 5f },
			ConstructionMaterials = MATERIALS.RAW_MINERALS
		};

		public WallConfig SealedWall = new WallConfig()
		{
			ConstructionMass = new[] { 100f, 5f },
			ConstructionMaterials = new[]
			{
				MATERIALS.BUILDABLERAW,
				MATERIALS.TRANSPARENT
			}
		};

		public class WallConfig
		{
			public DecorConfig Decor { get; set; } = new DecorConfig(0, 10);

			public float[] ConstructionMass { get; set; }

			public string[] ConstructionMaterials { get; set; }
		}

		public class FavoritePreset
		{
			public string Pattern { get; set; }
			public string Color { get; set; }
			public int Index { get; set; }
		}

		public class DecorConfig
		{
			public DecorConfig(int range, int amount)
			{
				Range = range;
				Amount = amount;
			}

			public int Range { get; set; }

			public int Amount { get; set; }
		}


		public void Validate()
		{
			if (!ValidateColor(DefaultColor))
			{
				DefaultColor = new Color(0.47058824f, 0.47058824f, 0.47058824f).ToHexString();
			}

			if (!ValidatePattern(DefaultPattern))
			{
				DefaultPattern = "Tile";
			}
		}

		public bool ValidateColor(string color)
		{
			if (color.IsNullOrWhiteSpace())
			{
				return false;
			}

			if (!(color.Length == 6 || color.Length == 8))
			{
				return false;
			}

			if (!long.TryParse(DefaultColor, System.Globalization.NumberStyles.HexNumber, null, out _))
			{
				return false;
			}

			return true;
		}

		public bool ValidatePattern(string pattern)
		{
			if (Mod.variants == null)
			{
				Log.Warning("Trying to check if a pattern exists before patterns are loaded.");
				return false;
			}

			return Mod.variants.ContainsKey(pattern);
		}
	}
}
