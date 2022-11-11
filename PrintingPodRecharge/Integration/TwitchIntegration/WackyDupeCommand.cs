using Database;
using System.Collections.Generic;
using System.Security.Principal;
using TUNING;
using UnityEngine;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

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

            if (!Mod.IsMeepHere || Mod.Settings.ColoredMeeps)
            {
                data.hairColor = GetRandomHairColor(); // more saturated wackier hairs
            }

            DupeGenHelper.Wackify(stats);

            stats.Apply(gameObject);

            ToastHelper.ToastToTarget("Spawning Duplicant", $"{gameObject.GetProperName()} has been brought into the world!", gameObject);
        }

    }
}
