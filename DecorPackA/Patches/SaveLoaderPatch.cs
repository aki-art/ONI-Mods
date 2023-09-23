using HarmonyLib;
using System;

namespace DecorPackA.Patches
{
	public class SaveLoaderPatch
	{
		// prevent the game trying to remember and load a non-existing facade after the mod has been uninstalled
		[HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
		public class SaveLoader_Save_Patch
		{
			public static void Prefix(ref string __state)
			{
				if (DPFacades.myFacades.Contains(PlanScreen.Instance.lastSelectedBuildingFacade))
				{
					__state = PlanScreen.Instance.lastSelectedBuildingFacade;
					PlanScreen.Instance.lastSelectedBuildingFacade = "DEFAULT_FACADE";
				}

				foreach (var restorer in Mod.facadeRestorers.items)
					restorer.OnSave();
			}

			public static void Postfix(ref string __state)
			{
				if (!__state.IsNullOrWhiteSpace())
					PlanScreen.Instance.lastSelectedBuildingFacade = __state;

				foreach (var restorer in Mod.facadeRestorers.items)
					restorer.AfterSave();
			}
		}
	}
}
