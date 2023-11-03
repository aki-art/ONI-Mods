using HarmonyLib;
using UnityEngine;

namespace Moonlet.Patches
{
	public class GameUtilPatch
	{
		[HarmonyPatch(typeof(GameUtil), "GetBiomeSprite")]
		public class GameUtil_GetBiomeSprite_Patch
		{
			public static bool Prefix(string id, ref Sprite __result)
			{
				if (Mod.subworldCategoriesLoader.TryGet(id, out var subworldCategory))
				{
					if (!subworldCategory.template.Icon.IsNullOrWhiteSpace())
					{
						__result = Assets.GetSprite(subworldCategory.template.Icon);
						return false;
					}
				}

				return true;
			}
		}
	}
}
