using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts.UI
{
	[ExecuteAlways]
	public class SpinnyWheel : KMonoBehaviour
	{
		public float rotatePower = 1440;
		public float stopPower = 300;

		public Rigidbody2D rbody;

		private bool isRotating;
		private float t;

		public Action<int> OnRoll;
		private List<LocText> labels = new List<LocText>();

		public override void OnSpawn()
		{
			base.OnSpawn();
			rbody = GetComponentInChildren<Rigidbody2D>();
		}

		public void Roll()
		{
			if (!isRotating)
			{
				rbody = GetComponentInChildren<Rigidbody2D>();
				Log.Assert("rbody", rbody);

				rbody.AddTorque(rotatePower);
				isRotating = true;
			}
		}

		private void FixedUpdate()
		{
			if (!isRotating)
				return;

			if (rbody.angularVelocity > 0)
			{
				rbody.angularVelocity -= stopPower * (Time.deltaTime * (1f / (SpeedControlScreen.Instance.speed + 1)));
				rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
			}

			if (rbody.angularVelocity == 0 && isRotating)
			{
				t += 1 * Time.deltaTime;
				if (t >= 0.5f)
				{
					GetResult();

					isRotating = false;
					t = 0;
				}
			}
		}

		private void GetResult()
		{
			var rot = rbody.transform.eulerAngles.z + 90;
			var index = (int)(rot / 90) % 4;

			Log.Debug($"rolled: {index} / {rot}");

			//if (TryGetComponent(out Animation animation))
			//	animation.Play();
			//else

			OnRoll?.Invoke(index);

			gameObject.SetActive(false);
		}

		public void SetOption(int index, string label)
		{
			if (labels == null || labels.Count == 0)
				GetComponentsInChildren(labels);

			if (index >= label.Length || index < 0)
				return;

			labels[index].SetText(label);
		}
	}
}
