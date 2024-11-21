using KSerialization;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class RotatableLamp : KMonoBehaviour
	{
		[Serialize] public float angle;
		[Serialize] public bool initialized;

		[MyCmpReq] private MoodLamp moodLamp;
		[MyCmpReq] private Rotatable rotatable;

		private KBatchedAnimController kbac;

		public bool IsActive { get; private set; }

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe(ModEvents.OnMoodlampChanged, OnLampChanged);
			Subscribe(ModEvents.OnLampRefreshedAnimation, OnRefreshAnimation);
			Subscribe((int)GameHashes.OperationalChanged, OnRefreshAnimation);
			Subscribe((int)GameHashes.Rotated, OnRefreshAnimation);
		}


		private void OnRefreshAnimation(object obj)
		{
			if (IsActive)
				UpdateKbac();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out RotatableLamp other) && other.IsActive)
			{
				initialized = true; // skip initialization if setting was somehow copied before the game was unpaused
				SetAngle(other.angle);
			}
		}

		public void SetAngle(float angle)
		{
			this.angle = angle;
			UpdateKbac();
		}

		private void OnLampChanged(object data)
		{
			if (LampVariant.HasTag(data, LampVariants.TAGS.ROTATABLE))
			{
				if (!initialized)
				{
					initialized = true;
					angle = Random.Range(0, 360f);
				}

				kbac = moodLamp.lampKbac;
				UpdateKbac();

				IsActive = true;

				return;
			}

			IsActive = false;
		}

		private void UpdateKbac()
		{
			var a = angle;
			if (rotatable.IsRotated)
			{
				a = Mathf.Abs(360f - a);
			}

			var t = 1f - (a / 360f);

			kbac.SetPositionPercent(t);
			kbac.UpdateAnim(0);
			kbac.SetDirty();
		}
	}
}
