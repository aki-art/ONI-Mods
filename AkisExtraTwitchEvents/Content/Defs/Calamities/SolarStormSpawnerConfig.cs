using Twitchery.Content.Scripts.WorldEvents;
using UnityEngine;

namespace Twitchery.Content.Defs.Calamities
{
	public class SolarStormSpawnerConfig : EntityConfigBase
	{
		public const string ID = "AETE_SolarStormSpawner";

		public override GameObject CreatePrefab()
		{
			var prefab = CreateBasic(ID);

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			var calamity = prefab.AddComponent<AETE_SolarStorm>();
			calamity.durationInSeconds = 900f;
			calamity.bigEvent = true;

			return prefab;
		}
	}
}
