using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class FrozenFoodExpressEvent() : TwitchEventBase(ID)
	{
		public const string ID = "FrozenFoodExpress";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var startPosition = PosUtil.ClampedMouseWorldPos();

			var go = new GameObject("AETE_FrozenFoodSpawner");
			var spawner = go.AddComponent<ThingSpawner>();
			spawner.transform.position = startPosition;
			spawner.minCount = 10;
			spawner.maxCount = 15;
			spawner.prefabTags = [
				SquirrelConfig.ID,
				HatchConfig.ID,
				PuftConfig.ID,
				PacuConfig.ID,
				PacuConfig.ID,
				PacuConfig.ID,
				PacuConfig.ID,
				PacuConfig.ID
				];

			if (DlcManager.IsExpansion1Active())
				spawner.prefabTags.Add(StaterpillarConfig.ID);

			if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
				spawner.prefabTags.Add(WoodDeerConfig.ID);

			spawner.radius = 4;
			spawner.minDelay = 0.2f;
			spawner.maxDelay = 0.6f;
			spawner.sceneLayer = Grid.SceneLayer.FXFront2;
			spawner.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
			spawner.followCursor = true;
			spawner.volume = 6f;
			spawner.configureSpawnFn += Freeze;
			spawner.Begin((cell, _) => GridUtil.IsCellEmpty(cell));

			ToastManager.InstantiateToastWithPosTarget(STRINGS.AETE_EVENTS.FROZENFOODEXPRESS.TOAST, STRINGS.AETE_EVENTS.FROZENFOODEXPRESS.DESC, startPosition);
			AudioUtil.PlaySound(ModAssets.Sounds.DOORBELL, ModAssets.GetSFXVolume());
		}

		private void Freeze(GameObject go)
		{
			var midasContainer = FUtility.Utils.Spawn(FrozenEntityContainerConfig.ID, go.gameObject);
			midasContainer.GetComponent<MidasEntityContainer>().StoreCritter(go.gameObject, float.PositiveInfinity);
			AudioUtil.PlaySound(ModAssets.Sounds.FREEZE_SOUNDS.GetRandom(), midasContainer.transform.position, FUtility.Utils.GetSFXVolume());
		}
	}
}
