using Klei.AI;

namespace Moonlet.Scripts
{
	public class Moonlet_EnvironmentMonitor : GameStateMachine<Moonlet_EnvironmentMonitor, Moonlet_EnvironmentMonitor.Instance>
	{
		public State checking;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = checking;

			checking
				.Update(UpdateEnvironment, UpdateRate.RENDER_1000ms);
		}

		private void UpdateEnvironment(Instance instance, float dt)
		{
		}

		public new class Instance : GameInstance
		{
			public Effects effects;

			public Instance(IStateMachineTarget master) : base(master)
			{
				effects = GetComponent<Effects>();
			}
		}
	}
}
