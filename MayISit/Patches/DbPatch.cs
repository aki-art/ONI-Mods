using FUtility;
using HarmonyLib;
using MayISit.Content.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MayISit.Patches
{
    internal class DbPatch
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                if(Mod.Setting.Chairs == null || Mod.Setting.Chairs.Count == 0)
                {
                    Log.Warning($"There are no chairs configured. Try resetting the mod or reviewing the configuration at {Path.Combine(Utils.ModPath, "settings.json")}");
                    return;
                }

                foreach(var chairInfo in Mod.Setting.Chairs)
                {
                    var prefab = Assets.GetPrefab(chairInfo.Key);
                    if(prefab != null && prefab.TryGetComponent(out KPrefabID kPrefabID))
                    {
                        kPrefabID.prefabSpawnFn += go => OnChairSpawn(go, chairInfo.Value);
                    }
                }
            }

            private static void OnChairSpawn(GameObject go, Config.SeatEntry[] seatEntry)
            {
                //var seat = go.AddComponent<Seat>();
            }
        }
    }
}
