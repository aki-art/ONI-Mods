using Moonlet.Utils;
using System;
using System.Collections.Generic;
using static ProcGen.World;

namespace Moonlet.Templates.SubTemplates
{
	[Serializable]
	public class TemplateSpawnRuleC : ShadowTypeBase<TemplateSpawnRules>
	{
		public string RuleId { get; set; }
		public List<string> Names { get; set; }
		public TemplateSpawnRules.ListRule ListRule { get; set; }
		public IntNumber SomeCount { get; set; }
		public IntNumber MoreCount { get; set; }
		public IntNumber Times { get; set; }
		public FloatNumber Priority { get; set; }
		public bool AllowDuplicates { get; set; }
		public bool AllowExtremeTemperatureOverlap { get; set; }
		public bool UseRelaxedFiltering { get; set; }
		public Vector2IC OverrideOffset { get; set; }
		public Vector2IC OverridePlacement { get; set; }
		public List<AllowedCellsFilter> AllowedCellsFilter { get; set; }

		public TemplateSpawnRuleC()
		{
			AllowedCellsFilter = new();
			OverrideOffset = new Vector2IC(0, 0);
			OverridePlacement = new Vector2IC(-1, -1);
			AllowDuplicates = false;
			UseRelaxedFiltering = false;
		}

		public override TemplateSpawnRules Convert()
		{
			/*			var cellsFilter = new List<AllowedCellsFilter>();

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
			*/
			var result = new TemplateSpawnRules
			{
				ruleId = RuleId,
				names = Names,
				listRule = ListRule,
				someCount = SomeCount.CalculateOrDefault(0),
				moreCount = MoreCount.CalculateOrDefault(0),
				times = Times.CalculateOrDefault(1),
				priority = Priority.CalculateOrDefault(0),
				allowDuplicates = AllowDuplicates,
				allowExtremeTemperatureOverlap = AllowExtremeTemperatureOverlap,
				useRelaxedFiltering = UseRelaxedFiltering,
				overrideOffset = OverrideOffset.ToVector2I(),
				overridePlacement = OverridePlacement.ToVector2I(),
				allowedCellsFilter = AllowedCellsFilter ?? new()
			};

			return result;
		}
	}
}
