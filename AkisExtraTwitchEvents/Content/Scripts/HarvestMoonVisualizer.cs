using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class HarvestMoonVisualizer : KMonoBehaviour, IRenderEveryTick
	{
		private Vector3 worldOrbitingOrigin;
		[Serialize] private float duration;
		[Serialize] public float elapsed;
		[Serialize] private bool isActive;

		public override void OnSpawn()
		{
			var myWorld = this.GetMyWorld();
			worldOrbitingOrigin = myWorld.WorldOffset + ((Vector2)myWorld.WorldSize / 2.0f);
			transform.position = new Vector3(worldOrbitingOrigin.x, myWorld.WorldSize.y - 10, Grid.GetLayerZ(Grid.SceneLayer.Background));  //new Vector3(myWorld.WorldOffset.X - 200.0f, worldOrbitingOrigin.y, Grid.GetLayerZ(Grid.SceneLayer.Background));

			GetComponent<KBatchedAnimController>().Play("idle");
			//End(true);

			AkisTwitchEvents.Instance.harvestMoonTracker[myWorld.id] = this;
		}

		public void RenderEveryTick(float dt)
		{
			return;

			elapsed += dt;

			var angle = Mathf.Lerp(-Mathf.PI / 0.5f, Mathf.PI / 0.5f, elapsed / duration);
			transform.RotateAround(worldOrbitingOrigin, Vector3.forward, angle);

			if (elapsed > duration)
				End();
		}

		public void Begin(float duration)
		{
			this.duration = duration;
			elapsed = 0;

			if (isActive)
				return;

			isActive = true;
			gameObject.SetActive(true);

			SimAndRenderScheduler.instance.Add(this);
		}

		public void End(bool force = false)
		{
			if (!isActive && !force)
				return;

			isActive = false;
			SimAndRenderScheduler.instance.Remove(this);
			gameObject.SetActive(false);

			AkisTwitchEvents.Instance.Trigger(ModEvents.HarvestMoonSet, this.GetMyWorldId());
		}
	}
}
