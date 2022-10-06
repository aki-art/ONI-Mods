using FUtility;
using FUtility.FUI;
using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;
using UnityEngine;

namespace SpookyPumpkinSO.Content
{
    internal class Tests
    {

        [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
        public class MinionIdentity_OnSpawn_Patch
        {
            public static void Postfix(MinionIdentity __instance)
            {
                //Helper.ListComponents(__instance.gameObject);

                if (__instance.TryGetComponent(out KBatchedAnimController kbac))
                {
                    Log.Debuglog("RENDERER FOUND");

                    var index = (int)kbac.GetMaterialType();
                    var t = Traverse.Create(kbac.GetBatch().group);
                    var materials = t.Field<Material[]>("materials").Value;

                    //materials[index] = new Material(Shader.Find("Klei/Building Place"));
                    //kbac.SetDirty();

                    //kbac.SetBatch(kbac.GetBatch());

                    var color = new Color(0, 24f, 30f, 0.23f);
                    /*
                    kbac.SetSymbolTint("snapTo_hair", color);
                    kbac.SetSymbolTint("snapTo_hair_always", color);
                    kbac.SetSymbolTint("snapTo_hat_hair", color);
                    kbac.SetSymbolTint("snapTo_headshape", color);
                    kbac.SetSymbolTint("snapTo_eyes", color);
                    kbac.SetSymbolTint("snapTo_mouth", color);
                    */


                    __instance.gameObject.AddComponent<Ghastly>();
                    //kbac.SetBlendValue(0.4f);
                    //kbac.OverlayColour = new Color(0, 0.7f, 1.2f, 0.4f);
                    //kbac.HighlightColour = new Color(0, 0.7f, 1.2f, 0.4f);
                    //var groupid = kbac.GetBatchGroupID(); ;
                    //var material = kbac.GetBatch();
                    //var material = kbac.GetBatchInstanceData();
                    //renderer.uiMat = new Material(Shader.Find("Klei/BloomedParticleShader"));
                }
            }
        }
    }
}
