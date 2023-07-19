using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Moonlet.Entities.ComponentTypes
{
	public class EdibleComponent : BaseComponent
	{
		public int KcalPerUnit { get; set; }

		public int Quality { get; set; }

		public float PreserveTemperature { get; set; }

		public float? PreserveTemperatureCelsius { get; set; }

		public float RotTemperature { get; set; }

		public float? RotTemperatureCelsius { get; set; }

		public float SpoilTime { get; set; }

		public bool CanRot { get; set; }

		public List<string> EffectsVanilla { get; set; }

		public List<string> EffectsDlc { get; set; }

		public List<string> Effects { get; set; }


		public EdibleComponent()
		{
			PreserveTemperature = FOOD.DEFAULT_PRESERVE_TEMPERATURE;
			RotTemperature = FOOD.DEFAULT_ROT_TEMPERATURE;
			SpoilTime = FOOD.SPOIL_TIME.VERYSLOW;
		}

		public override void Apply(GameObject prefab)
		{
			var id = prefab.PrefabID().ToString();

			if (Quality < -1 || Quality > 6)
			{
				Log.Warning($"{prefab.PrefabID()} food quality must be between -1 and 6");
				Quality = Mathf.Clamp(Quality, -1, 6);
			}

			var foodInfo = new FoodInfo(
				prefab.PrefabID().ToString(),
				DlcManager.VANILLA_ID,
				KcalPerUnit * 1000f,
				Quality,
				PreserveTemperature,
				RotTemperature,
				Mathf.Max(0, SpoilTime),
				CanRot);

			if (EffectsVanilla != null)
				foodInfo.AddEffects(EffectsVanilla, DlcManager.AVAILABLE_VANILLA_ONLY);

			if (EffectsDlc != null)
				foodInfo.AddEffects(EffectsDlc, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			if (Effects != null)
				foodInfo.AddEffects(Effects, DlcManager.AVAILABLE_ALL_VERSIONS);

			EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

			if (prefab.TryGetComponent(out ElementChunk chunk))
			{
				Mod.sharedElementsLoader.edibleElements ??= new();
				Mod.sharedElementsLoader.edibleElements.Add(id);

				prefab.TryGetComponent(out PrimaryElement primaryElement);

				Strings.Add($"STRINGS.ITEMS.FOOD.{id.ToUpperInvariant()}.NAME", primaryElement.GetProperName());
				Strings.Add($"STRINGS.ITEMS.FOOD.{id.ToUpperInvariant()}.DESC", primaryElement.Element.Description());
			}
		}
	}
}
