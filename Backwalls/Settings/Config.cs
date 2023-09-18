using FUtility;
using FUtility.SaveData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TUNING;
using UnityEngine;

namespace Backwalls.Settings
{
	public class Config : IUserSetting
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public WallLayer Layer { get; set; } = WallLayer.Automatic;

		public string DefaultPattern { get; set; } = "Tile";

		public string DefaultColor { get; set; } = new Color(0.47058824f, 0.47058824f, 0.47058824f).ToHexString();

		public bool EnableShinyTilesGlobal { get; set; } = true;

		public enum WallLayer
		{
			Automatic,
			BehindPipes,
			HidePipes
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
			if (Assets.Prefabs == null)
			{
				Log.Warning("Trying to check if a prefab exists before prefabs are loaded.");
				return false;
			}

			return Assets.TryGetPrefab(pattern) != null;
		}
	}
}
