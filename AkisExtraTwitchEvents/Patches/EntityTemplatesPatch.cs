/*using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class EntityTemplatesPatch
	{
		[HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToFood")]
		public class EntityTemplates_ExtendEntityToFood_Patch
		{
			public static void Postfix(GameObject template)
			{
				if (template.TryGetComponent(out Edible _))
					template.AddOrGet<AETE_GoldFlakeable>();
			}
		}
	}
}
*/