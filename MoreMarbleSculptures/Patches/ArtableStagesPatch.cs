using Database;
using FUtilityArt;
using HarmonyLib;
using static STRINGS.BUILDINGS.PREFABS;

namespace MoreMarbleSculptures.Patches
{
    public class ArtableStagesPatch
    {
        [HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
        public class TargetType_TargetMethod_Patch
        {
            public static void Postfix(ArtableStages __instance)
            {
                ArtHelper.GetDefaultDecors(__instance, MarbleSculptureConfig.ID, out var greatDecor, out var okayDecor, out var uglyDecor);

                AddGreatStatue(__instance, "dragon", greatDecor);
                AddGreatStatue(__instance, "talos", greatDecor);
                AddGreatStatue(__instance, "pacucorn", greatDecor);
                AddGreatStatue(__instance, "smugpip", greatDecor);
                AddGreatStatue(__instance, "panda", greatDecor);
                AddGreatStatue(__instance, "dashmaster", greatDecor);
                AddGreatStatue(__instance, "azura", greatDecor);
                AddGreatStatue(__instance, "poseidon", greatDecor);
                AddGreatStatue(__instance, "lung", greatDecor);
                AddGreatStatue(__instance, "toadstool", greatDecor);

                AddPoorStatue(__instance, "smile", uglyDecor);


                ArtHelper.MoveStages(
                    __instance.GetPrefabStages(MarbleSculptureConfig.ID),
                    Mod.Settings.MoveSculptures,
                    MARBLESCULPTURE.EXCELLENTQUALITYNAME,
                    MARBLESCULPTURE.AVERAGEQUALITYNAME,
                    MARBLESCULPTURE.POORQUALITYNAME,
                    uglyDecor,
                    okayDecor,
                    greatDecor);
            }

            private static void AddGreatStatue(ArtableStages __instance, string id, int decor)
            {
                __instance.Add(id, MARBLESCULPTURE.EXCELLENTQUALITYNAME, "mms_marble_kanim", id, decor, true, Db.Get().ArtableStatuses.Great, MarbleSculptureConfig.ID);
            }

            private static void AddPoorStatue(ArtableStages __instance, string id, int decor)
            {
                __instance.Add(id, MARBLESCULPTURE.POORQUALITYNAME, "mms_marble_kanim", id, decor, true, Db.Get().ArtableStatuses.Ugly, MarbleSculptureConfig.ID);
            }
        }
    }
}
