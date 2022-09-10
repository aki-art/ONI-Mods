using HarmonyLib;
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Patches
{
    // Update the hair color on the little UI dupe portraits
    public class CrewPortraitPatch
    {
        [HarmonyPatch(typeof(CrewPortrait), "SetPortraitData")]
        public class CrewPortrait_SetPortraitData_Patch
        {
            public static void Postfix(IAssignableIdentity identityObject, KBatchedAnimController controller)
            {
                if(controller == null)
                {
                    return;
                }

                if(identityObject is MinionIdentity identity)
                {
                    HairDye.Apply(identity, controller);
                }
                if (identityObject is StoredMinionIdentity storedIdentity)
                {
                    HairDye.Apply(storedIdentity, controller);
                }
                else if(identityObject is MinionAssignablesProxy proxy)
                {
                    if(proxy.target is KMonoBehaviour minion)
                    {
                        HairDye.Apply(minion, controller);
                    }
                }
            }
        }
    }
}
