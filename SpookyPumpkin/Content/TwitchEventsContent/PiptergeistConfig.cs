using SpookyPumpkinSO.Content.Cmps;
using UnityEngine;

namespace SpookyPumpkinSO.Content.TwitchEventsContent
{
	public class PiptergeistConfig : IEntityConfig
	{
		public const string ID = "SpookyPumpkin_Piptergeist";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				ID,
				"piptergeist",
				"",
				10f,
				true,
				Assets.GetAnim("sp_ghostpip_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				additionalTags: new()
				{
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				});

			prefab.AddComponent<Piptergeist>();

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
