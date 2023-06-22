namespace Twitchery.Content.Scripts
{

	public class PolymorphAi : GameStateMachine<PolymorphAi, PolymorphAi.Instance, IStateMachineTarget, PolymorphAi.Def>
	{
		private State dead;
		private State alive;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.ToggleStateMachine(smi => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def()))
				.Enter(CheckIfDead);

			alive
				.TagTransition(GameTags.Dead, dead);
		}

		private void CheckIfDead(Instance smi)
		{
			smi.GoTo(smi.HasTag(GameTags.Dead) ? dead : alive);
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
			}
		}
	}
}
