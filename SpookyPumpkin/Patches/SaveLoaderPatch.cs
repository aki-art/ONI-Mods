using HarmonyLib;
using System;

namespace SpookyPumpkinSO.Patches
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
		public class SaveLoader_Save_Patch
		{
			public static void Prefix()
			{
				foreach (var restore in Mod.spiceRestorers.Items)
					restore.OnSaveGame();

				foreach (var facePaint in Mod.facePaints.Items)
					facePaint.OnSaveGame();

				foreach (var facadeRestorer in Mod.facadeRestorers.Items)
					facadeRestorer.OnSaveGame();
			}

			public static void Postfix()
			{
				foreach (var restore in Mod.spiceRestorers.Items)
					restore.OnLoadGame();

				foreach (var facePaint in Mod.facePaints.Items)
					facePaint.OnLoadGame();

				foreach (var facadeRestorer in Mod.facadeRestorers.Items)
					facadeRestorer.AfterSave();
			}
		}
	}
}