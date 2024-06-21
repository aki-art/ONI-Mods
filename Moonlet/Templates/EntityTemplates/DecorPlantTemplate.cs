namespace Moonlet.Templates.EntityTemplates
{
	public class DecorPlantTemplate : BasePlantTemplate
	{
		public DecorPlantTemplate()
		{
			//SafeElements = ElementUtil.GetAllWithTag(ModTags.SafeAtmosphereForPlants);
			SafeElements = new string[]
			{
				SimHashes.Oxygen.ToString(),
				SimHashes.ContaminatedOxygen.ToString(),
				SimHashes.CarbonDioxide.ToString(),
			};
			// TODO: add tags to elements
		}
	}
}
