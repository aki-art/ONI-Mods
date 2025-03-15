using Twitchery.Content.Scripts.WorldEvents;
using UnityEngine;

namespace Twitchery.Content.Defs.Calamities
{
	public class BlizzardSpawnerConfig : EntityConfigBase
	{
		public const string ID = "AETE_BlizzardSpawner";

		public override GameObject CreatePrefab()
		{
			var prefab = CreateBasic(ID);

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			var calamity = prefab.AddComponent<AETE_Blizzard>();
			calamity.durationInSeconds = 150f;
			calamity.intenseRadius = 8f;
			calamity.nearSnowfallDensity = 0.1f;
			calamity.baseSnowfallDensityPerSquare100 = 3;
			calamity.bigEvent = true;

			return prefab;
		}
	}
}
