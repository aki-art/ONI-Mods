using FUtility;
using HarmonyLib;
using KMod;

namespace NoNameLengthLimit
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			Log.PrintVersion(this);
		}


		[HarmonyPatch(typeof(EditableTitleBar), "OnSpawn")]
		public class EditableTitleBar_OnSpawn_Patch
		{
			public static void Postfix(EditableTitleBar __instance)
			{
				__instance.inputField.characterLimit = 1024;
			}
		}
	}
}
