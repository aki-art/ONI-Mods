using STRINGS;
using System;

namespace AETNTweaks.Buildings.PyrositePylon
{
	public class NotSoMassiveHeatSink : StateMachineComponent<NotSoMassiveHeatSink.StatesInstance>
	{
		[MyCmpReq]
		private Operational operational;

		[MyCmpReq]
		private ElementConverter elementConverter;

		StatusItem statusItem;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
			statusItem = new StatusItem(
						id: "AwaitingFuel",
						name: BUILDING.STATUSITEMS.AWAITINGFUEL.NAME,
						BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP,
						"",
						StatusItem.IconType.Exclamation,
						NotificationType.BadMinor,
						false,
						OverlayModes.None.ID);

			statusItem.resolveStringCallback = AwaitingFuelResolveString;
		}

		private string AwaitingFuelResolveString(string str, object obj)
		{
			var elementConverter = ((StatesInstance)obj).master.elementConverter;
			var consumedElementTag = elementConverter.consumedElements[0].tag.ProperName();

			var formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].massConsumptionRate, GameUtil.TimeSlice.PerSecond);
			str = string.Format(str, consumedElementTag, formattedMass);

			return str;
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class States : GameStateMachine<States, StatesInstance, NotSoMassiveHeatSink>
		{
			public State disabled;
			public State idle;
			public State active;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = disabled;

				disabled
					.EventTransition(GameHashes.OperationalChanged, idle, smi => smi.master.operational.IsOperational);

				idle
					.EventTransition(GameHashes.OperationalChanged, disabled, smi => !smi.master.operational.IsOperational)
					.ToggleStatusItem(smi => smi.master.statusItem)
					.EventTransition(GameHashes.OnStorageChange, active, smi => smi.master.elementConverter.HasEnoughMassToStartConverting());

				active
					.EventTransition(GameHashes.OperationalChanged, disabled, smi => !smi.master.operational.IsOperational)
					.EventTransition(GameHashes.OnStorageChange, idle, smi => !smi.master.elementConverter.HasEnoughMassToStartConverting())
					.Enter(smi => smi.master.operational.SetActive(true))
					.Exit(smi => smi.master.operational.SetActive(false));
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, NotSoMassiveHeatSink, object>.GameInstance
		{
			public StatesInstance(NotSoMassiveHeatSink master) : base(master)
			{
			}
		}
	}
}