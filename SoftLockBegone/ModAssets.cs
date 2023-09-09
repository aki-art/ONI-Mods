using UnityEngine;
using Klei.AI;
using SoftLockBegone.Content.Scripts;

namespace SoftLockBegone
{
	public class ModAssets
	{
		public static GameObject placeHolderTemplate;

		public static void InitTemplate()
		{
			placeHolderTemplate = new GameObject("SoftLockBegone_Placeholder");
			placeHolderTemplate.AddComponent<KPrefabID>();
			placeHolderTemplate.AddComponent<KSelectable>().IsSelectable = true;

			// 3 storages to catch most cases
			placeHolderTemplate.AddComponent<Storage>();
			placeHolderTemplate.AddComponent<Storage>();
			placeHolderTemplate.AddComponent<Storage>();

			placeHolderTemplate.AddComponent<PrimaryElement>();

			placeHolderTemplate.AddComponent<Deconstructable>().allowDeconstruction = false;
			placeHolderTemplate.AddComponent<SaveLoadRoot>();
			placeHolderTemplate.AddComponent<OccupyArea>().SetCellOffsets(new[] { CellOffset.none });
			placeHolderTemplate.AddComponent<KBoxCollider2D>().size = new Vector2(1, 1);

			placeHolderTemplate.AddComponent<SLB_EntityComponent>();

			Object.DontDestroyOnLoad(placeHolderTemplate);

			placeHolderTemplate.SetActive(false);

			var kbac = placeHolderTemplate.AddComponent<KBatchedAnimController>();
			kbac.sceneLayer = Grid.SceneLayer.Ore;
			kbac.initialAnim = "object";
			kbac.AnimFiles = new[]
			{
				Assets.GetAnim("slb_missing_kanim")
			};
		}
	}
}
