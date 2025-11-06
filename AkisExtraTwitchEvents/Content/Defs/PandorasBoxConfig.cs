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

			var pandorasBox = prefab.AddComponent<PandorasBox>();
			pandorasBox.maxLifeTime = 200.0f;//CONSTS.CYCLE_LENGTH * 3.0f;
			pandorasBox.frequencyMin = 0.01f;
			pandorasBox.frequencyMax = 0.03f;
			pandorasBox.shakeSpeed = 0.1f;
			pandorasBox.amplitude = 0.1f;

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
