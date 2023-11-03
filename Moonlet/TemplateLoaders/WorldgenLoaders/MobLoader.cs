using Moonlet.Templates.WorldGenTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class MobLoader(MobTemplate template, string sourceMod) : TemplateLoaderBase<MobTemplate>(template, sourceMod)
	{
		public override void Initialize()
		{
			id = $"{sourceMod}/mobs";
			template.Id = id;
			base.Initialize();
		}

		public void LoadContent()
		{
			var table = template.GetValue();
			foreach (var mob in table)
			{
				Log.Debug("LOADED MOB: " + mob.Key);
				SettingsCache.mobs.MobLookupTable[mob.Key] = mob.Value;

				Log.Debug($"{mob.Value.prefabName} {mob.Value.location} {mob.Value.density}");
			}
		}

		public override void RegisterTranslations()
		{
		}
	}
}
