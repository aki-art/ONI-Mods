using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	public class HarvestMoonEvent() : TwitchEventBase(ID)
	{
		public const string ID = "HarvestMoon";

		private int targetWorldIdx = -1;

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override bool Condition() => CheckTargetWorldIsValid();

		private bool CheckTargetWorldIsValid()
		{
			if (targetWorldIdx != -1 && IsWorldEligible(targetWorldIdx))
				return true;

			var activeWorld = ClusterManager.Instance.activeWorld;
			if (IsWorldEligible(activeWorld.id))
			{
				targetWorldIdx = activeWorld.id;
				return true;
			}

			var startWorld = ClusterManager.Instance.GetStartWorld();
			if (startWorld.id != activeWorld.id && IsWorldEligible(startWorld.id))
			{
				targetWorldIdx = startWorld.id;
				return true;
			}

			foreach (var world in ClusterManager.Instance.WorldContainers)
			{
				if (IsWorldEligible(world.id))
				{
					targetWorldIdx = world.id;
					return true;
				}
			}

			targetWorldIdx = -1;
			return false;
		}

		public override void Run()
		{
			if (CheckTargetWorldIsValid())
				AkisTwitchEvents.Instance.BeginHarvestMoon(targetWorldIdx);
			else
				ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.HARVESTMOON.TOAST, STRINGS.AETE_EVENTS.HARVESTMOON.FAILED);
		}

		private bool IsWorldEligible(int worldIdx)
		{
			var world = ClusterManager.Instance.GetWorld(worldIdx);

			if (world == null)
				return false;

			if (world.IsModuleInterior)
				return false;

			if (!world.IsDupeVisited)
				return false;

			if (Components.LiveMinionIdentities.GetWorldItems(worldIdx).Count <= 0 && !DebugHandler.InstantBuildMode)
				return false;

			return Components.Crops.GetWorldItems(worldIdx).Count > 0;
		}
	}
}
