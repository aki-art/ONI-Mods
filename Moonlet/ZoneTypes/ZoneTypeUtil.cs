using System;
using System.Collections.Generic;
using System.Linq;
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
			indexOffset += Enum.GetNames(typeof(ZoneType)).Length;
			var zoneType = (ZoneType)(ZoneTypeNameLookup.Count + indexOffset); // (ZoneType)Hash.SDBMLower(data.Id);

			ZoneTypeNameLookup.Add(zoneType, data.Id);
			ReverseZoneTypeNameLookup.Add(data.Id, zoneType);

			zones.Add(data);

			return zoneType;
		}

		public static bool TryGetName(ZoneType type, out string name) => ZoneTypeNameLookup.TryGetValue(type, out name);

		public static bool TryParse(string value, out object result) => ReverseZoneTypeNameLookup.TryGetValue(value, out result);

		public static List<ZoneType> GetZoneTypes() => ZoneTypeNameLookup.Keys.ToList();
	}
}
