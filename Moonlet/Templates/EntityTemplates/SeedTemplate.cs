namespace Moonlet.Templates.EntityTemplates
{
	public class SeedTemplate : ItemTemplate
	{
		public string PlantId { get; set; }

		public string ReplantGroundTag { get; set; }

		public int SortOrder { get; set; } = 0;
	}
}
