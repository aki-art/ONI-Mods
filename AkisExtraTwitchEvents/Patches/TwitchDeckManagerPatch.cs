using FUtility;
using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.Core;
using Twitchery.Content.Events;

namespace Twitchery.Patches
{
	public class TwitchDeckManagerPatch
	{
        [HarmonyPatch(typeof(TwitchDeckManager), "Draw")]
        public class TwitchDeckManager_Draw_Patch
		{
            public static void Postfix(EventInfo __result)
			{
				Log.Debuglog("Event: " + __result.Id);
				if (__result != null && __result.Id == PolymorphEvent.ID)
                {
                    Log.Debuglog("Polymoprh has been rolled!");
                }
            }
        }
	}
}
