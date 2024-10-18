using Moonlet.Scripts.Moonlet.Entities;
using Moonlet.Templates;

namespace Moonlet.TemplateLoaders
{
	public class BuildingLoader(BuildingTemplate template, string sourceMod) : BuildingLoaderBase<BuildingTemplate>(template, sourceMod)
	{
		public override IBuildingConfig CreateConfig() => new GenericBuildingConfig(false, template);
	}
}
