using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Meds
{
	public class WereVoleCureConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_WereVoleCure";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_WEREVOLECURE.NAME,
				STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_WEREVOLECURE.DESC,
				1f,
				true,
				Assets.GetAnim("pill_foodpoisoning_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true,
				additionalTags: new List<Tag>()
				{
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				});

			prefab.AddComponent<AssignableWerevoleCure>();

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
