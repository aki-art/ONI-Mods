using HarmonyLib;
using Moonlet.Content.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlet.Patches
{
	internal class GamePatch
	{
		[HarmonyPatch(typeof(Game), "DestroyInstances")]
		public static class GameDestroyInstances
		{
			public static void Postfix()
			{
				Moonlet_ZonetypePainterTool.DestroyInstance();
			}
		}


		[HarmonyPatch(typeof(Game), "OnPrefabInit")]
		public class Game_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
			}
		}
	}
}
