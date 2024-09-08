using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class EffectTemplate : ITemplate, IOptional
	{
		public string GetId() => Id;

		public string Id { get; set; }

		public string Name { get; set; }

		public ITemplate.MergeBehavior Command { get; set; }

		public string[] Tags { get; set; }

		public string Tooltip { get; set; }

		public bool AtmoSuitImmunity { get; set; }

		public string Icon { get; set; }

		public float DurationSeconds { get; set; }

		public bool IsBad { get; set; }

		public bool TriggerFloatingText { get; set; } = true;

		public bool ShowInUI { get; set; } = true;

		public ModifierEntry[] Modifiers { get; set; }

		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		public bool Optional { get; set; }
	}
}
