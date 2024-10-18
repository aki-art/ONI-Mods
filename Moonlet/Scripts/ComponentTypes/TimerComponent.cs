using Moonlet.Templates;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class TimerComponent : BaseComponent
	{
		public TimerData Data { get; private set; }

		public class TimerData
		{
			public FloatNumber seconds;
			public string id;
		}

		public override void Apply(GameObject prefab)
		{
			Data.id ??= $"{Data.seconds}_second_timer";

			var timer = prefab.AddComponent<Moonlet_Timer>();
			timer.seconds = Data.seconds;
			timer.hash = Data.id;
		}
	}
}
