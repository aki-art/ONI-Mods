using Klei.AI;
using Moonlet.DocGen;
using Moonlet.Utils;

namespace Moonlet.Templates.SubTemplates
{
	public class AttributeModifierEntry : ModifierEntry, IDocumentation
	{
		public string AttributeId { get; private set; }

		public GameUtil.TimeSlice OverrideTimeSlice { get; set; }

		public bool UIOnly { get; private set; }

		public bool IsReadonly { get; private set; }

		public AttributeModifier Create(string description)
		{
			return new AttributeModifier(AttributeId, Value.CalculateOrDefault(0), description, IsMultiplier, UIOnly, IsReadonly);
		}
	}
}
