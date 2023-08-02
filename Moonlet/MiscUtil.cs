using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace Moonlet
{
	public class MiscUtil
	{
		public static Color ParseColor(string str)
		{
			// trim whitespace
			str = str.Replace(" ", "");

			if (str.Contains(','))
			{
				var components = str.Split(',');
				if (components.Length >= 3 && components.Length <= 4)
				{
					if (float.TryParse(components[0], out var r)
						&& float.TryParse(components[1], out var g)
						&& float.TryParse(components[2], out var b))
					{
						var color = new Color(r, g, b);

						if (components.Length == 4 && float.TryParse(components[3], out var a))
							color.a = a;

						return color;
					}
				}

				Log.Warning($"{str} is not a valid color format. Expected: FFFFFF or 1,1,1,1");

				return Color.magenta;
			}

			return Util.ColorFromHex(str);
		}

		// TODO: Support custom, with a dictionary for insertion
		public static List<Tag> GetStorageFilterFromName(string name, out bool editable)
		{
			editable = true;
			switch (name)
			{
				case nameof(STORAGEFILTERS.NOT_EDIBLE_SOLIDS):
					return STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
				case nameof(STORAGEFILTERS.FOOD):
					return STORAGEFILTERS.FOOD;
				case nameof(STORAGEFILTERS.SWIMMING_CREATURES):
					return STORAGEFILTERS.SWIMMING_CREATURES;
				case nameof(STORAGEFILTERS.PAYLOADS):
					return STORAGEFILTERS.PAYLOADS;
				case nameof(STORAGEFILTERS.LIQUIDS):
					return STORAGEFILTERS.LIQUIDS;
				case nameof(STORAGEFILTERS.GASES):
					return STORAGEFILTERS.GASES;
				case nameof(STORAGEFILTERS.BAGABLE_CREATURES):
					return STORAGEFILTERS.BAGABLE_CREATURES;
				case nameof(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE):
					editable = false;
					return STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE.ToList();
			}

			Log.Warning("Invalid Storage Filter ID " + name);

			return null;
		}

		public static bool ParseElementEntry(string value, out SimHashes elementId)
		{
			elementId = SimHashes.Void;

			if (value.IsNullOrWhiteSpace())
				return false;

			value.Replace(" ", ""); // trim spaces
			var entries = value.Split('>');

			foreach (var entry in entries)
			{
				var element = ElementLoader.FindElementByName(entry);
				if (element != null)
				{
					elementId = element.id;
					return true;
				}
			}

			return false;
		}
	}
}
