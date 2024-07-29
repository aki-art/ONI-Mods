using Moonlet.DocGen;

namespace Moonlet.Templates.SubTemplates
{
	public class ModifierEntry : IDocumentation
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public FloatNumber Value { get; set; }

		public bool IsMultiplier { get; set; }

		public void ModifyDocs(DocPage page)
		{
		}
	}
}
