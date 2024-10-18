using Moonlet.Utils;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProcGen.World;

namespace Moonlet.Templates.SubTemplates
{
	public class AllowedCellsFilterC : IShadowTypeBase<AllowedCellsFilter>
	{
		public string Tagcommand { get; set; }

		public string Tag { get; set; }

		public IntNumber MinDistance { get; set; }

		public IntNumber MaxDistance { get; set; }

		public string Command { get; set; }

		public List<string> TemperatureRanges { get; set; }

		public List<string> ZoneTypes { get; set; }

		public List<string> SubworldNames { get; set; }

		public IntNumber SortOrder { get; set; }

		public bool? IgnoreIfMissingTag { get; set; }

		public AllowedCellsFilter Convert(Action<string> log = null)
		{
			TemperatureRanges ??= [];
			ZoneTypes ??= [];

			return new AllowedCellsFilter()
			{
				tagcommand = EnumUtils.ParseOrDefault(Tagcommand, AllowedCellsFilter.TagCommand.Default),
				tag = Tag,
				minDistance = MinDistance.CalculateOrDefault(0),
				maxDistance = MaxDistance.CalculateOrDefault(0),
				command = EnumUtils.ParseOrDefault(Command, AllowedCellsFilter.Command.Replace),
				temperatureRanges = TemperatureRanges
					.Select(range => EnumUtils.ParseOrDefault(range, Temperature.Range.Room, Mod.temperaturesLoader.ranges))
					.ToList(),
				zoneTypes = ZoneTypes
					.Select(zone => EnumUtils.ParseOrDefault(zone, SubWorld.ZoneType.Sandstone, ZoneTypeUtil.quickLookup))
					.ToList(),
				subworldNames = SubworldNames ?? [],
				sortOrder = SortOrder.CalculateOrDefault(),
				ignoreIfMissingTag = IgnoreIfMissingTag.GetValueOrDefault(false)
			};
		}
	}
}
