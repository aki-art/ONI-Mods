using Moonlet.Scripts.Moonlet.Entities;
using Moonlet.Templates;

namespace Moonlet.TemplateLoaders
{
	public class BuildingLoader(BuildingTemplate template, string sourceMod) : BuildingLoaderBase<BuildingTemplate>(template, sourceMod)
	{
		public override void CreateAndRegister()
		{
			var def = ConfigureDef();

			var config = new GenericBuildingConfig
			{
				def = def,
				skipLoading = false
			};

			if (!template.OverlayMode.IsNullOrWhiteSpace())
				def.ViewMode = template.OverlayMode;

			ConfigureConduits(def);
			ConfigurePower(def);
			RegisterBuilding(config, def, template);
			AddToTech();
			AddToMenu();
		}
	}
}
