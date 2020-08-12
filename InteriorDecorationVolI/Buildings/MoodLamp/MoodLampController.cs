/*
namespace InteriorDecorationVolI.Buildings.MoodLamp
{
	public class MoodLampController : GameStateMachine<MoodLampController, MoodLampController.Instance>
	{
		[MyCmpReq]
		private readonly MoodLamp moodLamp;
		public State off;
		public State on;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;
			off
				.PlayAnim("variant_1_off")
				.EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
			on
				.PlayAnim("variant_1_on")
				.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null)
				.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true));
		}

		public class Def : BaseDef { }
		 
		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master, Def def) : base(master, def) { }
		}
	}
}
*//*
namespace InteriorDecorationVolI.Buildings.MoodLamp
{
	public class MoodLampController : GameStateMachine<MoodLampController, MoodLampController.Instance>
	{
		[MyCmpReq]
		private readonly MoodLamp moodLamp;
		public State off;
		public State on;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;
			off
				.PlayAnim("variant_1_off")
				.EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
			on
				.PlayAnim("variant_1_on")
				.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null)
				.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true));
		}

		public class Def : BaseDef { }
		 
		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master, Def def) : base(master, def) { }
		}
	}
}
*/