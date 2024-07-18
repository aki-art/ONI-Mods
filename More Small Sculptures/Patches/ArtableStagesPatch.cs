using Database;
using FUtility;
using HarmonyLib;
using static MoreSmallSculptures.STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE;

namespace MoreSmallSculptures.Patches
{
	public class ArtableStagesPatch
	{
		private const string TARGET_DEF = SmallSculptureConfig.ID;
		private const string KANIM_PREFIX = "mss";

		[HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_Ctor_Patch
		{
			public static void Postfix(ArtableStages __instance)
			{
				ArtableUtil.GetDefaultDecors(__instance, SculptureConfig.ID, out var greatDecor, out var okayDecor, out var uglyDecor);

				AddGreatStatue(__instance, "baby_pip", greatDecor, BABY_PIP.NAME, BABY_PIP.DESCRIPTION);
				AddGreatStatue(__instance, "two_baby_pips", greatDecor, BABY_PIP_2.NAME, BABY_PIP_2.DESCRIPTION);
				AddGreatStatue(__instance, "baby_beeta", greatDecor, BABY_BEETA.NAME, BABY_BEETA.DESCRIPTION);
				AddGreatStatue(__instance, "baby_pincher", greatDecor, BABY_POKESHELL.NAME, BABY_POKESHELL.DESCRIPTION);
				AddGreatStatue(__instance, "catcoon", greatDecor, CATCOON.NAME, CATCOON.DESCRIPTION);
				AddGreatStatue(__instance, "cat", greatDecor, CAT.NAME, CAT.DESCRIPTION);
				AddGreatStatue(__instance, "isaac_suncard", greatDecor, THE_SUN.NAME, THE_SUN.DESCRIPTION);
				AddGreatStatue(__instance, "totoro", greatDecor, TOTORO.NAME, TOTORO.DESCRIPTION);
				AddGreatStatue(__instance, "chu_totoro", greatDecor, CHU_TOTORO.NAME, CHU_TOTORO.DESCRIPTION);
				AddGreatStatue(__instance, "chibi_totoro", greatDecor, CHIBI_TOTORO.NAME, CHIBI_TOTORO.DESCRIPTION);
				AddGreatStatue(__instance, "arkay", greatDecor, ARKAY.NAME, ARKAY.DESCRIPTION);
				AddGreatStatue(__instance, "happy", greatDecor, HAPPY.NAME, HAPPY.DESCRIPTION);
				AddGreatStatue(__instance, "penrose", greatDecor, NOT_PENROSE.NAME, NOT_PENROSE.DESCRIPTION);
				AddGreatStatue(__instance, "egg", greatDecor, EGG.NAME, EGG.DESCRIPTION);
				AddGreatStatue(__instance, "froggit", greatDecor, FROGGIT.NAME, FROGGIT.DESCRIPTION);
				AddGreatStatue(__instance, "rotwood_onion", greatDecor, ROTWOOD_ONION.NAME, ROTWOOD_ONION.DESCRIPTION);
				AddGreatStatue(__instance, "spigot_seal", greatDecor, SPIGOT_SEAL.NAME, SPIGOT_SEAL.DESCRIPTION);

				AddMedStatue(__instance, "duck", okayDecor, DUCK.NAME, DUCK.DESCRIPTION);

				AddPoorStatue(__instance, "hightaste", uglyDecor, HIGHTASTE.NAME, HIGHTASTE.DESCRIPTION);
				AddPoorStatue(__instance, "banan", uglyDecor, BANANA.NAME, BANANA.DESCRIPTION);

				ArtableUtil.MoveStages(
					__instance.GetPrefabStages(TARGET_DEF),
					Mod.Settings.MoveSculptures,
					uglyDecor,
					okayDecor,
					greatDecor);
			}

			private static void AddGreatStatue(ArtableStages __instance, string id, int decor, string name, string description)
			{
				var fullId = ArtableUtil.AddStage(__instance, TARGET_DEF, KANIM_PREFIX, name, description, id, decor, ArtableStatuses.ArtableStatusType.LookingGreat);
				Mod.mySculptureIds.Add(fullId);
			}

			private static void AddMedStatue(ArtableStages __instance, string id, int decor, string name, string description)
			{
				var fullId = ArtableUtil.AddStage(__instance, TARGET_DEF, KANIM_PREFIX, name, description, id, decor, ArtableStatuses.ArtableStatusType.LookingOkay);
				Mod.mySculptureIds.Add(fullId);
			}

			private static void AddPoorStatue(ArtableStages __instance, string id, int decor, string name, string description)
			{
				var fullId = ArtableUtil.AddStage(__instance, TARGET_DEF, KANIM_PREFIX, name, description, id, decor, ArtableStatuses.ArtableStatusType.LookingUgly);
				Mod.mySculptureIds.Add(fullId);
			}
		}
	}
}
