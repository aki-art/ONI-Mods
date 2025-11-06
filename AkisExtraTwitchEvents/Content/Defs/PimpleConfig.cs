using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class PimpleConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_Pimple";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(ID,
				STRINGS.MISC.AKISEXTRATWITCHEVENTS_PIMPLE.NAME,
				"",
				100f,
				Assets.GetAnim("aete_pimple_kanim"),
				"idle",
				Grid.SceneLayer.BuildingFront,
				1,
				1,
				default);

			var pimple = prefab.AddComponent<Pimple>();
			pimple.minLiquidMult = 3f;
			pimple.maxLiquidMult = 7f;
			pimple.minDangerousLiquidMult = 0.5f;
			pimple.maxDangerousLiquidMult = 2f;
			pimple.durationSeconds = 900f;

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
