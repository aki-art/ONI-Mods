﻿using HarmonyLib;
using Moonlet.Scripts.UI;

namespace Moonlet.Patches
{
	public class PlayerControllerPatch
	{
		[HarmonyPatch(typeof(PlayerController), "OnKeyDown")]
		public class PlayerController_OnKeyDown_Patch
		{
			public static void Prefix(KButtonEvent e)
			{
				if (e.TryConsume(ModActions.OpenConsole.GetKAction()))
				{
					DevConsoleScreen.Instance.Show();
				}
			}
		}
	}
}
