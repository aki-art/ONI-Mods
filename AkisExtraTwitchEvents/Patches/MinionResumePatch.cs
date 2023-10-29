using HarmonyLib;

namespace Twitchery.Patches
{
	public class MinionResumePatch
	{
		// shut up the status item perks about noone being able to do this, when a pip can
		[HarmonyPatch(typeof(MinionResume), "AnyMinionHasPerk")]
		public class MinionResume_AnyMinionHasPerk_Patch
		{
			public static void Postfix(string perk, int worldId, ref bool __result)
			{
				if (__result || Mod.regularPips == null)
					return;

				var pips = worldId >= 0 ? Mod.regularPips.GetWorldItems(worldId) : Mod.regularPips.Items;

				if (pips.Count == 0)
					return;

				foreach (var pip in pips)
				{
					bool isAlive = !pip.HasTag(GameTags.Dead);

					if (isAlive && pip.HasPerk(perk))
					{
						__result = true;
						return;
					}
				}
			}
		}
	}
}
