using Moonlet.Utils;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates.SubTemplates
{
	[Serializable]
	public class TemplateSpawnRuleC
	{
		public string RuleId { get; set; }
		public List<string> Names { get; set; }
		public ProcGen.World.TemplateSpawnRules.ListRule ListRule { get; set; }
		public IntNumber SomeCount { get; set; }
		public IntNumber MoreCount { get; set; }
		public IntNumber Times { get; set; }
		public FloatNumber Priority { get; set; }
		public bool AllowDuplicates { get; set; }
		public bool AllowExtremeTemperatureOverlap { get; set; }
		public bool UseRelaxedFiltering { get; set; }
		public Vector2IC OverrideOffset { get; set; }
		public Vector2IC OverridePlacement { get; set; }
		public List<AllowedCellsFilterC> AllowedCellsFilter { get; set; }

		public ProcGen.World.TemplateSpawnRules Convert()
		{
			var cellsFilter = new List<ProcGen.World.AllowedCellsFilter>();

			if (AllowedCellsFilter != null)
			{
				foreach (var filter in AllowedCellsFilter)
				{
					var converted = (ProcGen.World.AllowedCellsFilter)filter;

					if (filter.zoneTypes != null)
					{
						var zoneTypes = new List<ProcGen.SubWorld.ZoneType>();
						foreach (var zoneType in filter.zoneTypes)
						{
							if (Enum.TryParse<ProcGen.SubWorld.ZoneType>(zoneType, out var zone))
								zoneTypes.Add(zone);
							else
								Log.Debug($"ZoneType {zoneType} not found.");
						}

						converted.zoneTypes = zoneTypes;
					}

					cellsFilter.Add(converted);
				}
			}

			return new ProcGen.World.TemplateSpawnRules()
			{
				ruleId = RuleId,
				names = Names,
				listRule = ListRule,
				someCount = SomeCount,
				moreCount = MoreCount,
				times = Times.CalculateOrDefault(1),
				priority = Priority,
				allowDuplicates = AllowDuplicates,
				allowExtremeTemperatureOverlap = AllowExtremeTemperatureOverlap,
				useRelaxedFiltering = UseRelaxedFiltering,
				overrideOffset = OverrideOffset.ToVector2I(),
				overridePlacement = OverridePlacement?.ToVector2I() ?? Vector2I.minusone,
				allowedCellsFilter = cellsFilter
			};
		}
	}
}
