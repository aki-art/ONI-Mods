using HarmonyLib;

namespace GoldenThrone.Patches
{
	public class ElementLoaderPatch
	{
		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public class ElementLoader_Load_Patch
		{
			public static void Postfix()
			{
				Mod.Settings.SetPreciousMetals();
			}
		}
	}
}
