using Klei.AI;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.BookI
{
    public class D8 : SelfImprovement
    {
        public const int MIN_POINTS = 10;
        public const int MAX_BONUS = 5;

        public override bool CanUse(MinionIdentity minionIdentity) => true;

        public override void OnUse(Worker worker)
        {
            var attributeLevels = worker.GetComponent<AttributeLevels>();
            var approximateTotals = 0;
            var offset = 999; // used to raise all stats a minimum of 0

            foreach (var attributeLevel in attributeLevels)
            {
                // find smallest attribute value
                if (attributeLevel.GetLevel() < offset)
                {
                    offset = attributeLevel.GetLevel();
                }

                // get sum of values, offset each with the lowest stat so it starts from 0
                approximateTotals += Mathf.Max(offset, attributeLevel.GetLevel() - offset);
            }
            
            // add a slight bonus
            approximateTotals += Random.Range(0, MAX_BONUS + 1);

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
                attributeLevels.SetLevel(ModDb.dupeSkillIds[i], Mathf.RoundToInt(value));
            }
        }
    }
}
