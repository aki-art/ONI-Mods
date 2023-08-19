using FUtility;
using ImGuiNET;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class HomingTest : KMonoBehaviour, IImguiDebug
	{
		[MyCmpGet] private Rigidbody2D rigidBody;
		[MyCmpGet] private KBatchedAnimController kbac;

		public static float speed = 250f;
		public static float maximumSpeed = 40f;
		public static float sliding = 0.02f;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			rigidBody.bodyType = RigidbodyType2D.Dynamic;
			rigidBody.simulated = true;
			rigidBody.useAutoMass = false;
			rigidBody.mass = 1.0f;
			rigidBody.angularDrag = 0.05f;
			rigidBody.drag = 0;
			rigidBody.gravityScale = 0;
			rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
			rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;

			kbac.SuspendUpdates(true);
		}

		private void FixedUpdate()
		{
			var target = CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos());
			Vector2 force = target - transform.position;
			force.Normalize();
			force *= speed;
			rigidBody.AddForce(force);
			rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, maximumSpeed);
			transform.position += (Vector3)force * sliding * Time.fixedDeltaTime;

			kbac.UpdateAnim(Time.fixedDeltaTime);
		}

		public void OnImgui()
		{
			ImGui.DragFloat("Speed", ref speed);
			ImGui.DragFloat("Max Speed", ref maximumSpeed);
			ImGui.DragFloat("sliding", ref sliding);
		}
	}
}
