using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using PrintingPodRecharge.Content.Items;

namespace PrintingPodRecharge.Patches
{
	public class ImmigrationPatches
	{
		[HarmonyPriority(Priority.Last)]
		[HarmonyPatch(typeof(Immigration), "OnPrefabInit")]
		public class Immigration_OnPrefabInit_Patch
		{
			public static void Postfix(Immigration __instance)
			{
				__instance.gameObject.AddOrGet<ImmigrationModifier>().LoadBundles();
				__instance.gameObject.AddOrGet<BioInksD6Manager>();
			}
		}

		[HarmonyPatch(typeof(Immigration), "RandomCarePackage")]
		public class Immigration_RandomCarePackage_Patch
		{
			public static void Postfix(ref CarePackageInfo __result)
			{
				if (ImmigrationModifier.Instance.IsOverrideActive)
				{
					var package = ImmigrationModifier.Instance.GetRandomPackage();
					if (package != null)
						__result = package;
				}
			}
		}

		[HarmonyPatch(typeof(Immigration), "EndImmigration")]
		public class Immigration_EndImmigration_Patch
		{
			public static void Postfix()
			{
				if (Integration.TwitchIntegration.UselessPrintsCommand.queued)
					Integration.TwitchIntegration.UselessPrintsCommand.Print();
				else if (Integration.TwitchIntegration.HelpfulPrintsCommand.queued)
					Integration.TwitchIntegration.HelpfulPrintsCommand.Print();
				else
					ImmigrationModifier.Instance.SetModifier(Bundle.None);
			}
		}

		[HarmonyPatch(typeof(Immigration))]
		[HarmonyPatch("ConfigureCarePackages")]
		public static class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(ref CarePackageInfo[] ___carePackages)
			{
				if (Mod.otherMods.IsSomeRerollModHere)
					___carePackages = ___carePackages.AddToArray(new CarePackageInfo(BioInkConfig.DEFAULT, 2f, null));
			}
		}
	}
}
