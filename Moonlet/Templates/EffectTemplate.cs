using Moonlet.Templates.SubTemplates;

namespace Moonlet.Templates
{
	public class EffectTemplate : ITemplate
	{
		public string GetId() => Id;

		public string Id { get; set; }

		public string Name { get; set; }

		public string Tooltip { get; set; }

		public bool AtmoSuitImmunity { get; set; }

		public string Icon { get; set; }

		public float DurationSeconds { get; set; }

		public bool IsBad { get; set; }

		public bool TriggerFloatingText { get; set; } = true;

		public bool ShowInUI { get; set; } = true;

		public ModifierEntry[] Modifiers { get; set; }
	}
}
