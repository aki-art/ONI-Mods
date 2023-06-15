using FUtility;
using Klei.AI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.BookI
{
	public class D8 : SelfImprovement
    {
        public const int MIN_POINTS = 10;
        public const int MIN_BONUS = 4;
        public const int MAX_BONUS = 7;

        public override bool CanUse(MinionIdentity minionIdentity) => true;

        public override void OnUse(Worker worker)
        {
            var values = new Dictionary<string, AttributeChange>();
            var attributeLevels = worker.GetComponent<AttributeLevels>();
            var approximateTotals = 0;
            var offset = 999; // used to raise all stats a minimum of 0
            var actualTotals = 0;
            var newTotals = 0;

            foreach (var attributeLevel in attributeLevels)
            {
                if (!ModDb.dupeSkillIds.Contains(attributeLevel.attribute.Attribute.Id))
                    continue;

                values.Add(attributeLevel.attribute.Attribute.Id, new AttributeChange()
                {
                    original = attributeLevel.GetLevel()
                });

                // find smallest attribute value
                if (attributeLevel.GetLevel() < offset)
                    offset = attributeLevel.GetLevel();

                // get sum of values, offset each with the lowest stat so it starts from 0
                approximateTotals += Mathf.Max(offset, attributeLevel.GetLevel() - offset);
                actualTotals += attributeLevel.GetLevel();
            }
            
            // add a slight bonus
            approximateTotals += Random.Range(MIN_BONUS, MAX_BONUS + 1);

            // clamp to minimum points to grant to a dupe after rerolling. just to be nice.
            approximateTotals = Mathf.Min(MIN_POINTS, approximateTotals);

            // create a matching length array of random values, 0-1
            var sampler = new float[ModDb.dupeSkillIds.Count];
            var samplerSum = 0f;

            for (int i = 0; i < sampler.Length; i++)
            {
                sampler[i] = Random.value;
                samplerSum += sampler[i];
            }

            // use the rolled 0-1 random values to distribute skills that mostly sum to approximateTotals
            // (it is approximate due to rounding to int)
            for (var i = 0; i < ModDb.dupeSkillIds.Count; i++)
            {
                var value = sampler[i] / samplerSum * approximateTotals;
                value += offset; // offset back up
                var level = Mathf.RoundToInt(value);
                var skillId = ModDb.dupeSkillIds[i];
                newTotals += level;

                attributeLevels.SetLevel(skillId, level);

				if (values.TryGetValue(skillId, out var change))
				{
                    Log.Debuglog($"rolled: {skillId} {level}");
					change.rolled = level;
				}
			}

            ShowDialog(values, worker.GetProperName(), actualTotals, newTotals);
        }

        private class AttributeChange
        {
            public int original;
            public int rolled;
        }

        private void ShowDialog(Dictionary<string, AttributeChange> data, string dupeName, int totalBefore, int totalAfter)
        {
            var message = new StringBuilder();
            var attributes = Db.Get().Attributes;

            foreach(var item in data)
            {
                var from = item.Value.original;
                var to = item.Value.rolled;
                var attribute = attributes.TryGet(item.Key);

                if(attribute == null)
				{
                    Log.Debuglog("attribute is null " + item.Key);
					continue;
				}

				var line = $"{attribute.Name} {item.Value.original} → {item.Value.rolled}";

                var diff = to - from;

                if (diff < 0)
                    line += $" (<color=#ce4028>{diff}</color>)";
                else if (diff > 0)
                    line += $" (<color=#55c04a>{diff}</color>)";

                message.AppendLine(line);
            }

            message.AppendLine();
            message.AppendLine($"<b>Total: </b> {totalBefore} → {totalAfter}");

            var prefab = ScreenPrefabs.Instance.InfoDialogScreen.gameObject;
            var parent = GameScreenManager.Instance.ssOverlayCanvas.gameObject;
            var infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(prefab, parent);

            infoDialogScreen
                .SetHeader($"{dupeName} changes")
                .AddPlainText(message.ToString())
                .AddDefaultOK();
        }
    }
}
