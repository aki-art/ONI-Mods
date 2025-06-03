extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;
using static ProcGen.World;

namespace Moonlet.Templates.SubTemplates
{
	[Serializable]
	public class TemplateSpawnRuleTemplate : BaseTemplate, IShadowTypeBase<TemplateSpawnRules>
	{
		public string RuleId { get; set; }
		public List<string> Names { get; set; }
		public TemplateSpawnRules.ListRule ListRule { get; set; }
		public IntNumber SomeCount { get; set; }
		public IntNumber MoreCount { get; set; }
		public IntNumber Times { get; set; }

		[YamlMember(Alias = "templatePriority")]
		public new string Priority { get; set; }

		[YamlMember(Alias = "priority")]
		public FloatNumber RulePriority { get; set; }
		public bool AllowDuplicates { get; set; }
		public bool AllowExtremeTemperatureOverlap { get; set; }
		public bool UseRelaxedFiltering { get; set; }
		public bool AllowNearStart { get; set; }
		public Vector2IC OverrideOffset { get; set; }
		public Vector2IC OverridePlacement { get; set; }
		public List<AllowedCellsFilterC> AllowedCellsFilter { get; set; }

		public TemplateSpawnRuleTemplate()
		{
			AllowedCellsFilter = [];
			OverrideOffset = new Vector2IC(0, 0);
			OverridePlacement = new Vector2IC(-1, -1);
			AllowDuplicates = false;
			UseRelaxedFiltering = false;
			AllowNearStart = false;
		}

		public TemplateSpawnRules Convert(Action<string> log)
		{
			var result = new TemplateSpawnRules
			{
				ruleId = RuleId ?? Id,
				names = Names,
				listRule = ListRule,
				someCount = SomeCount.CalculateOrDefault(0),
				moreCount = MoreCount.CalculateOrDefault(0),
				times = Times.CalculateOrDefault(1),
				priority = RulePriority.CalculateOrDefault(0),
				allowDuplicates = AllowDuplicates,
				allowExtremeTemperatureOverlap = AllowExtremeTemperatureOverlap,
				useRelaxedFiltering = UseRelaxedFiltering,
				allowNearStart = AllowNearStart,
				overrideOffset = OverrideOffset.ToVector2I(),
				overridePlacement = OverridePlacement.ToVector2I(),
				allowedCellsFilter = ShadowTypeUtil.CopyList<AllowedCellsFilter, AllowedCellsFilterC>(AllowedCellsFilter, str => Log.Warn(str)) ?? []
			};

			return result;
		}
	}
}
