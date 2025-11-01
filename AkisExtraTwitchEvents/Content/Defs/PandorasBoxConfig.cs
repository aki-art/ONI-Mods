using System;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	internal class PandorasBoxConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_PandorasBox";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.AKISEXTRATWITCHEVENTS_PANDORASBOX.NAME,
				STRINGS.ITEMS.AKISEXTRATWITCHEVENTS_PANDORASBOX.DESC,
				100f,
				true,
				Assets.GetAnim("aete_pandorasbox_kanim"),
				"closed",
				Grid.SceneLayer.BuildingFront,
				EntityTemplates.CollisionShape.RECTANGLE,
				1f,
				1.1f,
				true,
				additionalTags: [
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				]
			);


			prefab.AddComponent<PandorasBox>().maxLifeTime = 30f;

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
