using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class SmallBranchWalkerConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_SmallBranchWalker";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				ID,
				"Small Branch Placer",
				"",
				1f,
				false,
				Assets.GetAnim("barbeque_kanim"),
				"object",
				Grid.SceneLayer.Front);

			var walker = prefab.AddComponent<BranchWalker>();
			walker.branchOffChance = 0.1f;
			walker.maxDistance = 3;
			walker.barkElement = Elements.FakeLumber;
			walker.barkMass = 20f;
			walker.stepRange = 4;
			walker.minimumSteps = 1;
			walker.maximumSteps = 3;
			walker.foliageElement = SimHashes.Algae;
			walker.foliageRadius = 2;
			walker.foliageMass = 200f;
			walker.foliageDensity = 0.9f;
			walker.maxComplexity = 1; // 2 is already giant
			walker.maxHardness = 50;

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
