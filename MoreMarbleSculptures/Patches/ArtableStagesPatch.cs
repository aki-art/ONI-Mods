using Database;
using FUtility;
using HarmonyLib;
using static MoreMarbleSculptures.STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE;

namespace MoreMarbleSculptures.Patches
{
	public class ArtableStagesPatch
	{
		[HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_TargetMethod_Patch
		{
			private const string KANIM_PREFIX = "marblesculptures";

			public static void Postfix(ArtableStages __instance)
			{
				ArtableUtil.GetDefaultDecors(
					__instance,
					MarbleSculptureConfig.ID,
					out var greatDecor,
					out var okayDecor,
					out var uglyDecor);

				AddGreatStatue(__instance, DRAGON.NAME, DRAGON.DESCRIPTION, "dragon", greatDecor);
				AddGreatStatue(__instance, TALOS.NAME, TALOS.DESCRIPTION, "talos", greatDecor);
				AddGreatStatue(__instance, PACUCORN.NAME, PACUCORN.DESCRIPTION, "pacucorn", greatDecor);
				AddGreatStatue(__instance, SMUGPIP.NAME, SMUGPIP.DESCRIPTION, "smugpip", greatDecor);
				AddGreatStatue(__instance, PANDA.NAME, PANDA.DESCRIPTION, "panda", greatDecor);
				AddGreatStatue(__instance, DASHMASTER.NAME, DASHMASTER.DESCRIPTION, "dashmaster", greatDecor);
				AddGreatStatue(__instance, AZURA.NAME, AZURA.DESCRIPTION, "azura", greatDecor);
				AddGreatStatue(__instance, POSEIDON.NAME, POSEIDON.DESCRIPTION, "poseidon", greatDecor);
				AddGreatStatue(__instance, LUNG.NAME, LUNG.DESCRIPTION, "lung", greatDecor);
				AddGreatStatue(__instance, TOADSTOOL.NAME, TOADSTOOL.DESCRIPTION, "toadstool", greatDecor);
				AddGreatStatue(__instance, PIGKING.NAME, PIGKING.DESCRIPTION, "pigking", greatDecor);
				AddGreatStatue(__instance, FLOX.NAME, FLOX.DESCRIPTION, "flox", greatDecor);

				AddPoorStatue(__instance, SMILE.NAME, SMILE.DESCRIPTION, "smile", uglyDecor);

				ArtableUtil.MoveStages(
					__instance.GetPrefabStages(MarbleSculptureConfig.ID),
					Mod.Settings.MoveSculptures,
					uglyDecor,
					okayDecor,
					greatDecor);
			}

			private static void AddGreatStatue(ArtableStages __instance, string name, string description, string id, int decor)
			{
				var fullId = ArtableUtil.AddStage(__instance, MarbleSculptureConfig.ID, KANIM_PREFIX, name, description, id, decor, ArtableStatuses.ArtableStatusType.LookingGreat);
				Mod.mySculptureIds.Add(fullId);
			}

			private static void AddPoorStatue(ArtableStages __instance, string name, string description, string id, int decor)
			{
				var fullId = ArtableUtil.AddStage(__instance, MarbleSculptureConfig.ID, KANIM_PREFIX, name, description, id, decor, ArtableStatuses.ArtableStatusType.LookingUgly);
				Mod.mySculptureIds.Add(fullId);
			}
		}
	}
}
