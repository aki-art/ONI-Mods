using Moonlet.Scripts;
using Moonlet.Templates;

namespace Moonlet.TemplateLoaders
{
	public class TileLoader(TileTemplate template, string sourceMod) : BuildingLoaderBase<TileTemplate>(template, sourceMod)
	{
		public override IBuildingConfig CreateConfig()
		{
			return new TileBuildingConfig(false, template)
			{
				sourceMod = sourceMod
			};
		}
	}
}
