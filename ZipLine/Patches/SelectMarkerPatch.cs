using FUtility;
using HarmonyLib;
using UnityEngine;
using ZipLine.Content.Cmps;
using ZipLine.Content.Entities;

namespace ZipLine.Patches
{
    // make the little triangle slection marker closer to the actual rope,
    // 1 tile above the middle of it, instead of hovering above it's maximum Y
    public class SelectMarkerPatch
    {
        [HarmonyPatch(typeof(SelectMarker), "SetTargetTransform")]
        public class SelectMarker_SetTargetTransform_Patch
        {
            public static void Prefix(SelectMarker __instance, Transform target_transform)
            {
            }
        }


        [HarmonyPatch(typeof(SelectMarker), "LateUpdate")]
        public class SelectMarker_LateUpdate_Patch
        {
            public static void Postfix(SelectMarker __instance, Transform ___targetTransform)
            {
                if (___targetTransform.gameObject.TryGetComponent(out KPolygonCollider2D collider))
                {
                    Log.Debuglog("select", -(collider.bounds.size.y / 2f));
                    __instance.transform.position += new Vector3(0, -(collider.bounds.size.y / 2f));
                }
            }
        }
    }
}
