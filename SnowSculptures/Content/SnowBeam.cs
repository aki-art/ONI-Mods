using FUtility;
using HarmonyLib;
using System;
using TemplateClasses;
using UnityEngine;

namespace SnowSculptures.Content
{
    public class SnowBeam
    {
        public const string CONTEXT = "SnowSculptures_SnowmanBuilding";
        public const string LASER_EFFECT = "SnowSculptures_SnowManEffect";

        public static void AddSnapOn(GameObject gameObject)
        {
            gameObject.AddOrGet<SnapOn>().snapPoints.Add(new SnapOn.SnapPoint
            {
                pointName = "dig",
                automatic = false,
                context = CONTEXT,
                buildFile = Assets.GetAnim("water_gun_kanim"),
                overrideSymbol = "snapTo_rgtHand"
            });
        }

        internal static void AddLaserEffect(GameObject minionPrefab)
        {
            var laserEffects = minionPrefab.transform.Find("LaserEffect").gameObject;
            var kbatchedAnimEventToggler = laserEffects.GetComponent<KBatchedAnimEventToggler>();
            var kbac = minionPrefab.GetComponent<KBatchedAnimController>();

            var laserEffect = new MinionConfig.LaserEffect
            {
                id = LASER_EFFECT,
                animFile = "sm_snowbeam_kanim",
                anim = "loop",
                context = CONTEXT
            };

            var laserGo = new GameObject(laserEffect.id);
            laserGo.transform.parent = laserEffects.transform;
            laserGo.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);

            var tracker = laserGo.AddOrGet<KBatchedAnimTracker>();
            tracker.controller = kbac;
            tracker.symbol = new HashedString("snapTo_rgtHand");
            tracker.offset = new Vector3(195f, -35f, 0f);
            tracker.useTargetPoint = true;

            var kbatchedAnimController = laserGo.AddOrGet<KBatchedAnimController>();
            kbatchedAnimController.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim(laserEffect.animFile)
            };

            var item = new KBatchedAnimEventToggler.Entry
            {
                anim = laserEffect.anim,
                context = laserEffect.context,
                controller = kbatchedAnimController
            };

            kbatchedAnimEventToggler.entries.Add(item);

            laserGo.AddOrGet<LoopingSounds>();
        }
    }
}
