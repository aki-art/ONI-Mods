using ONITwitchLib;
using ProcGen;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public abstract class PandoraEventBase(float weight, float duration = float.PositiveInfinity) : IWeighted
	{
		protected float elapsed;
		protected PandorasBox box;
		public bool isActive;
		public float duration = duration;
		private float nextSpawnTimer = 0;
		private float nextSpawnTimerTimeOut = 0;
		public bool extraMean;

		public virtual float weight { get; set; } = weight;

		public virtual bool IsFinished { get; private set; }

		public virtual bool Condition() => true;

		public abstract Danger GetDanger();

		public PandoraEventBase ExtraMean()
		{
			this.extraMean = true;
			return this;
		}

		public virtual void Run(PandorasBox box)
		{
			this.box = box;
			isActive = true;
		}

		public void End()
		{
			isActive = false;
			IsFinished = true;
		}

		public virtual void Tick(float dt)
		{
			if (!isActive || box == null)
				return;

			elapsed += dt;
			nextSpawnTimer += dt;

			if (nextSpawnTimer > nextSpawnTimerTimeOut)
			{
				Spawn(dt);
				nextSpawnTimer = 0.0f;
			}

			if (elapsed > duration)
				End();
		}

		public virtual void Spawn(float dt)
		{
		}

		protected void SetSpawnTimer(float time)
		{
			nextSpawnTimerTimeOut = time;
		}
	}
}
