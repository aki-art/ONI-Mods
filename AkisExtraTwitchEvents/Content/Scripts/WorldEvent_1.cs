/*using KSerialization;
using System;

namespace Twitchery.Content.Scripts
{
	public abstract class WorldEvent
	{
		[Serialize] public Config config;
		[Serialize] public bool isActive;

		[Serializable]
		public struct Config
		{
			public int worldIdx;
			public int aggression;
			public float elapsed;
			public float totalDuration;
			public int duration;
		}

		public Config Configure(int worldIdx, float duration, int aggression)
		{
			config = new Config()
			{
				worldIdx = worldIdx,
				totalDuration = duration,
				aggression = aggression,
			};

			return config;
		}

		public void Update(float dt)
		{
			config.elapsed += dt;

			if (config.elapsed > config.duration)
				End(false);
		}

		public void Begin(bool instant)
		{
			isActive = true;
			AkisTwitchEvents.Instance.ToggleOverlay(config.worldIdx, OverlayRenderer.SAND_STORM, true, instant);
		}

		public void Restore(Config config)
		{
			this.config = config;
			Begin(true);
		}

		public void OnSerialize()
		{
			AkisTwitchEvents.Instance.sandStormConfig = config;
		}

		public void End(bool instant)
		{
			if (!isActive)
				return;

			isActive = false;
			AkisTwitchEvents.Instance.ToggleOverlay(config.worldIdx, OverlayRenderer.SAND_STORM, true, instant);
		}

		public bool IsRunning() => isActive;

	}
}
*/