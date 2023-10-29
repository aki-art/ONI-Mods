using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class RotateSlowly : MonoBehaviour
	{
		public float rotationSpeed;

		void Update()
		{
			transform.Rotate(Vector3.forward, rotationSpeed);
		}
	}
}
