using KSerialization;

namespace DecorPackB.Content.Scripts
{
	public class RotatableLamp : KMonoBehaviour
	{
		[MyCmpReq] private Light2D light;
		[Serialize] public float angle;

		public override void OnSpawn()
		{
			base.OnSpawn();
			UpdateAngle();
		}

		void Update()
		{
			SetAngle(angle + 0.01f);
		}

		public void SetAngle(float angle)
		{
			this.angle = angle;
			UpdateAngle();
		}

		private void UpdateAngle()
		{
			light.Angle = angle;
		}
	}
}
