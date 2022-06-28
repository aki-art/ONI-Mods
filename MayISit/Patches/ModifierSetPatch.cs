using FUtility;
using HarmonyLib;
using Klei.AI;

namespace MayISit.Patches
{
    public class ModifierSetPatch
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
            }
        }

        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                ModDb.Effects.Register(__instance);

                PrintEffect(__instance.effects.Get("Socialized"));
                PrintEffect(__instance.effects.Get("RecentlySocialized"));

            }

            private static void PrintEffect(Effect socialized)
            {
                Log.Debuglog("socialized " + socialized == null);
                Log.Debuglog("duration " + socialized.duration);
                Log.Debuglog("emoteCooldown " + socialized.emoteCooldown);
                Log.Debuglog("showInUI " + socialized.showInUI);
                Log.Debuglog("triggerFloatingText " + socialized.triggerFloatingText);

                Log.Debuglog("conditions");

                if (socialized.emotePreconditions != null)
                {
                    foreach (var condition in socialized.emotePreconditions)
                    {
                        Log.Debuglog("   condition ");
                    }
                }

                Log.Debuglog("modifiers");

                if (socialized.SelfModifiers != null)
                {

                    foreach (var modifier in socialized.SelfModifiers)
                    {
                        Log.Debuglog("   modifier " + modifier?.AttributeId + " - " + modifier?.Value + ", mult: " + modifier?.IsMultiplier);
                    }
                }

                Log.Debuglog(socialized.duration);
            }
        }
    }
}
