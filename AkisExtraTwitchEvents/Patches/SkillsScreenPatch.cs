using HarmonyLib;

namespace Twitchery.Patches
{
	public class SkillsScreenPatch
	{
		[HarmonyPatch(typeof(SkillsScreen), "SortRows")]
		public class SkillsScreen_SortRows_Patch
		{
			public static void Prefix(SkillsScreen __instance)
			{
				if (__instance == null || __instance.sortableRows == null)
					return;

				for (int i = __instance.sortableRows.Count - 1; i >= 0; i--)
				{
					var ai = __instance.sortableRows[i].assignableIdentity;
					if (ai == null)
						continue;

					if (__instance.sortableRows.FindAll(r => r.assignableIdentity == ai).Count >= 2)
					{
						__instance.sortableRows.RemoveAt(i);
						Ownables ownables = ai.GetSoleOwner();
						if (ownables != null && ownables.gameObject != null)
							Util.KDestroyGameObject(ownables.gameObject);
					}
				}
			}
		}
	}
}
