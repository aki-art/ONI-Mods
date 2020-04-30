using Slag.Critter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Slag.Items
{
    public class MiteMolt : KMonoBehaviour, IEffectDescriptor, IGameObjectEffectDescriptor
    {
        public const string ID = "MiteMolt";
        [SerializeField]
        private string ui_anim;
        public List<WeightedMetalOption> Rewards { get; set; }

        public void SetRewards(List<WeightedMetalOption> rewards)
        {
        }
        public void SetUIAnim(string anim)
        {
            ui_anim = anim;
        }
        public List<Descriptor> GetEffectDescriptions()
        {
            return new List<Descriptor>() {
                new Descriptor(
                    "Molt shed by a mite.",
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
