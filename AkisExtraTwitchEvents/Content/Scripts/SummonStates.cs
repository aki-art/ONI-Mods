namespace Twitchery.Content.Scripts
{
	public class SummonedStates : GameStateMachine<SummonedStates, SummonedStates.Instance, IStateMachineTarget, SummonedStates.Def>
	{
		public State summoning;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = summoning;

			summoning
				.PlayAnim("grooming_pst")
				.OnAnimQueueComplete(null);
		}

		public class Def : BaseDef { }

		public new class Instance : GameInstance
		{
			public static readonly Chore.Precondition IsBeingSummoned = new()
			{
				id = "AkisExtraTwitchEvents_IsBeingSummoned",
				fn = (ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(TTags.summoning)
			};

			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				chore.AddPrecondition(IsBeingSummoned);
			}
		}
	}
}
