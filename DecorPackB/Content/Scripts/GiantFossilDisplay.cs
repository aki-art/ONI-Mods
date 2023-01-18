using Database;
using Klei.AI;

namespace DecorPackB.Content.Scripts
{
    public class GiantFossilDisplay : KMonoBehaviour
    {
        [MyCmpReq]
        private GiantExhibition exhibition;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.NewDay, RefreshEffect);
            exhibition.Subscribe((int)ModHashes.FossilStageSet, RefreshEffect);
        }

        private void ApplyEffectToDupes(bool emote)
        {
            var worldId = this.GetMyWorldId();

            foreach (MinionIdentity identity in Components.LiveMinionIdentities)
            {
                if (identity is null || identity.GetMyWorldId() != worldId)
                {
                    continue;
                }

                if (emote)
                {
                    identity.GetComponent<Facing>().Face(transform.position.x);
                }

                var effects = identity.GetComponent<Effects>();

                if (effects.HasEffect(DPEffects.INSPIRED_GIANT))
                {
                    effects.Remove(DPEffects.INSPIRED_GIANT);
                }

                effects.Add(DPEffects.INSPIRED_GIANT, false);

                Debug.Log("Applied effect to " + identity.GetProperName());
            }
        }

        private void RefreshEffect(object obj)
        {
            if (obj is ArtableStatuses.ArtableStatusType stage)
            {
                if (stage != ArtableStatuses.ArtableStatusType.AwaitingArting)
                {
                    ApplyEffectToDupes(true);
                }
            }
        }
    }
}
