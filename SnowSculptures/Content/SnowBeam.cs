using FUtility;
using HarmonyLib;
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
                buildFile = Assets.GetAnim("painting_gun_kanim"),
                overrideSymbol = "snapTo_rgtHand"
            });
        }

        public static MinionConfig.LaserEffect GetLaserEffect()
        {
            return new MinionConfig.LaserEffect
            {
                id = LASER_EFFECT,
                animFile = "sm_snowbeam_kanim",
                anim = "loop",
                context = CONTEXT
            };
        }

        public static void SetupLaserEffect(ref MinionConfig.LaserEffect[] effects)
        {
            Log.Debuglog("SetupLaserEffect");
            Log.Assert("effects", effects);

            var effect = new MinionConfig.LaserEffect
            {
                id = LASER_EFFECT,
                animFile = "sm_snowbeam_kanim",
                anim = "loop",
                context = CONTEXT
            };

            effects = effects.AddToArray(effect);

            foreach(var item in effects)
            {
                Log.Debuglog("effect " + item.id);
            }
        }
    }
}
