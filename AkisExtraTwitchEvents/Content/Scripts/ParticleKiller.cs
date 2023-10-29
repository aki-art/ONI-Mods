using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class ParticleKiller : MonoBehaviour
	{
		private ParticleSystem particleSystem;
		private readonly List<ParticleSystem.Particle> exit = new();

		private void Start()
		{
			particleSystem = GetComponent<ParticleSystem>();
		}

		void OnParticleTrigger()
		{
			if (particleSystem == null)
				particleSystem = GetComponent<ParticleSystem>();

			int numExit = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

			for (int i = 0; i < numExit; i++)
			{
				ParticleSystem.Particle p = exit[i];
				p.remainingLifetime = 0.15f;
				exit[i] = p;
			}

			particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
		}
	}
}
