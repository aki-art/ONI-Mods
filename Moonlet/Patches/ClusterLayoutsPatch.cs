using HarmonyLib;
using ProcGen;

namespace Moonlet.Patches
{
	public class ClusterLayoutsPatch
	{
		[HarmonyPatch(typeof(ClusterLayouts), "LoadFiles")]
		public class ClusterLayouts_LoadFiles_Patch
		{
			public static void Postfix()
			{
				Mod.clustersLoader.ApplyToActiveTemplates(loader => loader.LoadContent());
			}
		}
	}
}
