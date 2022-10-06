using FUtility;
using HarmonyLib;
using Slag.Content.Items;

namespace Slag.Patches
{
    public class EdiblePatch
    {
        [HarmonyPatch(typeof(Edible), "StopConsuming")]
        public class Edible_StopConsuming_Patch
        {
            public static void Postfix(Edible __instance, Worker worker)
            {
                if (__instance.PrefabID() == CrispyFireCracklingsConfig.ID)
                {
                    Log.Debuglog("Eaten crispy fire");
                    new EmoteChore(
                        worker.GetComponent<ChoreProvider>(),
                        Db.Get().ChoreTypes.EmoteHighPriority,
                        "anim_react_relish_kanim",
                        new HashedString[]
                        {
                            "react"
                        },
                        null);
                }
            }
        }
    }
}
