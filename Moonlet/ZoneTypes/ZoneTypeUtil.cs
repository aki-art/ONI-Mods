using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Moonlet.ZoneTypes
{
	public class ZoneTypeUtil
	{
		public const int LAST_INDEX = 16;

		private static readonly Dictionary<ZoneType, string> ZoneTypeNameLookup = new();
		private static readonly Dictionary<string, object> ReverseZoneTypeNameLookup = new();
		public static List<ZoneTypeData> zones = new();

		public static int GetCount() => ZoneTypeNameLookup.Count;

		public static ZoneType Register(ZoneTypeData data, int indexOffset = 0)
		{
			var zoneType = (ZoneType)(ZoneTypeNameLookup.Count + indexOffset);

			ZoneTypeNameLookup.Add(zoneType, data.id);
			ReverseZoneTypeNameLookup.Add(data.id, zoneType);
			zones.Add(data);

			return zoneType;
		}

		public static bool TryGetName(ZoneType type, out string name)
		{
			return ZoneTypeNameLookup.TryGetValue(type, out name);
		}

		public static bool TryParse(string value, out object result)
		{
			return ReverseZoneTypeNameLookup.TryGetValue(value, out result);
		}
	}
}
