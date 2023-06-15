using FUtility;
using Klei.AI;

namespace PrintingPodRecharge.Content.Items.BookI
{
	public class BookVolumeII : SelfImprovement
	{
		public override bool CanUse(MinionIdentity minionIdentity)
		{
			return true;
		}

		private AttributeInstance GetLowestAttribute(AttributeLevels attributeLevels)
		{
			if (attributeLevels == null)
			{
				return null;
			}

			var attributes = attributeLevels.GetAttributes();

			AttributeInstance minLevel = null;
			var minLevelValue = 9999f;

			foreach (var id in ModDb.dupeSkillIds)
			{
				var attribute = attributes.Get(id);
				if (attribute == null)
				{
					continue;
				}

				if (minLevel == null)
				{
					minLevel = attribute;
					minLevelValue = attribute.GetTotalValue();
				}
				else
				{
					var value = attribute.GetTotalValue();
					if (attribute.GetTotalValue() < minLevelValue)
					{
						minLevel = attribute;
						minLevelValue = value;
					}
				}
			}

			return minLevel;
		}

		public override void OnUse(Worker worker)
		{
			var attributeLevels = worker.GetComponent<AttributeLevels>();
			var minLevel = GetLowestAttribute(attributeLevels);

			if (minLevel != null)
			{
				Log.Debuglog("min level is " + minLevel.modifier.Id);
				var value = UnityEngine.Random.Range(5, 7);
				var attribute = attributeLevels.GetAttributeLevel(minLevel.modifier.Id);
				if (attribute != null)
				{
					attributeLevels.SetLevel(attribute.attribute.Id, attribute.level + value);
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, $"{value} {minLevel.modifier.Name}", worker.transform, 2f);
				}
			}
		}

		public override string GetStatusString(IAssignableIdentity minionIdentity)
		{
			GetMinionIdentity(assignee, out var identity, out var storedIdentity);
			AttributeLevels attributeLevels = null;

			if (identity != null)
				attributeLevels = identity.GetComponent<AttributeLevels>();
			else if (storedIdentity != null)
				attributeLevels = storedIdentity.GetComponent<AttributeLevels>();

			var lowest = GetLowestAttribute(attributeLevels);

			return $"{minionIdentity.GetProperName()} ({lowest.modifier.Name}: {lowest.GetTotalDisplayValue()})";
		}
	}
}
