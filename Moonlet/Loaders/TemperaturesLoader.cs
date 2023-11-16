using Moonlet.TemplateLoaders.WorldgenLoaders;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class TemperaturesLoader(string path) : TemplatesLoader<TemperatureLoader>(path)
	{
		public Dictionary<string, Temperature.Range> ranges = new();

		public override void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry)
		{
			ranges = new();
			base.LoadYamls<TemplateType>(mod, singleEntry);
		}
	}
}
