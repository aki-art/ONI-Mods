using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class BigBird : KMonoBehaviour
	{
		public static readonly HashedString LAMP_ID = "bigbird";
		public bool isActive;
		[MyCmpReq] private MoodLamp moodLamp;
		[MyCmpReq] private Rotatable rotatable;

		private KBatchedAnimController kbac;
		public Vector3 position;

		public const float PI2 = Mathf.PI * 2;

		public override void OnSpawn()
		{
			base.OnSpawn();

			Subscribe(ModEvents.OnMoodlampChanged, OnLampChanged);

			position = transform.position + new Vector3(0.20f, 0.50f, 0);
			kbac = moodLamp.lampKbac;
		}

		private void OnLampChanged(object data)
		{
			isActive = LampVariant.TryGetData<string>(data, "LampId", out var id) && id == LAMP_ID;
		}

		public void Update()
		{
			if (!isActive)
				return;

			var mousePosition = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
			var vector = mousePosition - position;

			if (rotatable.IsRotated)
				vector.x *= -1;

			var angle = Mathf.Atan2(vector.y, vector.x);

			if (angle < 0)
				angle += PI2;

			var t = angle / PI2;
			t = Mathf.Clamp01(t);

			kbac.SetPositionPercent(t);
		}
	}
}
