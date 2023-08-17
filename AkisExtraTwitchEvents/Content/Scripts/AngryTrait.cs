using FUtility;
using Klei.AI;
using KSerialization;

namespace Twitchery.Content.Scripts
{
	public class AngryTrait : KMonoBehaviour
	{
		[MyCmpReq] public KBatchedAnimController kbac;
		[MyCmpReq] public FaceGraph faceGraph;

		public override void OnSpawn()
		{
			base.OnSpawn();
			kbac.animScale *= 1.3f;
			faceGraph.AddExpression(TExpressions.hulk);

			if (AkisTwitchEvents.maxDanger > ONITwitchLib.Danger.None)
				this.AddTag(TTags.angry);
		}
	}
}
