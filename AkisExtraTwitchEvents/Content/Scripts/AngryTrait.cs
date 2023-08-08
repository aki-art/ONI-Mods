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

		public void MigrateStrengthStat()
		{
			var attributeLevels = GetComponent<AttributeLevels>();
			var strengthId = Db.Get().Attributes.Strength.Id;
			var level = attributeLevels.GetAttributeLevel(strengthId);
			attributeLevels.SetLevel(Db.Get().Attributes.Strength.Id, level.GetLevel() - 20);
		}

		public void MigrateHealth()
		{
			var health = GetComponent<Health>();
			health.hitPoints += 100;
		}
	}
}
