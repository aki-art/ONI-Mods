using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace Moonlet.Elements
{
	public class ElementData : ElementLoader.ElementEntry
	{
		[YamlIgnore]
		public SimHashes SimHash { get; set; }

		[YamlIgnore]
		public string[] AddedBy { get; set; }

		// keeping properties lower case for consistency with base game yaml
		public string priority { get; set; }
		public string name { get; set; }
		public string descriptionText { get; set; }
		public string color { get; set; }
		public string uiColor { get; set; }
		public string conduitColor { get; set; }
		public string unstableColorTint { get; set; }
		public string materialReferene { get; set; }
		public string mainTexture { get; set; }
		public string mainTextureFromExisting { get; set; }
		public string mainTextureTint { get; set; }
		public string specularTexture { get; set; }
		public float textureUVScale { get; set; }
		public string normalMapTexture { get; set; }
		public string specularColor { get; set; }
		public string debrisAnim { get; set; }
		public string rotAtmosphereQuality { get; set; }
		public string gasTextureType { get; set; }
		public string unstableAnim { get; set; }
		public float eyeIrritationStrength { get; set; }
		public Modifiers modifiers { get; set; }
		public Effects duplicantEffects { get; set; }
		public AudioConfig audio { get; set; }
		// wildcard
		public Dictionary<string, object> customData { get; set; }

		public ElementData()
		{
			color = "FFFFFF";
			lowTemp = 0;
			highTemp = 9999;
		}

		public int GetPriority(string modId)
		{
			if (Enum.TryParse<Priority>(priority, out var enumValue))
				return (int)enumValue;

			if (int.TryParse(priority, out var value))
				return value;

			Log.Warning($"Error in element {modId}/{elementId}: Priority value must be a valid keyword or integer number. " +
				$"Keywords: {Enum.GetNames(typeof(Priority)).Join()}");

			return (int)Priority.Default;
		}

		public class AudioConfig
		{
			public string copyElement { get; set; }
			public string ambienceType { get; set; }
			public string solidAmbienceType { get; set; }
			public string miningSound { get; set; }
			public string miningBreakSound { get; set; }
			public string oreBumpSound { get; set; }
			public string floorEventAudioCategory { get; set; }
			public string creatureChewSound { get; set; }
		}

		public class Effects
		{
			public string soaked { get; set; }
			public bool soakedExlusive { get; set; }
			public string steppedIn { get; set; }
			public bool steppedInExclusive { get; set; }
			public string breathIn { get; set; }
			public string walkOn { get; set; }
		}

		public class Modifiers
		{
			public float decor { get; set; }
			public float overHeat { get; set; }
		}

		public class DebrisSublimation
		{
			public float rate { get; set; }
			public float minAmount { get; set; }
			public float maxDestinationMass { get; set; }
			public float massPower { get; set; }
			public string sublimationElementId { get; set; }
			public string sublimateFx { get; set; }
		}

		public static class GasTextureType
		{
			public const string DEFAULT = "Default";
			public const string OXYGEN = "Oxygen";
		}

		public enum Priority
		{
			VeryLow = 0,
			Low = 5,
			Default = 10,
			High = 15,
			VeryHigh = 20,
			Required = 1000,
		}
	}
}
