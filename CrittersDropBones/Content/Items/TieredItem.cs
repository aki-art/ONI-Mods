using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrittersDropBones.Items
{
    // Responsible to update the bone artwork based on it's size tier
    public class TieredItem : KMonoBehaviour
    {
        [MyCmpReq]
        private EntitySplitter entitySplitter;

        [MyCmpReq]
        private PrimaryElement primaryElement;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [SerializeField]
        public List<Tier> tiers;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            entitySplitter.Subscribe((int)GameHashes.SplitFromChunk, obj => UpdateTier());
            entitySplitter.Subscribe((int)GameHashes.Absorb, obj => UpdateTier());
            UpdateTier();
        }

        private void UpdateTier()
        {
            var tier = GetTierForMass(primaryElement.Mass);
            kbac.Play(tier.anim);
        }

        private Tier GetTierForMass(float mass)
        {
            return tiers.FindLast(tier => tier.minimumMass <= mass);
        }

        [Serializable]
        public class Tier
        {
            [SerializeField]
            public float minimumMass = 0f;

            [SerializeField]
            public string anim = "object";
        }
    }
}
