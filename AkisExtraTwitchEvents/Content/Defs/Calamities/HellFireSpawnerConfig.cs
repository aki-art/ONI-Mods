using Twitchery.Content.Scripts.WorldEvents;
using UnityEngine;

namespace Twitchery.Content.Defs.Calamities
{
	public class HellFireSpawnerConfig : EntityConfigBase
	{
		public const string ID = "AETE_HellFireSpawner";

		public override GameObject CreatePrefab()
		{
			var prefab = CreateBasic(ID);

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			var calamity = prefab.AddComponent<AETE_HellFire>();
			calamity.durationInSeconds = 150f;
			calamity.radius = 4;
			calamity.density = 0.1f;
			calamity.bigEvent = true;

			return prefab;
		}
	}
}
