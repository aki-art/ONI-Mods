using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_Timer : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public HashedString id;
		[SerializeField] public float time;

		private float _elapsed;
		private bool _running;

		public static void OneShot(Component parent, float time)
		{
			var timer = parent.gameObject.AddComponent<AETE_Timer>();
			timer.time = time;
			timer.Begin();
		}

		public void Begin()
		{
			_elapsed = 0;
			_running = true;
		}

		public void Pause()
		{
			_running = false;
		}

		public void Resume()
		{
			_running = true;
		}

		public void Sim33ms(float dt)
		{
			if (!_running)
				return;

			_elapsed += dt;
			if (_elapsed > time)
				Trigger(ModEvents.TimeOut, id);
		}

	}
}
