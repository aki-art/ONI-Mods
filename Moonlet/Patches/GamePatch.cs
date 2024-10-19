using HarmonyLib;
using Moonlet.Scripts.Tools;

namespace Moonlet.Patches
{
	public class GamePatch
	{
		[HarmonyPatch(typeof(Game), "DestroyInstances")]
		public static class GameDestroyInstances
		{
			public static void Postfix()
			{
				Moonlet_ZonetypePainterTool.DestroyInstance();
			}
		}
	}
}
