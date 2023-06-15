using PrintingPodRecharge.Content;
using PrintingPodRecharge.Integration.TwitchIntegration;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.BookI
{
    public class SelfImprovement : Assignable, IGameObjectEffectDescriptor
    {
        [MyCmpAdd]
        private SelfImprovementWorkable2 workable;

        [MyCmpGet]
        private KSelectable kSelectable;

        [SerializeField]
        public string workableAnim = "anim_react_thumbsup_kanim";

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            slotID = PAssignableSlots.BOOK_ID;
            canBePublic = false;
        }

        public virtual void OnUse(Worker worker)
        {

        }

        public virtual bool CanUse(MinionIdentity minionIdentity)
        {
            return true;
        }

        public virtual string GetStatusString(IAssignableIdentity minionIdentity)
        {
            return global::STRINGS.BUILDING.STATUSITEMS.ASSIGNEDTO.NAME.Replace("{Assignee}", minionIdentity.GetProperName());
        }

        protected void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
        {
            if (assignableIdentity is MinionAssignablesProxy proxy)
            {
                minionIdentity = proxy.GetTargetGameObject().GetComponent<MinionIdentity>();
                storedMinionIdentity = proxy.GetTargetGameObject().GetComponent<StoredMinionIdentity>();
                return;
            }

            minionIdentity = assignableIdentity as MinionIdentity;
            storedMinionIdentity = assignableIdentity as StoredMinionIdentity;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            AddAssignPrecondition(proxy =>
            {
                var minionIdentity = proxy.target as MinionIdentity;
                if (minionIdentity != null)
                    return CanUse(minionIdentity);

                return false;
            });

            OnAssign += UpdateStatusString;
            UpdateStatusString(null);

            workable.overrideAnims = new[]
            {
                Assets.GetAnim(workableAnim)
            };
        }

        public override void Assign(IAssignableIdentity new_assignee)
        {
            if (new_assignee == assignee)
                return;

            if (new_assignee is MinionIdentity identity)
                new_assignee = identity.assignableProxy.Get();

            base.Assign(new_assignee);
        }

        private void UpdateStatusString(IAssignableIdentity assignables)
        {
            if (kSelectable == null)
                return;

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
