using ONITwitchLib.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class CursorRainer : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public ushort elementIdx;
		[SerializeField] public float duration;
		[SerializeField] public float amountPerDroplet;
		[SerializeField] public float temperature;

		private float elapsed;
		private bool isStarted;
		private Vector2 offset = new(-0.5f, 0.5f);
		public void Sim33ms(float dt)
		{
			if (!isStarted)
				return;

			if (elapsed > duration)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			elapsed += dt;

			FallingWater.instance.AddParticle(
				(Vector2)PosUtil.ClampedMouseWorldPos() + offset,
				elementIdx,
				amountPerDroplet,
				temperature,
				byte.MaxValue,
				0,
				skip_decor: true);
		}

		public void StartRaining()
		{
			isStarted = true;
		}
	}
}
