using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	internal class BranchWalkerConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_BranchWalker";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				ID,
				"Branch Placer",
				"",
				1f,
				false,
				Assets.GetAnim("barbeque_kanim"),
				"object",
				Grid.SceneLayer.Front);

			var walker = prefab.AddComponent<BranchWalker>();
			walker.branchOffChance = 0.3f;
			walker.maxDistance = 10;
			walker.barkElement = Elements.FakeLumber;
			walker.barkMass = 50f;
			walker.stepRange = 4;
			walker.minimumSteps = 4;
			walker.maximumSteps = 7;
			walker.foliageElement = SimHashes.Algae;
			walker.foliageRadius = 3;
			walker.foliageMass = 200f;
			walker.foliageDensity = 0.9f;
			walker.maxComplexity = 1; // 2 is already giant
			walker.maxHardness = 50;

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
