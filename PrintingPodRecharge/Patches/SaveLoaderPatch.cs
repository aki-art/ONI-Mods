using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System;

namespace PrintingPodRecharge.Patches
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), "Save", new [] { typeof(string), typeof(bool), typeof(bool) })]
		public class SaveLoader_Save_Patch
		{
			public static void Prefix()
			{
				foreach (var identity in Components.MinionIdentities.Items)
				{
					if (identity.TryGetComponent(out CustomDupe hairDye))
						hairDye.OnSaveGame();
				}
			}

			public static void Postfix()
			{
				foreach (var identity in Components.MinionIdentities.Items)
				{
					if (identity.TryGetComponent(out CustomDupe customDupe))
						customDupe.OnLoadGame();
				}
			}
		}
	}
}
