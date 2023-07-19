using FUtility;
using System;
using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class SublimatesComponent : BaseComponent
	{
		public string Element { get; set; }

		public float Rate { get; set; }

		public float MinMass { get; set; }

		public string SublimateFx { get; set; }

		public float MaxDestinationMass { get; set; }

		public float MassPower { get; set; }

		public string Disease { get; set; }

		public int DiseaseCount { get; set; }

		public SublimatesComponent()
		{
			MassPower = 1f;
			MaxDestinationMass = 1.8f;
		}

		public override void Apply(GameObject prefab)
		{
			var sublimates = prefab.AddOrGet<Sublimates>();
			var sublimatedElement = Element;

			var sub = ElementLoader.FindElementByName(sublimatedElement);

			if (sub == null)
				return;

			if (!Enum.TryParse<SpawnFXHashes>(SublimateFx, out var fx))
			{
				Log.Warning($"{SublimateFx} is not a registered SpawnFxHashes. Add an entry to fx.");
				fx = SpawnFXHashes.OxygenEmissionBubbles;
			}

			sublimates.spawnFXHash = fx;

			sublimates.info = new Sublimates.Info(
				Rate,
				MinMass,
				MaxDestinationMass,
				MassPower,
				sub.id);

			if (!Disease.IsNullOrWhiteSpace())
			{
				var diseaseIdx = Db.Get().Diseases.GetIndex(Disease);
				if (diseaseIdx != byte.MaxValue)
				{
					sublimates.info.diseaseIdx = diseaseIdx;
					sublimates.info.diseaseCount = DiseaseCount;
				}
			}
		}
	}
}
