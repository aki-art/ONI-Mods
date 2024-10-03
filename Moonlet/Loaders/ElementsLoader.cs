using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class ElementsLoader(string path) : TemplatesLoader<TemplateLoaders.ElementLoader>(path)
	{
		public void LoadElements(Dictionary<string, SubstanceTable> substanceTablesByDlc)
		{
			var substances = substanceTablesByDlc[DlcManager.VANILLA_ID].GetList();
			ApplyToActiveTemplates(element => element.LoadContent(ref substances));
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

		public void LoadInfos()
		{
			ApplyToActiveTemplates(template => template.CreateInfo());
		}
	}
}
