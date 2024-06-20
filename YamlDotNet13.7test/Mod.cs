/* Test to see if a merged in YamlDotNet with a newer version
 * - works
 * - clashes with games version
 * - clashes with others mods version
 * seems all good */

extern alias YamlDotNetButNew;
using HarmonyLib;
using KMod;
using System.IO;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace YamlDotNet13._7test
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			ReadYaml<object>("C:/Users/Aki/Documents/Klei/OxygenNotIncluded/mods/dev/Moonlet_dev/moonlet/data/tags.yaml");
		}

		public static T ReadYaml<T>(string path) where T : class
		{
			var builder = new DeserializerBuilder()
				.WithNamingConvention(FixedCamelCaseConvention.Instance)
				.IncludeNonPublicProperties()
				.IgnoreUnmatchedProperties();

			var deserializer = builder.Build();

			var content = File.ReadAllText(path);
			return deserializer.Deserialize<T>(content);
		}

	}
}