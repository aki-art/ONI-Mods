using System.Collections;

namespace DecorPackA.Scripts
{
	public class DecorPackA_SaltTracker : KMonoBehaviour
	{
		[MyCmpReq] private MessStation messStation;
		[MyCmpReq] private KBatchedAnimController kbac;

		public override void OnSpawn()
		{
			base.OnSpawn();
			StartCoroutine(ForceAnimation(this));
			Subscribe(ModEvents.OnSkinChanged, _ => UpdateAnimation());
		}

		private void UpdateAnimation()
		{
			if (messStation.smi.IsInsideState(messStation.smi.sm.salt.salty))
			{

				Log.Debug("playing animation salt");
				kbac.Play("salt");
			}
			else
			{
				Log.Debug("playing animation pff");
				kbac.Play("off");
			}
		}

		private static IEnumerator ForceAnimation(DecorPackA_SaltTracker tracker)
		{
			yield return SequenceUtil.waitForEndOfFrame;

			tracker.UpdateAnimation();
		}
	}
}
