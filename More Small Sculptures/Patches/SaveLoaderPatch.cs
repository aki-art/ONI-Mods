using HarmonyLib;

namespace MoreSmallSculptures.Patches
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), "Save", new[]
		{
			typeof(string),
			typeof(bool),
			typeof(bool)
		})]
		public class SaveLoader_Save_Patch
		{
			public static void Prefix()
			{
				foreach (var restore in Mod.artRestorers.Items)
					restore.OnSaveGame();
			}

			public static void Postfix()
			{
				foreach (var restore in Mod.artRestorers.Items)
					restore.Restore();
			}
		}
	}
}