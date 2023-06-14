using Database;
using FUtility;
using HarmonyLib;
using MoreSmallSculptures.FUtilityArt;
using static MoreSmallSculptures.STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE;
using static STRINGS.BUILDINGS.PREFABS;

namespace MoreSmallSculptures.Patches
{
    public class ArtableStagesPatch
    {
        private const string ANIM_FILE = "mss_sculptures_kanim";
        private const string TARGET_DEF = SmallSculptureConfig.ID;

        [HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
        public class TargetType_Ctor_Patch
        {
            public static void Postfix(ArtableStages __instance)
            {
                ArtHelper.GetDefaultDecors(__instance, SculptureConfig.ID, out var greatDecor, out var okayDecor, out var uglyDecor);

                __instance.Add(CreateGreatStage("baby_pip", greatDecor, BABY_PIP.NAME, BABY_PIP.DESCRIPTION));
                __instance.Add(CreateGreatStage("two_baby_pips", greatDecor, BABY_PIP_2.NAME, BABY_PIP_2.DESCRIPTION));
                __instance.Add(CreateGreatStage("baby_beeta", greatDecor, BABY_BEETA.NAME, BABY_BEETA.DESCRIPTION));
                __instance.Add(CreateGreatStage("baby_pincher", greatDecor, BABY_POKESHELL.NAME, BABY_POKESHELL.DESCRIPTION));
                __instance.Add(CreateGreatStage("catcoon", greatDecor, CATCOON.NAME, CATCOON.DESCRIPTION));
                __instance.Add(CreateGreatStage("cat", greatDecor, CAT.NAME, CAT.DESCRIPTION));
                __instance.Add(CreateGreatStage("isaac_suncard", greatDecor, THE_SUN.NAME, THE_SUN.DESCRIPTION));
                __instance.Add(CreateGreatStage("totoro", greatDecor, TOTORO.NAME, TOTORO.DESCRIPTION));
                __instance.Add(CreateGreatStage("chu_totoro", greatDecor, CHU_TOTORO.NAME, CHU_TOTORO.DESCRIPTION));
                __instance.Add(CreateGreatStage("chibi_totoro", greatDecor, CHIBI_TOTORO.NAME, CHIBI_TOTORO.DESCRIPTION));
                __instance.Add(CreateGreatStage("arkay", greatDecor, ARKAY.NAME, ARKAY.DESCRIPTION));
                __instance.Add(CreateGreatStage("happy", greatDecor, HAPPY.NAME, HAPPY.DESCRIPTION));
                __instance.Add(CreateGreatStage("penrose", greatDecor, NOT_PENROSE.NAME, NOT_PENROSE.DESCRIPTION));
                __instance.Add(CreateGreatStage("egg", greatDecor, EGG.NAME, EGG.DESCRIPTION));
                __instance.Add(CreateGreatStage("froggit", greatDecor, FROGGIT.NAME, FROGGIT.DESCRIPTION));
                __instance.Add(CreateGreatStage("rotwood_onion", greatDecor, ROTWOOD_ONION.NAME, ROTWOOD_ONION.DESCRIPTION));

                __instance.Add(CreateOkayStage("duck", okayDecor, DUCK.NAME, DUCK.DESCRIPTION));

                __instance.Add(CreateBadStage("hightaste", uglyDecor, HIGHTASTE.NAME, HIGHTASTE.DESCRIPTION));
                __instance.Add(CreateBadStage("banan", uglyDecor, BANANA.NAME, BANANA.DESCRIPTION));

                ArtHelper.MoveStages(
                    __instance.GetPrefabStages(TARGET_DEF),
                    Mod.Settings.MoveSculptures,
                    uglyDecor,
                    okayDecor,
                    greatDecor);
            }

            private static ArtableStage CreateStage(string stageId, int decor, string name, bool cheer, ArtableStatusItem statusItem, string description)
            {
                var id = $"{TARGET_DEF}_{stageId}";
                Mod.myOverrides.Add(id);

                return new ArtableStage(
                    id,
                    name,
                    description,
                    PermitRarity.Universal,
                    "mss_" + stageId + "_kanim",
                    stageId,
                    decor,
                    cheer,
                    statusItem,
                    TARGET_DEF);
            }

            private static ArtableStage CreateGreatStage(string stageId, int decor, string name, string description)
            {
                return CreateStage(
                    stageId,
                    decor,
                    name,
                    true,
                    Db.Get().ArtableStatuses.LookingGreat,
                    description);
            }

            private static ArtableStage CreateBadStage(string stageId, int decor, string name, string description)
            {
                return CreateStage(
                    stageId,
                    decor,
                    name,
                    false,
                    Db.Get().ArtableStatuses.LookingUgly,
                    description);
            }

            private static ArtableStage CreateOkayStage(string stageId, int decor, string name, string description)
            {
                return CreateStage(
                    stageId,
                    decor,
                    name,
                    false,
                    Db.Get().ArtableStatuses.LookingOkay,
                    description);
            }
        }
    }
}
