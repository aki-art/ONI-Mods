using FUtility;
using HarmonyLib;
using KSerialization;
using PrintingPodRecharge.Cmps;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using static PrintingPodRecharge.Patches.SaveLoaderPatch.SaveLoader_Save_Patch;

namespace PrintingPodRecharge.Patches
{
    public class SaveLoaderPatch
    {

        [HarmonyPatch(typeof(SaveLoader), "Load", typeof(IReader))]
        public class SaveLoader_Load_Patch
        {
            private static FakeMinionIdentity fakeIdentity;

            public static void Postfix(IReader reader)
            {
                if(fakeIdentity == null)
                {
                    fakeIdentity = new GameObject().AddComponent<FakeMinionIdentity>();
                }

                Log.Debuglog("POST LOAD");
                foreach (MinionIdentity identity in Components.MinionIdentities)
                {
                    Log.Debuglog("IDENTITY ");
                    if (identity.TryGetComponent(out CustomDupe customDupe))
                    {
                        var mapping = Manager.GetDeserializationMapping(typeof(FakeMinionIdentity));

                        mapping.Deserialize(fakeIdentity, reader);

                        Log.Debuglog("DESERIALIZED FAKE IDENTITY");
                        Log.Debuglog(fakeIdentity.bodyData.headShape);
                    }
                }
            }

            [SerializationConfig(MemberSerialization.OptIn)]
            public class FakeMinionIdentity : KMonoBehaviour
            {
                [Serialize]
                public KCompBuilder.BodyData bodyData;
            }
        }

        [HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix()
            {
                foreach (MinionIdentity identity in Components.MinionIdentities)
                {
                    if (identity.TryGetComponent(out CustomDupe hairDye))
                    {
                        hairDye.OnSaveGame();
                    }
                }
            }

            public static void Postfix()
            {
                foreach (MinionIdentity identity in Components.MinionIdentities)
                {
                    if (identity.TryGetComponent(out CustomDupe customDupe))
                    {
                        customDupe.OnLoadGame();
                    }
                }
            }
        }
    }
}
