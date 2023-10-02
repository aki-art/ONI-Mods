using ONITwitchLib;
using SpookyPumpkinSO.Content.Cmps;
using SpookyPumpkinSO.Content.TwitchEventsContent;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public class PiptergeistEvent() : EventBase(ID)
	{
		public const string ID = "Piptergeist";

		public override Danger GetDanger() => Danger.Small;

		public HashSet<string> allowedBuildingIds = new()
		{
			StorageLockerConfig.ID,
			RefrigeratorConfig.ID,
			StorageLockerSmartConfig.ID,
			RationBoxConfig.ID,
			"Beached_MiniFridge",
			EspressoMachineConfig.ID,
		};

		public override int GetNiceness() => Intent.EVIL;
		public override void Run()
		{
			foreach (var building in Components.BuildingCompletes.items)
			{
				if (allowedBuildingIds.Contains(building.PrefabID().ToString()) && building.TryGetComponent(out Storage storage))
				{
					if (building.HasTag(GameTags.Creatures.ReservedByCreature))
						continue;

					if (storage.IsEmpty())
						continue;

					var piptergeist = FUtility.Utils.Spawn(PiptergeistConfig.ID, building.gameObject);
					piptergeist.GetComponent<Piptergeist>().smi.SetStorage(storage);
				}
			}

			ToastManager.InstantiateToast(STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.PIPTERGEIST.TOAST, STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.PIPTERGEIST.TOAST_BODY);
		}
	}
}
