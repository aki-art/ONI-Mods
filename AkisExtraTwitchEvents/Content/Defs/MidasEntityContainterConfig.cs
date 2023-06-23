using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	internal class MidasEntityContainterConfig : IEntityConfig
	{
		public const string ID = "AkisTwitchEvents_MidasEntityContainer";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.MISC.MIDAS_STATE,
				"",
				100f,
				Assets.GetAnim("barbeque_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				1,
				1,
				TUNING.DECOR.BONUS.TIER8);

			prefab.AddComponent<MinionStorage>();
			prefab.AddComponent<MidasEntityContainer>();
			prefab.AddComponent<Storage>();
			SymbolOverrideControllerUtil.AddToPrefab(prefab);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
