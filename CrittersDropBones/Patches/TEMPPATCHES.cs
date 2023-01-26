#if DEBUG
using HarmonyLib;
namespace CrittersDropBones.Patches
{
    public class TEMPPATCHES
    {

        [HarmonyPatch(typeof(CodexEntryGenerator), "GenerateFoodDescriptionContainers")]
        public class CodexEntryGenerator_GenerateFoodDescriptionContainers_Patch
        {
            public static void Prefix(EdiblesManager.FoodInfo food)
            {
                Log.Assert("foodinfo", food);
                Log.Debuglog($"Generating food info for {food.Name}");
                Log.Debuglog($"Description: {food.Description}");
                Log.Assert("effects", food.Effects);
            }
        }
    }
}
#endif
