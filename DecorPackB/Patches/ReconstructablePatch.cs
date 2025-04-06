using HarmonyLib;
using System.Linq;

namespace DecorPackB.Patches
{
	// https://forums.kleientertainment.com/klei-bug-tracker/oni/changing-materials-deletes-secondary-and-tertiary-materials-r48458/
	public class ReconstructablePatch
	{
		[HarmonyPatch(typeof(Reconstructable), "RequestReconstruct")]
		public class Reconstructable_RequestReconstruct_Patch
		{
			public static void Postfix(Reconstructable __instance)
			{
				if (DetailsScreen.Instance == null)
					return;

				if (__instance.selectedElementsTags.Length > 1)
					return;

				var tab = DetailsScreen.Instance.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material);

				if (tab == null)
					return;

				var panel = tab.bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>();

				if (panel == null)
					return;

				var panels = panel.GetComponentsInChildren<MaterialSelector>();

				if (panels.Length > 1)
				{
					var elements = panels
						.Where(p => p.isActiveAndEnabled)
						.Select(p => p.CurrentSelectedElement)
						.ToArray();

					if (elements.Length > 0)
						__instance.selectedElementsTags = elements;
				}
			}
		}
	}
}
