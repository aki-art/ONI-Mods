using ONITwitchLib;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;
using static Twitchery.Utils.MiscUtil;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraRewardEvent(float weight, float duration) : PandoraEventBase(weight, duration)
	{
		private List<WeighedElementOption> options =
		[
			WeighedElementOption.Prefab(SimHashes.Iron.CreateTag().ToString(),10, mass: 10.0f),
			WeighedElementOption.Prefab(SimHashes.Steel.CreateTag().ToString(),10, mass: 10.0f),
			WeighedElementOption.Prefab(SimHashes.Gold.CreateTag().ToString(),10, weight = 10.0f, mass: 10.0f),
			WeighedElementOption.Prefab(SimHashes.Tungsten.CreateTag().ToString(),10, mass: 10.0f),
			WeighedElementOption.Prefab(SimHashes.Diamond.CreateTag().ToString(),20, weight = 6.0f, mass: 5.0f),
			WeighedElementOption.Prefab(SimHashes.Granite.CreateTag().ToString(),10, mass: 20.0f),
			WeighedElementOption.Prefab(SimHashes.OxyRock.CreateTag().ToString(),10, mass: 5.0f),
			WeighedElementOption.Prefab(SimHashes.Iridium.CreateTag().ToString(),10, mass: 10.0f),
			WeighedElementOption.Prefab("Beached_Zirconium",10, mass: 10.0f),
			WeighedElementOption.Prefab("Beached_Pearl",10, mass: 10.0f)
		];

		private List<WeighedElementOption> selectedOptions;

		public override Danger GetDanger() => Danger.None;

		public override void Run(PandorasBox box)
		{
			options.RemoveAll(o => Assets.TryGetPrefab(o.id) == null);

			base.Run(box);

			selectedOptions = [];
			for (var i = 0; i < 3; i++)
			{
				selectedOptions.Add(options.GetWeightedRandom());
			}

			SetSpawnTimer(duration / 10.0f);
		}

		public override void Spawn(float dt)
		{
			base.Spawn(dt);

			if (box == null)
				return;

			foreach (var option in selectedOptions)
			{
				for (var i = 0; i < option.count; i++)
				{
					var reward = FUtility.Utils.Spawn(option.id, (box.transform.position + new Vector3(0, 0.5f)) with
					{
						z = Grid.GetLayerZ(Grid.SceneLayer.Move)
					});

					reward.GetComponent<PrimaryElement>().Mass = option.mass;

					FUtility.Utils.YeetRandomly(reward, true, 3, 6, false);
				}
			}
		}
	}
}
