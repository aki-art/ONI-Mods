using DecorPackB.Items;
using FUtility;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class DiggablePatch
    {
        [HarmonyPatch(typeof(Diggable), "OnStopWork")]
        public class Diggable_OnStopWork_Patch
        {
            public static void Prefix(Diggable __instance, bool ___isDigComplete, Element ___originalDigElement)
            {
                Log.Debuglog(___isDigComplete, ___originalDigElement.id);
                if (___isDigComplete && ___originalDigElement.id == SimHashes.Fossil)
                {
                    Log.Debuglog("spawning");
                    float chance = DlcManager.IsExpansion1Active() ?
                        Mod.Settings.FossilDisplay.FossileNoduleFromFossilChance_SpacedOut :
                        Mod.Settings.FossilDisplay.FossileNoduleFromFossilChance_VanillaOrClassic;

                    if (UnityEngine.Random.value < 1f)
                    {
                        Utils.Spawn(FossilNoduleConfig.ID, __instance.transform.position);
                    }
                }
            }
        }
    }
}
