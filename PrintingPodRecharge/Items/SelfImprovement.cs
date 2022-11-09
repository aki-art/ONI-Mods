using Klei.AI;
using PrintingPodRecharge.Content;
using System.Linq;

namespace PrintingPodRecharge.Items
{
    public class SelfImprovement : Ownable
    {
        [MyCmpAdd]
        private SelfImprovementWorkable workable;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            slotID = PAssignableSlots.Book.Id;
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
    }
}
