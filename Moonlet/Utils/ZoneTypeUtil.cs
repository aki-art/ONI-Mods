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

		private static readonly Dictionary<ZoneType, string> ZoneTypeNameLookup = new();
		private static readonly Dictionary<string, object> ReverseZoneTypeNameLookup = new();

		public static Dictionary<int, int> runTimeIndexLookup = new(); // TODO

		public static ZoneType Register(ZoneTypeTemplate data, int indexOffset = 0)
		{
			indexOffset += Enum.GetNames(typeof(ZoneType)).Length;
			var zoneType = (ZoneType)(ZoneTypeNameLookup.Count + indexOffset); // (ZoneType)Hash.SDBMLower(data.Id);

			ZoneTypeNameLookup.Add(zoneType, data.Id);
			ReverseZoneTypeNameLookup.Add(data.Id, zoneType);

			return zoneType;
		}

		public static bool TryGetName(ZoneType type, out string name) => ZoneTypeNameLookup.TryGetValue(type, out name);

		public static bool TryParse(string value, out object result) => ReverseZoneTypeNameLookup.TryGetValue(value, out result);

		public static List<ZoneType> GetZoneTypes() => ZoneTypeNameLookup.Keys.ToList();
	}
}
