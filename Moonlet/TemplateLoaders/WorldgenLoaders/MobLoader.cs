using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class MobLoader(MobTemplate template, string sourceMod) : TemplateLoaderBase<MobTemplate>(template, sourceMod)
	{
		public static Mob.Location AnyWater = MobLocationUtil.Register("Moonlet_AnyWater");
		public static Mob.Location Liquid = MobLocationUtil.Register("Moonlet_Liquid");

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
				var id = MoonletMods.Instance.GetPrefixedId(sourceMod, mob.Key);
				SettingsCache.mobs.MobLookupTable[id] = mob.Value.Convert();
			}
		}

		public override void RegisterTranslations()
		{
		}
	}
}
