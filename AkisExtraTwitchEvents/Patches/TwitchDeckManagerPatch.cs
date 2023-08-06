using FUtility;
using HarmonyLib;
using System;
using System.Reflection;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class TwitchDeckManagerPatch
	{
		public static void TryPatch(Harmony harmony)
		{
			var type = ONITwitchLib.Core.CoreTypes.TwitchDeckManagerType;
			if (type != null)
			{
				var original = type.GetMethod("Draw", BindingFlags.Public | BindingFlags.Instance);

				if(original == null)
				{
					Log.Warning("TwitchDeckManager.Draw doesn't exist or it's signature was changed.");
					return;
				}

				var postfix = typeof(TwitchDeckManagerPatch).GetMethod(nameof(Postfix), new Type[] {});

				harmony.Patch(original, postfix: new HarmonyMethod(postfix));
			}
		}

		public static void Postfix()
		{
			AkisTwitchEvents.Instance.OnDraw();
		}
	}
}
