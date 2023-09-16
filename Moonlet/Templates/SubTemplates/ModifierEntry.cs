namespace Moonlet.Templates.SubTemplates
{
	public class ModifierEntry : ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public float Value { get; set; }

		public bool IsMultiplier { get; set; }

		public string GetId() => Id;
	}
}
