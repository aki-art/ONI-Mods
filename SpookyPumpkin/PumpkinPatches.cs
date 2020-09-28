using FUtility;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookyPumpkin
{
    class PumpkinPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Log.PrintVersion();
            }
        }


        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {

            public static void Postfix(BuildingComplete __instance)
            {


                var blooms = UnityEngine.Object.FindObjectsOfType<BloomEffect>();
                foreach(var bloom in blooms)
                {
                   // Debug.Log($"BLOOM: {bloom.gameObject.name}");
                    //FUtility.FUI.Helper.ListComponents(bloom.gameObject);
                    //FUtility.FUI.Helper.ListChildren(bloom.transform);
                }

                var controller2 = __instance.GetComponent<KBatchedAnimController>();
                if (controller2 == null) return;
               // var batch = controller2.batchGroupID();
                //batch.GetDataTextures
/*                var controller = __instance.GetComponent<KAnimControllerBase>();
                if (controller == null) return;
                foreach (KAnimFile kanimFile in controller.AnimFiles)
                {
                    if (!(kanimFile == null))
                    {
                        KAnimFileData data = kanimFile.GetData();
                        if (data.build != null && data.frameCount > 0)
                        {
                            *//*                            KAnim.Build.SymbolFrame[] frames = data.build.frames;
                                                        foreach (KAnim.Build.SymbolFrame frame in frames)
                                                        {

                                                            Debug.Log(frame.);
                                                        }*//*
                            Debug.Log("Frame elements:" + __instance.name);
                            for (int i = 0; i < data.frameCount; i++)
                            {
                                var frame = data.GetAnimFrameElement(i);
                                Debug.Log(frame.flags);
                            }

                        }
                    }
                }

                var controller2 = __instance.GetComponent<KBatchedAnimController>();
                if (controller2 == null) return;

                var batch = controller2.GetBatch();
                Material material = batch.group.GetMaterial(batch.materialType);
                if (batch == null)
                {
                    Debug.Log("there is no batch");
                    return;
                }
                for (int j = 0; j < batch.group.data.frameElements.Count; j++)
                {
                    KAnim.Anim.Frame frame = batch.group.data.GetFrame(j);
                    if (frame == KAnim.Anim.Frame.InvalidFrame)
                    {
                        return;
                    }
                    HashedString hash = HashedString.Invalid;
                    for (int i = 0; i < frame.numElements; i++)
                    {
                        int num = frame.firstElementIdx + i;
                        if (num < batch.group.data.frameElements.Count)
                        {
                            KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
                            if (!(frameElement.symbol == HashedString.Invalid))
                            {
                                Debug.Log(frameElement.flags);
                                
                                //hash = frameElement.symbol;
                                break;
                            }
                        }
                    }
                }*/
            }
        }

    }
}
