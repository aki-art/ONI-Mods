using HarmonyLib;

namespace CrittersDropBones.Patches
{
    public class EdiblePatch
    {
        [HarmonyPatch(typeof(Edible), "StopConsuming")]
        public class Edible_StopConsuming_Patch
        {
            public static void Postfix(Worker worker, EdiblesManager.FoodInfo ___foodInfo)
            {
                if(worker == null)
                {
                    return;
                }

                ModDb.FoodEffects.Apply(___foodInfo.Id, worker);
            }
        }
    }
}
