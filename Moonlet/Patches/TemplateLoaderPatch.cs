using HarmonyLib;
using Moonlet.Scripts;
using UnityEngine;

namespace Moonlet.Patches
{
	public class TemplateLoaderPatch
	{
		[HarmonyPatch(typeof(TemplateLoader), nameof(TemplateLoader.Stamp))]
		public class TemplateLoader_Stamp_Patch
		{
			public static void Postfix(TemplateContainer template, Vector2 rootLocation)
			{
				Moonlet_Mod.Instance.ApplyZoneTypeOverrides(template, rootLocation);
			}
		}
	}
}
