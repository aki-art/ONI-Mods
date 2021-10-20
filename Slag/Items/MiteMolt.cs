using Slag.Critter;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Items
{
    public class MiteMolt : KMonoBehaviour, IEffectDescriptor, IGameObjectEffectDescriptor
    {
        public const string ID = "MiteMolt";
        [SerializeField]
        public MiteTuning.MoltTier tier;

        public List<Descriptor> GetEffectDescriptions()
        {
            return new List<Descriptor>() {
                new Descriptor(
                    tier.ToString() + "molt shed by a mite.",
                    "This is the tooltip string",
                    Descriptor.DescriptorType.Information,
                    false )
            };
        }
        public List<Descriptor> GetDescriptors(GameObject go)
        {
            return GetEffectDescriptions();
        }

        public List<Descriptor> GetDescriptors(BuildingDef def)
        {
            return GetEffectDescriptions();
        }
    }
}
