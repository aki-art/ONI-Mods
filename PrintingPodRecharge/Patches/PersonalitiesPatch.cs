using Database;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class PersonalitiesPatch
    {
        [HarmonyPatch(typeof(Personalities), "GetPersonalityFromNameStringKey")]
        public class Personalities_GetPersonalityFromNameStringKey_Patch
        {
            public static void Prefix(ref string name_string_key, ref Personality __result)
            {
                if(name_string_key.StartsWith("shook_"))
                {
                    // TODO
                }
            }
        }
    }
}
