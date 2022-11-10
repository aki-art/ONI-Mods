using Klei.AI;
using PrintingPodRecharge.Content;
using PrintingPodRecharge.Integration.TwitchIntegration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge.Items
{
    public class SelfImprovement : Assignable, IGameObjectEffectDescriptor
    {
        [MyCmpAdd]
        private SelfImprovementWorkable workable;

        [MyCmpGet]
        private KSelectable kSelectable;


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            slotID = PAssignableSlots.BOOK_ID;

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            AddAssignPrecondition(proxy =>
            {
                var minionIdentity = proxy.target as MinionIdentity;
                if (minionIdentity != null)
                {
                    var traits = minionIdentity.GetComponent<Traits>().GetTraitIds();
                    return traits.Any(t => ModAssets.badTraits.Contains(t));    
                }

                return false;
            });

            OnAssign += UpdateStatusString;
            UpdateStatusString(null);
        }

        public override void Assign(IAssignableIdentity new_assignee)
        {
            if (new_assignee == assignee)
            {
                return;
            }

            if (new_assignee is MinionIdentity identity)
            {
                new_assignee = identity.assignableProxy.Get();
            }

            base.Assign(new_assignee);
        }

        private void UpdateStatusString(IAssignableIdentity assignables)
        {
            if (kSelectable == null)
            {
                return;
            }

            var status_item = assignee != null ? DbInit.assignedStatus : Db.Get().BuildingStatusItems.Unassigned;
            kSelectable.SetStatusItem(Db.Get().StatusItemCategories.Ownable, status_item, this);
        }

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            var item = new Descriptor();
            item.SetupDescriptor(global::STRINGS.UI.BUILDINGEFFECTS.ASSIGNEDDUPLICANT, global::STRINGS.UI.BUILDINGEFFECTS.TOOLTIPS.ASSIGNEDDUPLICANT, Descriptor.DescriptorType.Requirement);

            return new List<Descriptor>() { item };
        }
    }
}
