using Moonlet.Utils;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class ElementsLoader(string path) : TemplatesLoader<TemplateLoaders.ElementLoader>(path)
	{
		public void LoadElements(Dictionary<string, SubstanceTable> substanceTablesByDlc)
		{
			var substances = substanceTablesByDlc[DlcManager.VANILLA_ID].GetList();
			ApplyToActiveTemplates(element => element.LoadContent(ref substances));
			DumpElementData();
		}

		private void DumpElementData()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"" +
				$"Name," +
				$"State," +
				$"SHC," +
				$"TC," +
				$"Hardness," +
				$"High Temp," +
				$"High Temp Output," +
				$"Low Temp," +
				$"Low Temp Output," +
				$"Radiation Absorption," +
				$"Radiation," +
				$"Decor," +
				$"OverHeat");

			foreach (var entry in GetTemplates())
			{
				var template = entry.template;
				if (template == null)
					continue;

				var highTempTrans = template.HighTempTransitionTarget;

				if (template.HighTempTransitionOreId != null)
				{
					var p = GameUtil.GetFormattedPercent(template.HighTempTransitionOreMassConversion.CalculateOrDefault(0));
					var p2 = GameUtil.GetFormattedPercent(1.0f - template.HighTempTransitionOreMassConversion.CalculateOrDefault(0));

					highTempTrans = $"{p} {template.HighTempTransitionTarget}: {p2} {template.HighTempTransitionOreId}";
				}

				var lowTempTrans = template.LowTempTransitionTarget;

				if (template.LowTempTransitionOreId != null)
				{
					var p = GameUtil.GetFormattedPercent(template.LowTempTransitionOreMassConversion);
					var p2 = GameUtil.GetFormattedPercent(1.0f - template.LowTempTransitionOreMassConversion);

					lowTempTrans = $"{p}% {template.LowTempTransitionTarget}: {p2} {template.LowTempTransitionOreId}";
				}

				float decor = 0;
				if (template.Modifiers != null)
				{
					var decorMod = template.Modifiers.FindIndex(mod => mod.Id == "Decor");
					if (decorMod != -1)
					{
						decor = template.Modifiers[decorMod].Value;
					}
				}

				float overheat = 0;
				if (template.Modifiers != null)
				{
					var overheatMod = template.Modifiers.FindIndex(mod => mod.Id == "OverHeat");
					if (overheatMod != -1)
					{
						overheat = template.Modifiers[overheatMod].Value;
					}
				}

				stringBuilder.AppendLine($"{template.Name}," +
				$"{template.State}," +
				$"{template.SpecificHeatCapacity.CalculateOrDefault(0)}," +
				$"{template.ThermalConductivity.CalculateOrDefault(0)}," +
				$"{template.Hardness.CalculateOrDefault(0)}," +
				$"{template.HighTemp.CalculateOrDefault(0)}," +
				$"{highTempTrans}," +
				$"{template.LowTemp.CalculateOrDefault(0)}," +
				$"{lowTempTrans}," +
				$"{GameUtil.GetFormattedPercent(template.RadiationAbsorptionFactor)}," +
				$"{template.RadiationPer1000Mass.CalculateOrDefault(0)}," +
				$"{(decor == 0 ? "" : GameUtil.GetFormattedPercent(decor))}," +
				$"{(overheat == 0 ? "" : GameUtil.GetFormattedPercent(overheat))}");
			}

			string path1 = Path.Combine(FUtility.Utils.ModPath, "element_dump.txt");
			File.WriteAllText(path1, stringBuilder.ToString());
			Log.Debug($"Dumped elements to {path1}");
		}

		public void SetExposureValues(Dictionary<SimHashes, float> customExposureRates)
		{
			foreach (var template in loaders)
			{
				if (template.isActive)
					template.SetExposureValue(customExposureRates);
			}
		}

		public override void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry)
		{
			base.LoadYamls<TemplateType>(mod, singleEntry);

			if (loaders.Count > 0)
				OptionalPatches.requests |= OptionalPatches.PatchRequests.Enums;
		}

		public void AddElementYamlCollection(List<ElementLoader.ElementEntry> result)
		{
			foreach (var template in loaders)
			{
				if (template.isActive)
					result.Add(template.ToElementEntry());
			}
		}

		public void CreateUnstableFallers(ref List<UnstableGroundManager.EffectInfo> effects, UnstableGroundManager.EffectInfo referenceEffect)
		{
			foreach (var element in loaders)
			{
				if (!element.isActive)
					continue;

				var info = element.elementInfo;

				if (element.IsUnstable())
					effects.Add(CreateEffect(info.SimHash, info.id, referenceEffect.prefab));
			}
		}

		private static UnstableGroundManager.EffectInfo CreateEffect(SimHashes element, string prefabId, GameObject referencePrefab)
		{
			var prefab = Object.Instantiate(referencePrefab);
			prefab.name = $"Unstable{prefabId}";

			return new UnstableGroundManager.EffectInfo()
			{
				prefab = prefab,
				element = element
			};
		}

		internal void LoadInfos()
		{
			ApplyToActiveTemplates(template => template.CreateInfo());
		}
	}
}
