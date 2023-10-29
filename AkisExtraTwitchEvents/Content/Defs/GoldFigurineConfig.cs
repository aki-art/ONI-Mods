/*using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class GoldFigurineConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_GoldFigurine";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				ID,
				"A creature turned into solid gold.",
				30f,
				false,
				Assets.GetAnim("rrp_catdrawing_kanim"),
				"object",
				Grid.SceneLayer.BuildingBack,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.9f,
				true,
				0,
				SimHashes.Creature,
				additionalTags: new List<Tag>
				{
					GameTags.MiscPickupable,
					GameTags.PedestalDisplayable
				});

			prefab.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
			prefab.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER2);

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
*/