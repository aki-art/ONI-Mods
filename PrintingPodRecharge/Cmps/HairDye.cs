using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
{
    public class HairDye : KMonoBehaviour
    {
        public static Dictionary<MinionStartingStats, Color> rolledHairs = new Dictionary<MinionStartingStats, Color>();

        [SerializeField]
        [Serialize]
        public Color hairColor;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [Serialize]
        public bool dyedHair;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (dyedHair)
            {
                TintHair(kbac, hairColor);
            }
        }

        public static bool Apply(KMonoBehaviour dupe, KBatchedAnimController kbac = null)
        {
            if(dupe != null && dupe.TryGetComponent(out HairDye dye) && dye.dyedHair)
            {
                kbac = kbac ?? dupe.GetComponent<KBatchedAnimController>();
                if (kbac == null)
                {
                    return false;
                }

                TintHair(kbac, dye.hairColor);

                return true;
            }

            return false;
        }

        public static void TintHair(KBatchedAnimController kbac, Color color)
        {
            kbac.SetSymbolTint("snapto_hair", color);
            kbac.SetSymbolTint("snapto_hair_always", color);
            kbac.SetSymbolTint("snapto_hat_hair", color);
        }
    }
}
