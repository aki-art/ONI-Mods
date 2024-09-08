using Moonlet.DocGen;
using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class SpiceTemplate : ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Priority { get; set; }

		public string[] DlcIds { get; set; }

		public ITemplate.MergeBehavior Command { get; set; }

		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		[Doc("Sprite that represents the bottle in the UI.")]
		public string Icon { get; set; }

		public string Description { get; set; }

		[Doc("List of effects to apply when a duplicants eats a food with this")]
		public AttributeModifierEntry[] Effects { get; set; }

		[Doc("Effects on the food itself. No vanilla implementation (does nothing)")]
		public AttributeModifierEntry[] FoodEffect { get; set; }

		[Doc("What color to tint the Spice Grinders animation")]
		public ColorEntry Color { get; set; }

		[Doc("A food with this spice has this many more or less calories.")]
		public float KCalModifier { get; set; }

		[Doc("The list ingredients needed to be delivered to the Spice Grinder")]
		public SpiceIngredientEntry[] Ingredients { get; set; }

		public class SpiceIngredientEntry : IDocumentation
		{
			public string Tag { get; set; }

			public float Amount { get; set; }

			public void ModifyDocs(DocPage page)
			{
			}
		}
	}
}
