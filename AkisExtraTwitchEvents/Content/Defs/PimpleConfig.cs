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
			pimple.minLiquidMult = 0.5f;
			pimple.maxLiquidMult = 3f;
			pimple.liquidLossPerSecond = pimple.maxLiquidMult / 1200f;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
