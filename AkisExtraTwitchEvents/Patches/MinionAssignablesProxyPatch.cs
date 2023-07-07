using HarmonyLib;
using System.Linq;

namespace Twitchery.Patches
{
	//ugly fix for an ugly error i made
	public class MinionAssignablesProxyPatch
	{
        [HarmonyPatch(typeof(MinionAssignablesProxy), "OnPrefabInit")]
        public class MinionAssignablesProxy_OnPrefabInit_Patch
        {
            public static bool Prefix(MinionAssignablesProxy __instance)
            {
                if(Components.MinionAssignablesProxy.Contains(__instance))
                {
                    Util.KDestroyGameObject(__instance.gameObject);
                    return false;
                }

                return true;
            }
        }
	}
}
