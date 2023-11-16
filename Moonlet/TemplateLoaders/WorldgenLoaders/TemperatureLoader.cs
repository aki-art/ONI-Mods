using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class TemperatureLoader(TemperatureTemplate template, string sourceMod) : TemplateLoaderBase<TemperatureTemplate>(template, sourceMod)
	{
		public Temperature.Range TemperatureRange { get; private set; }

		public override void Initialize()
		{
			id = $"temperatures{relativePath}";
			template.Id = id;
			base.Initialize();

			Log.Debug("initializing temperatures " + template.Add.Count);

			foreach (var temp in template.Add)
			{
				Log.Debug("Registered temperature range " + temp.Key);
				Mod.temperaturesLoader.ranges[temp.Key] = (Temperature.Range)Hash.SDBMLower(temp.Key);
			}
		}

		public override void Validate()
		{
			base.Validate();
			foreach (var temp in template.Add)
			{
				if (temp.Value == null)
					Issue($"Temperature {temp.Key} needs to have a range defined.");
				else
				{
					var min = temp.Value.Min.CalculateOrDefault(0);
					var max = temp.Value.Max.CalculateOrDefault(9999);

					if (min < 0 || min > 9999)
					{
						Issue($"Temperature {temp.Key} min value {min}K is outside of allowed range: must be between 0K - 9999K");
						temp.Value.Min = 0;
					}

					if (max < 0 || max > 9999)
					{
						Issue($"Temperature {temp.Key} max value {max}K is outside of allowed range: must be between 0K - 9999K");
						temp.Value.Max = min;
					}

					if (min > max)
					{
						Issue($"Temperature {temp.Key} is invalid, min {min}K needs to be smaller or equal to max {max}K. Defaulted both to the value of min ({min}K)");
						temp.Value.Max = min;
					}
				}
			}
		}

		public void LoadContent()
		{
			var table = template.Add;
			foreach (var temperature in table)
			{
				if (Mod.temperaturesLoader.ranges.TryGetValue(temperature.Key, out var range))
					SettingsCache.temperatures[range] = temperature.Value.ToTemperature();
			}
		}

		public override void RegisterTranslations()
		{
		}

	}
}
