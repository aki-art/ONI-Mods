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
			this.AddTag(TTags.angry);
		}
	}
}
