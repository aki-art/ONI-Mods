using UnityEngine;

namespace PrintingPodRecharge.Items
{
    internal class Shaker : Assignable
    {
        [MyCmpAdd]
        private ShakerWorkable workable;

        protected override void OnSpawn()
        {
            base.OnSpawn();
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
