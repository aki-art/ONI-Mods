using Twitchery.Content.Scripts.WorldEvents;
using UnityEngine;

namespace Twitchery.Content.Defs.Calamities
{
	public class SandstormSpawnerConfig : EntityConfigBase
	{
		public const string ID = "AETE_SandstormSpawner";

		public override GameObject CreatePrefab()
		{
			var prefab = CreateBasic(ID);

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			var calamity = prefab.AddComponent<AETE_SandStorm>();
			calamity.durationInSeconds = 150f;
			calamity.intenseRadius = 8f;
			calamity.nearSandfallDensity = 0.1f;
			calamity.baseSandfallDensityPerSquare100 = 0;
			calamity.bigEvent = true;

			return prefab;
		}
	}
}
