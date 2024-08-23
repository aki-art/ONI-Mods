using Moonlet.Templates.WorldGenTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProcGen.SubWorld;

namespace Moonlet.Utils
{
	public class ZoneTypeUtil
	{
		public const int LAST_INDEX = 16;
		private static int indexOffset = -1;

		private static readonly Dictionary<ZoneType, string> ZoneTypeNameLookup = new();
		private static readonly Dictionary<string, object> ReverseZoneTypeNameLookup = new();

		public static Dictionary<int, int> runTimeIndexLookup = new(); // TODO

		public static ZoneType Register(ZoneTypeTemplate data, int indexOffset = 0)
		{
			indexOffset = GetZoneTypeCount();
			var zoneType = (ZoneType)(ZoneTypeNameLookup.Count + indexOffset); // (ZoneType)Hash.SDBMLower(data.Id);

			ZoneTypeNameLookup.Add(zoneType, data.Id);
			ReverseZoneTypeNameLookup.Add(data.Id, zoneType);

			return zoneType;
		}

		private static bool IsAZoneType(int value)
		{
			var str = ((ZoneType)value).ToString();
			return !int.TryParse(str, out var _);
		}

		private static int GetZoneTypeCount()
		{
			if (indexOffset > -1)
				return indexOffset;

			var baseGame = Enum.GetNames(typeof(ZoneType)).Length;
			var total = baseGame;

			// if other non-Moonlet mods registered any zone types, as long as they pathched ToString i can layer on top of them, avoiding some collisions
			while (IsAZoneType(total) && total < byte.MaxValue)
			{
				total++;
			}

			if (total > baseGame)
				Log.Info($"Found {total - baseGame} modded zonetypes, skipping these enum values (starting indexing at {total}).");

			indexOffset = total;
			return total;
		}

		public static bool TryGetName(ZoneType type, out string name) => ZoneTypeNameLookup.TryGetValue(type, out name);

		public static bool TryParse(string value, out object result) => ReverseZoneTypeNameLookup.TryGetValue(value, out result);

		public static List<ZoneType> GetZoneTypes() => ZoneTypeNameLookup.Keys.ToList();
	}
}
