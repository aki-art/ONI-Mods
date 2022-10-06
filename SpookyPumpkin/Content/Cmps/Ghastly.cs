using FUtility;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Cmps
{
    public class Ghastly : KMonoBehaviour, ISim1000ms
    {
        [SerializeField]
        public Color ghostlyColor = new Color(0, 24, 30); // over-reaching the color on purpose. this burnt-out clipping teal gives the effect i wanted

        [SerializeField]
        public Color transparentTint = new Color(1f, 1f, 1f, 0.4f);

        [Serialize]
        public bool isPumpkinEffectActive;

        [Serialize]
        public bool isGhastly;

        [MyCmpReq]
        public KBatchedAnimController kbac;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.EffectAdded, OnEffectAdded);
            Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);
        }

        public static void TryApplyHighlight(GameObject go, float value)
        {
            if(go.TryGetComponent(out Ghastly ghastly) && ghastly.isGhastly)
            {
                ghastly.kbac.HighlightColour += new Color(ghastly.ghostlyColor.r + value, ghastly.ghostlyColor.g + value, ghastly.ghostlyColor.b + value);
            }
        }

        private void OnEffectRemoved(object obj)
        {
            if (obj is Effect effect && effect.Id == SPSpices.PumpkinSpice.Id)
            {
                Log.Debuglog("Removed Pumpkinspice effect");
                isPumpkinEffectActive = false;
                if(isGhastly)
                {
                    TurnMaterial();
                }
            }
        }

        private void OnEffectAdded(object obj)
        {
            if (obj is Effect effect && effect.Id == SPSpices.PumpkinSpice.Id)
            {
                Log.Debuglog("Added Pumpkinspice effect");
                isPumpkinEffectActive = true;
            }
        }

        public void Sim1000ms(float dt)
        {
            if(!isPumpkinEffectActive)
            {
                return;
            }

            var isNight = true;//GameClock.Instance.IsNighttime();

            if(!isGhastly && isNight)
            {
                TurnGhost();
            }
            else if(isGhastly && !isNight)
            {
                TurnMaterial();
            }
        }

        private void TurnMaterial()
        {
            isGhastly = false;
            kbac.TintColour = Color.white;
            kbac.HighlightColour = Color.white;
        }

        private void TurnGhost()
        {
            isGhastly = true;
            kbac.TintColour = transparentTint;
            kbac.HighlightColour = ghostlyColor;
        }
    }
}
