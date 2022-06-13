using HarmonyLib;
using Kigurumis.Content;
using UnityEngine;

namespace Kigurumis.Patches
{
    internal class MinionConfigPatch
    {
        [HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        public class MinionConfig_CreatePrefab_Patch
        {
            public static void Postfix(ref GameObject __result)
            {
                var snapPoints = __result.GetComponent<SnapOn>().snapPoints;
                var kigurumis = Db.Get().EquippableFacades.resources.FindAll(r => r.DefID == KigurumiConfig.ID);

                foreach (var kigu in kigurumis)
                {
                    snapPoints.Add(new SnapOn.SnapPoint
                    {
                        pointName = kigu.Id,
                        automatic = false,
                        context = "",
                        buildFile = Assets.GetAnim(kigu.Id + "_kanim"),
                        overrideSymbol = "snapTo_hat"
                    });
                }
            }
        }
    }
}
