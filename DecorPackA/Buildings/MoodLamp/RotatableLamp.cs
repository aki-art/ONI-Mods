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

		private KBatchedAnimController kbac;

		public bool IsActive { get; private set; }

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(ModEvents.OnMoodlampChanged, OnLampChanged);
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
			if(LampVariant.HasTag(data, LampVariants.TAGS.ROTATABLE))
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
			kbac.SetPositionPercent(1f - (angle / 360f));
			kbac.UpdateAnim(0);
			kbac.SetDirty();
		}
	}
}
