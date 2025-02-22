using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class BrackeneRainEvent() : TwitchEventBase(ID)
	{
		public const string ID = "BrackeneRain";

		public override bool Condition() => DiscoveredResources.Instance.IsDiscovered(SimHashes.Milk.CreateTag());

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.Small;

		public override void Run()
		{
			var go = new GameObject("brackene cloud spawner");

			var position = Grid.CellToPos(GridUtil.FindCellWithCavityClearance(PosUtil.ClampedMouseCell())) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
			go.transform.position = position;

			var spawner = go.AddComponent<MilkRainSpawner>();
			spawner.count = 3;
			spawner.appearDuration = 3f;
			spawner.duration = 60f;
			spawner.fadeOutDuration = 4f;
			spawner.animFile = "gassy_moo_kanim";
			spawner.animation = "idle_loop";
			spawner.liquid = SimHashes.Milk;
			spawner.radius = 4f;
			spawner.totalLiquidSpawned = 5000f;
			spawner.temperature = 310f;
			spawner.gas = SimHashes.Methane;
			spawner.gasPerTile = 0.5f;
			spawner.finalSpeed = 400f;

			go.SetActive(true);
			spawner.StartRaining();

			ToastManager.InstantiateToastWithPosTarget(
				STRINGS.AETE_EVENTS.BRACKENERAIN.TOAST,
				STRINGS.AETE_EVENTS.BRACKENERAIN.DESC,
				position);
		}
	}
}
