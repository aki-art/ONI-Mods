using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace PrintingPodRecharge.Patches
{
	public class CarePackageContainerPatch
	{
		// Fixes incorrect KCAl formatting for some items that should display themselves so
		[HarmonyPatch(typeof(CarePackageContainer), "GetSpawnableQuantityOnly")]
		public class CarePackageContainer_GetSpawnableQuantityOnly_Patch
		{
			public static void Postfix(ref string __result, CarePackageInfo ___info)
			{
				var info = EdiblesManager.GetFoodInfo(___info.id);

				if (info != null && info.CaloriesPerUnit <= 0)
					__result = string.Format(global::STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, ___info.quantity.ToString());
			}
		}

		[HarmonyPatch(typeof(CarePackageContainer), "GetCurrentQuantity")]
		public class CarePackageContainer_GetCurrentQuantity_Patch
		{
			public static void Postfix(WorldInventory inventory, ref string __result, CarePackageInfo ___info)
			{
				var info = EdiblesManager.GetFoodInfo(___info.id);

				if (info != null && info.CaloriesPerUnit <= 0)
				{
					var amount = inventory.GetAmount(___info.id.ToTag(), false);
					__result = string.Format(global::STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, amount.ToString());
				}
			}
		}

		// Foods with 0 calorie (such as raw egg or some seeds) would appear glitchy and weird, with no animation
		[HarmonyPatch(typeof(CarePackageContainer), "SetAnimator")]
		public static class CarePackageContainer_SetAnimator_Patch
		{
			public static void Postfix(CarePackageContainer __instance)
			{
				if (!ImmigrationModifier.Instance.IsOverrideActive)
					return;

				var activeBundle = ImmigrationModifier.Instance.GetActiveCarePackageBundle();

				if (activeBundle != null && activeBundle.replaceAnim && activeBundle.bgAnim != null)
				{
					// animController field on __instance is never assigned
					var bg = __instance.transform.Find("Details/PortraitContainer/BG");

					if (bg == null)
					{
						Log.Warning("__instance.transform.Find(\"Details/PortraitContainer/BG\") is null");
						return;
					}

					var kbac = __instance.transform.Find("Details/PortraitContainer/BG").GetComponent<KBatchedAnimController>();
					kbac.SwapAnims(activeBundle.bgAnim);

					var bgTint = activeBundle.printerBgTint;
					var glowTint = activeBundle.printerBgTintGlow;

					kbac.SetSymbolTint("forever", bgTint);
					kbac.SetSymbolTint("grid_bloom", glowTint);
					kbac.SetSymbolTint("inside_rough", glowTint);

					kbac.SetDirty();
					kbac.UpdateAnim(1);
					kbac.Play("crewSelect_bg", KAnim.PlayMode.Loop);
				}
			}

			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
			{
				var f_CaloriesPerUnit = AccessTools.Field(typeof(EdiblesManager.FoodInfo), "CaloriesPerUnit");

				var codes = orig.ToList();

				var lnLoadCaloriesPerUnit = codes.FindIndex(ci => ci.LoadsField(f_CaloriesPerUnit));

				if (lnLoadCaloriesPerUnit == -1)
					return codes;

				var lnStoreNum = codes.FindIndex(lnLoadCaloriesPerUnit, ci => ci.opcode == OpCodes.Stloc_1);

				if (lnStoreNum == -1)
					return codes;

				var f_info = AccessTools.Field(typeof(CarePackageContainer), "info");
				var m_FilterFoodInfo = AccessTools.Method(typeof(CarePackageContainer_SetAnimator_Patch), "FilterFoodInfo", new[] { typeof(int), typeof(CarePackageInfo) });

				codes.InsertRange(lnStoreNum, new[]
				{
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Ldfld, f_info),
					new CodeInstruction(OpCodes.Call, m_FilterFoodInfo)
				});

				Log.PrintInstructions(codes);

				return codes;
			}

			private static int FilterFoodInfo(int existingValue, CarePackageInfo info)
			{
				return existingValue <= 0 ? (int)info.quantity : existingValue;
			}
		}
	}
}
