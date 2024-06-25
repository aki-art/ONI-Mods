using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class EdibleComponent : BaseComponent
	{
		public EdibleData Data { get; set; }

		public override bool CanApplyTo(GameObject prefab)
		{
			var isPickupable = prefab.TryGetComponent<Pickupable>(out _);

			if (!isPickupable)
				Log.Warn($"Component type {nameof(EdibleComponent)} can only be added to pickupable items!");

			return isPickupable;
		}

		public class EdibleData
		{
			public IntNumber KcalPerUnit { get; set; }

			public IntNumber Quality { get; set; }

			public float PreserveTemperature { get; set; }

			public float? PreserveTemperatureCelsius { get; set; }

			public float RotTemperature { get; set; }

			public float? RotTemperatureCelsius { get; set; }

			public float SpoilTime { get; set; }

			public bool CanRot { get; set; }

			public List<string> EffectsVanilla { get; set; }

			public List<string> EffectsDlc { get; set; }

			public List<string> Effects { get; set; }

			public EdibleData()
			{
				PreserveTemperature = FOOD.DEFAULT_PRESERVE_TEMPERATURE;
				RotTemperature = FOOD.DEFAULT_ROT_TEMPERATURE;
				SpoilTime = FOOD.SPOIL_TIME.VERYSLOW;
			}
		}

		public override void Apply(GameObject prefab)
		{
			var id = prefab.PrefabID().ToString();
			Log.Debug("Applying edible component to " + id);

			if (Data.Quality < -1 || Data.Quality > 6)
			{
				Log.Warn($"{prefab.PrefabID()} food quality must be between -1 and 6");
				Data.Quality = Mathf.Clamp(Data.Quality, -1, 6);
			}

			var foodInfo = new EdiblesManager.FoodInfo(
				id,
				DlcManager.VANILLA_ID,
				Data.KcalPerUnit.CalculateOrDefault(0) * 1000f,
				Data.Quality.CalculateOrDefault(-1),
				Data.PreserveTemperature,
				Data.RotTemperature,
				Mathf.Max(0, Data.SpoilTime),
				Data.CanRot);

			if (Data.EffectsVanilla != null)
				foodInfo.AddEffects(Data.EffectsVanilla, DlcManager.AVAILABLE_VANILLA_ONLY);

			if (Data.EffectsDlc != null)
				foodInfo.AddEffects(Data.EffectsDlc, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			if (Data.Effects != null)
				foodInfo.AddEffects(Data.Effects, DlcManager.AVAILABLE_ALL_VERSIONS);

			EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

			// TODO: edible elements
			/*	if (prefab.TryGetComponent(out ElementChunk _))
				{
					Mod.sharedElementsLoader.edibleElements ??= new();
					Mod.sharedElementsLoader.edibleElements.Add(id);

					prefab.TryGetComponent(out PrimaryElement primaryElement);

					Strings.Add($"STRINGS.ITEMS.FOOD.{id.ToUpperInvariant()}.NAME", primaryElement.GetProperName());
					Strings.Add($"STRINGS.ITEMS.FOOD.{id.ToUpperInvariant()}.DESC", primaryElement.Element.Description());
				}*/
		}
	}
}
