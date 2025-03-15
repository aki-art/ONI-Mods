using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class CharredEntityContainerConfig : IEntityConfig
	{
		public const string ID = "AkisTwitchEvents_CharredEntity";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				"x",
				"",
				100f,
				true,
				Assets.GetAnim("barbeque_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1,
				1,
				true,
				0,
				SimHashes.Carbon,
				[
					GameTags.PedestalDisplayable,
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				]);


			prefab.AddComponent<MinionStorage>();
			prefab.AddComponent<AnimationMimic>();
			prefab.AddComponent<CharredEntity>();
			prefab.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
			prefab.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER2);

			SymbolOverrideControllerUtil.AddToPrefab(prefab);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
