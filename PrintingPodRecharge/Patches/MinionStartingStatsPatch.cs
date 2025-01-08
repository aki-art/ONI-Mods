using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using TUNING;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PrintingPodRecharge.Patches
{
	public class MinionStartingStatsPatch
	{
		[HarmonyPatch(typeof(MinionStartingStats), "Deliver")]
		public class MinionStartingStats_Deliver_Patch
		{
			public static void Postfix(MinionStartingStats __instance, GameObject __result)
			{
				CustomDupe.stats.Remove(__instance);
			}
		}

		[HarmonyPatch(typeof(MinionStartingStats), "Apply")]
		public class MinionStartingStats_Apply_Patch
		{
			public static void Prefix(MinionStartingStats __instance, GameObject go)
			{
				if (__instance.personality.model == GameTags.Minions.Models.Bionic)
					return;
				CustomDupe.stats.Remove(__instance);
			}
		}

		[HarmonyPatch(typeof(MinionStartingStats), "GenerateStats")]
		public class MinionStartingStats_GenerateStats_Patch
		{
			public static void Prefix(MinionStartingStats __instance)
			{
				if (__instance.personality.model == GameTags.Minions.Models.Bionic)
					return;

				var randomReplaceChance = Mod.Settings.GetActualRandomReplaceChance();
				if (ImmigrationModifier.Instance.ActiveBundle == Bundle.Shaker
					|| (randomReplaceChance > 0 && Random.value <= randomReplaceChance))
				{
					CustomDupe.stats.Add(__instance);
				}
			}

		}

		[HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
		public class MinionStartingStats_GenerateTraits_Patch
		{
			// __result is pointsDelta
			public static void Postfix(MinionStartingStats __instance, ref int __result)
			{
				if (__instance.personality.model == GameTags.Minions.Models.Bionic)
					return;

				if (CustomDupe.stats.Contains(__instance))
				{
					var settings = BundleLoader.bundleSettings.ActiveRando(__instance);
					var value = Random.Range(settings.MinimumSkillBudgetModifier, settings.MaximumSkillBudgetModifier + 1);

					__result += Mathf.FloorToInt(value);

					if (ImmigrationModifier.Instance.ActiveBundle == Bundle.SuperDuplicant)
					{
						DupeGenHelper.AddRandomTraits(__instance, 1, 1, DUPLICANTSTATS.GENESHUFFLERTRAITS);
						__result += BundleLoader.bundleSettings.vacillating.ExtraSkillBudget;
						__result = Mathf.Clamp(__result, 0, settings.MaximumTotalBudget + BundleLoader.bundleSettings.vacillating.ExtraSkillBudget / 2);
					}
					else
					{
						__result = Mathf.Clamp(__result, 0, settings.MaximumTotalBudget);
					}

					DupeGenHelper.AddRandomTraits(__instance, 0, settings.MaxBonusPositiveTraits, DUPLICANTSTATS.GOODTRAITS);
					DupeGenHelper.AddRandomTraits(__instance, 0, settings.MaxBonusNegativeTraits, DUPLICANTSTATS.BADTRAITS);

					if (Random.value < 0.5f)
						DupeGenHelper.AddRandomTraits(__instance, 1, 1, DUPLICANTSTATS.NEEDTRAITS);


					__result = Mathf.Clamp(__result, 0, 20);
				}

				if (ImmigrationModifier.Instance.ActiveBundle == Bundle.SuperDuplicant)
				{
					Log.Debug("shuffler ink");
					DupeGenHelper.AddGeneShufflerTrait(__instance);
					__result += BundleLoader.bundleSettings.vacillating.ExtraSkillBudget;
				}
			}
		}
	}
}
