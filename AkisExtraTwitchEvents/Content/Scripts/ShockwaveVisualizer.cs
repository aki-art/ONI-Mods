using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class ShockwaveVisualizer : KMonoBehaviour, ISimEveryTick
	{
		public MeshRenderer renderer;
		private float elapsed = 0f;
		private float minMult = 0f;
		private float duration = 0f;

		public void Play(float minMult = 0.164f, float duration = 3f)
		{
			this.minMult = minMult;
			this.duration = duration;

			var go = Instantiate(ModAssets.Prefabs.shockwave);
			go.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
			go.transform.parent = transform;

			renderer = go.GetComponentInChildren<MeshRenderer>();
		}

		public void SimEveryTick(float dt)
		{
			elapsed += dt;

			if (elapsed > duration)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			var t = Mathf.Pow(elapsed / duration, 3f);
			renderer.material.SetFloat("_BlendAlpha", Mathf.Lerp(3f, 0f, elapsed / duration));
			//renderer.material.SetFloat("_AlphaFalloff", Mathf.Clamp(Mathf.Lerp(10f, 0f, elapsed / duration), 0f, 10f));
		}
	}
}