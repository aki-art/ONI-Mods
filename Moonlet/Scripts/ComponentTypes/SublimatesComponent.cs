using Moonlet.Templates;
using Moonlet.Utils;
using System;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class SublimatesComponent : BaseComponent
	{
		public SublimatesData Data { get; set; }

		public class SublimatesData
		{
			public string Element { get; set; }

			public FloatNumber Rate { get; set; }

			public FloatNumber MinMass { get; set; }

			public string SublimateFx { get; set; }

			public FloatNumber MaxDestinationMass { get; set; }

			public FloatNumber MassPower { get; set; }

			public string Disease { get; set; }

			public IntNumber DiseaseCount { get; set; }

			public SublimatesData()
			{
				MassPower = 1f;
				MaxDestinationMass = 1.8f;
			}
		}

		public override bool CanApplyTo(GameObject prefab)
		{
			var isValid = prefab.TryGetComponent<ElementChunk>(out _);

			if (!isValid)
				Log.Warn($"Component type {nameof(SublimatesComponent)} can only be added to element debris items!");

			return isValid;
		}

		public override void Apply(GameObject prefab)
		{
			var sublimates = prefab.AddOrGet<Sublimates>();
			var sublimatedElement = Data.Element;

			var sub = ElementLoader.FindElementByName(sublimatedElement);

			if (sub == null)
				return;

			if (!Enum.TryParse<SpawnFXHashes>(Data.SublimateFx, out var fx))
			{
				Log.Warn($"Issue with {Data.Element} debris entry: {Data.SublimateFx} is not a registered SpawnFxHashes. Add an entry to fx.");
				fx = SpawnFXHashes.OxygenEmissionBubbles;
			}

			sublimates.spawnFXHash = fx;

			sublimates.info = new Sublimates.Info(
				Data.Rate.CalculateOrDefault(0),
				Data.MinMass.CalculateOrDefault(0),
				Data.MaxDestinationMass.CalculateOrDefault(0),
				Data.MassPower.CalculateOrDefault(0),
				sub.id);

			if (!Data.Disease.IsNullOrWhiteSpace())
			{
				var diseaseIdx = Db.Get().Diseases.GetIndex(Data.Disease);
				if (diseaseIdx != byte.MaxValue)
				{
					sublimates.info.diseaseIdx = diseaseIdx;
					sublimates.info.diseaseCount = Data.DiseaseCount;
				}
			}
		}
	}
}
