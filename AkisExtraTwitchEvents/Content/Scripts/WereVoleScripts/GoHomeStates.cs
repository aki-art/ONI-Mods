namespace Twitchery.Content.Scripts.WereVoleScripts
{
	public class GoHomeStates : GameStateMachine<GoHomeStates, GoHomeStates.Instance, IStateMachineTarget, GoHomeStates.Def>
	{
		public State moving;
		public State foundSafety;
		public State failed;
		public State behaviourcomplete;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = moving;

			moving
				.MoveTo(GetTargetCell, foundSafety, behaviourcomplete, true)
				.ToggleStatusItem(
					STRINGS.CREATURES.STATUSITEMS.AKIS_EXTRATWITCHEVENTS_GOINGHOME.NAME,
					STRINGS.CREATURES.STATUSITEMS.AKIS_EXTRATWITCHEVENTS_GOINGHOME.TOOLTIP,
					category: Db.Get().StatusItemCategories.Main);

			failed
				.TriggerOnEnter(ModEvents.FailedToFindSafety)
				.GoTo(behaviourcomplete);

			foundSafety
				.TriggerOnEnter(ModEvents.FoundSafety)
				.GoTo(behaviourcomplete);

			behaviourcomplete
				.BehaviourComplete(TTags.returningHome);
		}

		private static int GetTargetCell(Instance smi) => smi.GetSMI<WereVoleContainer.SMInstance>().targetCell;

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, TTags.returningHome);
			}
		}
	}
}