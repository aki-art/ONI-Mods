using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;

namespace PrintingPodRecharge.Patches
{
	[HarmonyPatch(typeof(SkillsScreen), "CurrentlySelectedMinion", MethodType.Setter)]
	public class SkillsScreen_CurrentlySelectedMinion_Patch
	{
		public static void Prefix(IAssignableIdentity value)
		{
			if (value is MinionIdentity identity)
			{
				CustomDupe.UpdateIdentity(identity);
			}
		}
	}
}
