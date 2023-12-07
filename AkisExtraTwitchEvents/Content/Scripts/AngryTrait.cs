using FUtility;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AngryTrait : KMonoBehaviour
	{
		[MyCmpReq] public KBatchedAnimController kbac;
		[MyCmpReq] public FaceGraph faceGraph;
		[MyCmpReq] public KPrefabID kPrefabId;

		public float smashChance;

		private const float LOW_TRESHOLD = 0.2f;

		public override void OnSpawn()
		{
			base.OnSpawn();
			kbac.animScale *= 1.3f;
			faceGraph.AddExpression(TExpressions.hulk);

			Log.Debug("DANGER: " + AkisTwitchEvents.maxDanger);
			if (AkisTwitchEvents.maxDanger > ONITwitchLib.Danger.None)
				this.AddTag(TTags.angry);

			var stress = Db.Get().Amounts.Stress.Lookup(gameObject);
			stress.OnDelta += OnStressChanged;
		}

		private void OnStressChanged(float value)
		{
			if (value > LOW_TRESHOLD)
			{
				if (Mathf.Approximately(smashChance, 0))
					kPrefabId.AddTag(TTags.angry);

				smashChance = value * 0.001f;
			}
			else
			{
				if (smashChance > 0)
					kPrefabId.RemoveTag(TTags.angry);

				smashChance = 0;
			}
		}
	}
}
