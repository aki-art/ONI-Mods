namespace DuctTape.Components
{
    internal class DuctTapeConnection : GameStateMachine<DuctTapeConnection, DuctTapeConnection.StatesInstance, IStateMachineTarget, DuctTapeConnection.Def>
	{
		private State unlinked;
		private State linked;

		private BoolParameter isConnected = new BoolParameter();

		public override void InitializeStates(out BaseState defaultState)
		{
			defaultState = unlinked;

			unlinked
				.ParamTransition(isConnected, linked, IsTrue)
				.ToggleStatusItem(ModAssets.StatusItems.notConnected, smi => smi);

			linked
				.ParamTransition(isConnected, unlinked, IsFalse);
		}

		public class Def : BaseDef
		{
			public Tag headBuildingTag;
			public Tag ignoreTag;
		}

		public class StatesInstance : GameInstance
		{
			public StatesInstance(IStateMachineTarget master, Def def) : base(master, def)
			{
			}
		}
	}
}
