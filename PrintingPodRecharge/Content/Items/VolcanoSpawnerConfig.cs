using UnityEngine;

namespace PrintingPodRecharge.Content.Items
{
	public class VolcanoSpawnerConfig : IEntityConfig
	{
		public const string ID = "PrintingPodRecharge_VolcanoSpawner";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				global::STRINGS.CREATURES.SPECIES.GEYSER.SMALL_VOLCANO.NAME,
				global::STRINGS.CREATURES.SPECIES.GEYSER.SMALL_VOLCANO.DESC + "...",
				100,
				true,
				Assets.GetAnim("geyser_molten_volcano_small_kanim"),
				"idle",
				Grid.SceneLayer.Ore,
				EntityTemplates.CollisionShape.RECTANGLE,
				isPickupable: true);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			FUtility.Utils.Spawn("GeyserGeneric_" + GeyserGenericConfig.SmallVolcano, inst);
			Util.KDestroyGameObject(inst);
		}
	}
}
