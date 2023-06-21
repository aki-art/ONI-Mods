using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class PolymorphTrackerConfig : IEntityConfig
	{
		public const string ID = "AkisTwitchEvents_PolymorphTracker";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Polymorph Tracker", false);

			prefab.AddComponent<MinionStorage>();

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
