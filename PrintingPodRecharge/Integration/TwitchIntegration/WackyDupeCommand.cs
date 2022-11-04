using Database;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class WackyDupeCommand
    {
        public const string ID = "WackyDupe";

        public static bool Condition()
        {
            return true;
        }

        public static void Run()
        {
            SpawnMinion();
        }
        public static Color GetRandomHairColor()
        {
            return Random.ColorHSV(0, 1, 0.5f, 1f, 0.4f, 1f);
        }

        private static void SpawnMinion()
        {
            var gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
            gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;

            Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);

            var position = Grid.CellToPosCBC(Grid.PosToCell(GameUtil.GetActiveTelepad()), Grid.SceneLayer.Move);
            gameObject.transform.SetLocalPosition(position);
            gameObject.SetActive(true);

            var stats = new MinionStartingStats(false);
            var data = DupeGenHelper.GenerateRandomDupe(stats);
            data.hairColor = GetRandomHairColor(); // more saturated wackier hairs

            DupeGenHelper.AddRandomTraits(stats, 0, 6, DUPLICANTSTATS.GOODTRAITS);
            DupeGenHelper.AddRandomTraits(stats, 0, 6, DUPLICANTSTATS.BADTRAITS);
            DupeGenHelper.AddRandomTraits(stats, 1, 1, DUPLICANTSTATS.NEEDTRAITS);
            DupeGenHelper.AddRandomTraits(stats, 0, 2, DUPLICANTSTATS.GENESHUFFLERTRAITS);

            var disabledChoreGroups = new List<ChoreGroup>();
            foreach(var trait in stats.Traits)
            {
                if(trait.disabledChoreGroups != null && trait.disabledChoreGroups.Length > 0)
                {
                    disabledChoreGroups.AddRange(trait.disabledChoreGroups);
                }
            }

            RegenerateAptitudes(stats);
            RegenerateAttributes(stats, 20);

            stats.Apply(gameObject);
        }

        private static void RegenerateAttributes(MinionStartingStats stats, int maxCost)
        {
            var list = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES);
            var cost = 0;

            foreach (var attribute in list)
            {
                var value = Random.Range(-10, 20);

                if(attribute == Db.Get().Attributes.Athletics.Id)
                {
                    value = Mathf.Min(value, -5);
                }

                if (cost + value > maxCost)
                {
                    value = 0;
                }

                cost += value;

                stats.StartingLevels[attribute] = value;
            }
        }

        private static int RegenerateAptitudes(MinionStartingStats stats)
        {
            stats.skillAptitudes = new Dictionary<SkillGroup, float>();
            var count = Random.Range(1, 6);

            var list = new List<SkillGroup>(Db.Get().SkillGroups.resources);
            list.Shuffle();

            for (int i = 0; i < count; i++)
            {
                stats.skillAptitudes.Add(list[i], DUPLICANTSTATS.APTITUDE_BONUS);
            }

            return count;
        }
    }
}
