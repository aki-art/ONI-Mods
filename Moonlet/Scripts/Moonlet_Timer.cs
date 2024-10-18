using KSerialization;
using UnityEngine;
using static Moonlet.Scripts.MoonletComponentHolder;

namespace Moonlet.Scripts
{
	public class Moonlet_Timer : KMonoBehaviour
	{
		[SerializeField] public float seconds;
		[SerializeField] public HashedString hash;
		[SerializeField] public event EntityComponentFn OnTriggerFn;

		[Serialize] public float elapsed;

		void Update()
		{
			elapsed += Time.deltaTime;

			if (elapsed > seconds)
			{
				OnTriggerFn?.Invoke(gameObject);
				elapsed = 0;
			}
		}
	}
}
