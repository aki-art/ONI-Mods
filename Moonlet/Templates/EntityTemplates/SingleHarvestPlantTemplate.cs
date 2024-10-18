namespace Moonlet.Templates.EntityTemplates
{
	public class SingleHarvestPlantTemplate : BasePlantTemplate
	{
		public string DeathAnimation { get; set; }
		public string DeathFx { get; set; }

		public SingleHarvestPlantTemplate() : base()
		{
			DeathAnimation = "harvest";
		}
	}
}
