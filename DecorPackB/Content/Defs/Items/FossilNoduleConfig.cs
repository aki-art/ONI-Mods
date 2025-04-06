using UnityEngine;

namespace DecorPackB.Content.Defs.Items
{
	public class FossilNoduleConfig : EntityConfigBase
	{
		public static string ID = "DecorPackB_FossilNodule";

		public override GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.DECORPACKB_FOSSILNODULE.NAME,
				STRINGS.ITEMS.DECORPACKB_FOSSILNODULE.DESC,
				1f,
				false,
				Assets.GetAnim("dp_fossil_nodule_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.33f,
				true,
				0,
				SimHashes.Lime,
				[
					DPTags.buildingFossilNodule,
					GameTags.PedestalDisplayable
				]);

			return prefab;
		}
	}
}
