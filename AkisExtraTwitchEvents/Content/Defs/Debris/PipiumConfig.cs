using ProcGen;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
	public class PipiumConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.Pipium;

		public static List<PipOption> options = [
			new PipOption(SquirrelConfig.ID, 3.0f),
			new PipOption(BabySquirrelConfig.ID, 1.0f),
			new PipOption(SquirrelHugConfig.ID, 1.0f),
			new PipOption(SquirrelConfig.ID, 0.2f),
		];

		public struct PipOption(string id, float weight = 1.0f) : IWeighted
		{
			public string id = id;
			public float weight { get; set; } = weight;
		}

		public GameObject CreatePrefab()
		{
			if (Mod.isBeachedHere)
			{
				options.Add(new PipOption("Beached_MerPip", 2.0f));
				options.Add(new PipOption("Beached_BabyMerpip", 0.5f));
			}

			if (Mod.isPipMorphsHere)
			{
				options.Add(new PipOption("SquirrelAutumn", 2f));
				options.Add(new PipOption("SquirrelAutumnBaby", 0.5f));
				options.Add(new PipOption("SquirrelSpring", 2f));
				options.Add(new PipOption("SquirrelSpringBaby", 0.5f));
				options.Add(new PipOption("SquirrelWinter", 2f));
				options.Add(new PipOption("SquirrelWinterBaby", 0.5f));
			}

			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);
			prefab.GetComponent<KPrefabID>().prefabSpawnFn += go =>
			{
				var mass = go.GetComponent<PrimaryElement>().Mass;

				var count = Mathf.CeilToInt(mass);
				count = Mathf.Min(count, 10); // prevent game-crashing spam

				for (var i = 0; i < count; i++)
				{
					var prefabId = options.GetWeightedRandom().id;
					var pip = FUtility.Utils.Spawn(prefabId, go);

					FUtility.Utils.YeetRandomly(pip, true, 1, 2, false);

					Util.KDestroyGameObject(go);
				}
			};

			return prefab;
		}
	}
}
