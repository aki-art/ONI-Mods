using ONITwitchLib;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;
using static Twitchery.Utils.MiscUtil;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraShrapnelEvent(float weight, float duration) : PandoraEventBase(weight, duration)
	{
		private bool meteorListSanitized = false;
		private WeighedElementOption selectedOption;
		private const float SHRAPNEL_SPEED = 10f;

		private static List<WeighedElementOption> elementOptions = [

			WeighedElementOption.Prefab(IronCometConfig.ID, 5),
			WeighedElementOption.Prefab(GoldCometConfig.ID, 5),
			WeighedElementOption.Prefab(AlgaeCometConfig.ID, 5),
			WeighedElementOption.Prefab(SnowballCometConfig.ID, 5),
			WeighedElementOption.Prefab(SpaceTreeSeedCometConfig.ID, 10),
			WeighedElementOption.Prefab(GassyMooCometConfig.ID, 5),
			WeighedElementOption.Prefab(OxyliteCometConfig.ID, 10),
			WeighedElementOption.Prefab(HardIceCometConfig.ID, 10),
			WeighedElementOption.Prefab(IridiumCometConfig.ID, 5),
			WeighedElementOption.Prefab(LightDustCometConfig.ID, 8),
			WeighedElementOption.Prefab(PhosphoricCometConfig.ID, 8),
			WeighedElementOption.Prefab(FullereneCometConfig.ID, 8),
			WeighedElementOption.Prefab(SatelliteCometConfig.ID, 3),
			WeighedElementOption.Prefab(RockCometConfig.ID, 8),
			WeighedElementOption.Prefab(BleachStoneCometConfig.ID, 10),
			WeighedElementOption.Prefab("Beached_DiamondComet", 8, weight: 3),
			WeighedElementOption.Prefab("Beached_AbyssaliteComet", 8, weight: 3),
			WeighedElementOption.Prefab("Beached_SparklingAquamarineComet", 3, weight: 3),
			WeighedElementOption.Prefab("Beached_SparklingVoidComet", 3, weight: 3),
			WeighedElementOption.Prefab("Beached_SparklingZirconComet", 3, weight: 3),
		];

		public override Danger GetDanger() => Danger.High;

		public override void Run(PandorasBox box)
		{
			base.Run(box);

			if (!meteorListSanitized)
			{
				elementOptions.RemoveAll(option => Assets.TryGetPrefab(option.id) == null);
				meteorListSanitized = true;
			}

			selectedOption = elementOptions.GetWeightedRandom();
			SetSpawnTimer(duration / selectedOption.count);

		}

		public override void Spawn(float dt)
		{
			if (box == null)
				return;

			var go = Util.KInstantiate(Assets.GetPrefab(selectedOption.id), box.transform.position);
			go.SetActive(true);

			var angle = (float)Random.Range(-45, 315) % 360;
			var rads = angle * Mathf.PI / 180f;

			var shrapnel = go.GetComponent<Comet>();
			shrapnel.Velocity = new Vector2(-Mathf.Cos(rads), Mathf.Sin(rads)) * SHRAPNEL_SPEED;

			var kbac = shrapnel.GetComponent<KBatchedAnimController>();
			kbac.Rotation = (float)(-(float)angle) - 90f;
		}
	}
}
