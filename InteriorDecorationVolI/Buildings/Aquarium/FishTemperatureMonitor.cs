/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    class FishTemperatureMonitor : StateMachineComponent<FishTemperatureMonitor.SMInstance>
	{
		[MyCmpReq]
		private PrimaryElement primaryElement;
		protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

		public class States : GameStateMachine<States, SMInstance, FishTemperatureMonitor>
		{
			public State normal;
			public State overheating;
			public State boiling;
			public State freezing;
			public State frozen;

			public FloatParameter internalTemp;
			public BoolParameter hasFish;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = normal;
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, FishTemperatureMonitor, object>.GameInstance
		{
			public SMInstance(FishTemperatureMonitor master) : base(master) { }
		}
	}
}
*/