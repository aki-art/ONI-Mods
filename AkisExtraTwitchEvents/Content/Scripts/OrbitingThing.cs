using UnityEngine;

namespace Twitchery.Content.Scripts
{
	class OrbitingThing : KMonoBehaviour
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[SerializeField] public Vector2 orbitSpeedModifier;
		[SerializeField] public float erratic;
		[SerializeField] public float maxSpeed;
		[SerializeField] public float damp;
		[SerializeField] public float speed;
		[SerializeField] public Vector3 trackerOffset;

		private Vector3 velocity;
		private float targetSpeed = 1f;
		private float currentSpeed = 1f;

		private Vector3 orbitSpeed;
		private Vector3 targetOrbitSpeed;

		private static readonly Vector3 center = new(0, 0, Grid.GetLayerZ(Grid.SceneLayer.FXFront2));

		private bool started;

		public Vector3 GetTrackerPosition() => kbac.PositionIncludingOffset + trackerOffset;

		public OrbitingThing()
		{
			orbitSpeedModifier = new Vector2(1f, 1.5f);
			erratic = 0.005f;
			maxSpeed = 8f;
			damp = 0.94f;
			speed = 25f;
		}

		public void Begin()
		{
			orbitSpeed = new(GetRandomOrbitModifier(), GetRandomOrbitModifier(), GetRandomOrbitModifier());
			kbac.Offset = Vector3.zero;
			velocity = Vector3.zero;
			started = true;
		}

		private float GetRandomOrbitModifier()
		{
			float num = Random.value;
			num += 0.33f;
			num *= Random.value > 0.5f ? -1 : 1;
			num *= 0.14f;

			return num;
		}

#pragma warning disable IDE0051
		private void FixedUpdate()
		{
			if (!started)
				return;

			if (Mathf.Approximately(currentSpeed, targetSpeed))
			{
				targetSpeed = Random.Range(orbitSpeedModifier.x, orbitSpeedModifier.y);
			}

			currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, erratic);

			if (Mathf.Approximately(Vector3.Distance(targetOrbitSpeed, orbitSpeed), 0))
			{
				targetOrbitSpeed = new(GetRandomOrbitModifier(), GetRandomOrbitModifier(), GetRandomOrbitModifier());
			}

			orbitSpeed = Vector3.MoveTowards(orbitSpeed, targetOrbitSpeed, erratic);

			var vec = kbac.Offset - center;
			vec = vec.normalized;
			vec = Vector3.Cross(vec, orbitSpeed);
			vec *= currentSpeed;

			velocity += vec;
			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
			velocity *= damp;

			var newOffset = speed * Time.fixedDeltaTime * velocity;

			var motionX = newOffset.x - kbac.Offset.x;
			if (motionX != 0)
				kbac.FlipX = motionX < 0;

			kbac.Offset = newOffset;


			kbac.SetDirty();
			kbac.UpdateAnim(0);
		}
#pragma warning restore IDE0051
	}
}
