using Klei.AI;

namespace Moonlet.Scripts
{
	public class Moonlet_ExposureMonitor : GameStateMachine<Moonlet_ExposureMonitor, Moonlet_ExposureMonitor.Instance, IStateMachineTarget, Moonlet_ExposureMonitor.Def>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.Update(UpdateStatus, UpdateRate.SIM_1000ms);
		}

		private void UpdateStatus(Instance smi, float dt)
		{
			if (smi == null || smi.master == null)
				return;

			var cell = Grid.PosToCell(smi);
			var element = Grid.Element[cell];

			if (smi.effects == null)
				smi.effects = smi.master.GetComponent<Effects>();


			if (!Moonlet_Mod.stepOnEffects.TryGetValue(element.id, out var effects) || effects == null)
				return;

			if (element != null
				&& element.IsGas
				&& effects.SubmergedIn != null
				&& smi.effects != null)
				smi.effects.Add(effects.SubmergedIn.Id, true);
		}

		public class Def : BaseDef
		{
		}

		public new class Instance(IStateMachineTarget master) : GameInstance(master)
		{
			public Effects effects = master.GetComponent<Effects>();
		}
	}
}
