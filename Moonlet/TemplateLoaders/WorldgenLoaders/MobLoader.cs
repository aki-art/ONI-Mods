using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System.IO;
using YamlDotNet.Serialization;

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
			Log.Debug("converting mobs");
			var table = template.GetValue();
			foreach (var mob in table)
			{
				SettingsCache.mobs.MobLookupTable[mob.Key] = mob.Value.Convert();
				Log.DebugProperties(SettingsCache.mobs.MobLookupTable[mob.Key]);

				var testFileNameProcessed = "C:/Users/Aki/Desktop/yaml tests/" + id.LinkAppropiateFormat() + "_Processed.yaml";
				using StreamWriter writer2 = new(testFileNameProcessed);
				new SerializerBuilder()
					.EmitDefaults()
					.Build()
					.Serialize(writer2, SettingsCache.mobs.MobLookupTable[mob.Key]);
				writer2.Close();

			}
		}

		public override void RegisterTranslations()
		{
		}
	}
}
