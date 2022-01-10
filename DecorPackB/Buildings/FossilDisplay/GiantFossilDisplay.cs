using Klei.AI;
using System.Collections.Generic;
using static DecorPackB.STRINGS.BUILDINGS.PREFABS;

namespace DecorPackB.Buildings.FossilDisplay
{
    public class GiantFossilDisplay : KMonoBehaviour, IExhibition
    {
        [MyCmpGet]
        private Assemblable assemblable;

        private static readonly Dictionary<string, string> descriptions = new Dictionary<string, string>()
        {
            { "Default", "..." },
            { "T-Rex", DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.TREX.DESC },
            { "Deerclops", DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.DEERCLOPS.DESC },
            { "Para", DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.DEERCLOPS.DESC },
            { "Pterodactyl", DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.PTERODACTYL.DESC },
            { "Pugalisk", DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.PUGALISK.DESC },
            { "Livyatan ", DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.LIVYATAN.DESC },
        };

        public string GetDescription()
        {
            string flavorText = descriptions.TryGetValue(GetComponent<Assemblable>().CurrentStage, out string desc) ? desc : "";
            return flavorText + "\n\n" + DECORPACKB_FOSSILDISPLAY.ASSEMBLEDBY.Replace("{duplicantName}", assemblable.assemblerName);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.NewDay, RefreshEffect);
            assemblable.Subscribe((int)GameHashes.WorkableCompleteWork, RefreshEffect);
        }

        private void ApplyEffectToDupes(bool emote)
        {
            var worldId = this.GetMyWorldId();

            foreach(MinionIdentity identity in Components.LiveMinionIdentities )
            {
                if (identity is null || identity.GetMyWorldId() != worldId)
                {
                    continue;
                }

                if (emote) { 
                    identity.GetComponent<Facing>().Face(transform.position.x);
                }

                Effects effects = identity.GetComponent<Effects>();

                if(effects.HasEffect(ModAssets.Effects.INSPIRED_GIANT)) {
                    effects.Remove(ModAssets.Effects.INSPIRED_GIANT);
                }

                effects.Add(ModAssets.Effects.INSPIRED_GIANT, false);

                Debug.Log("Applied effect to " + identity.GetProperName());
            }
        }

        private void RefreshEffect(object obj)
        {
            if(assemblable.CurrentStage == Assemblable.DEFAULT_STAGE_ID)
            {
                return;
            }

            ApplyEffectToDupes(true);
        }
    }
}
