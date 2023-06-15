using FUtility;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingPodRecharge.Content.Items.BookI
{
    public class BookVolumeI : SelfImprovement
    {
        private static readonly Dictionary<string, Type> componentTraitsById = new Dictionary<string, Type>()
        {
            { "Stinky", typeof(Stinky) },
            { "Liam", typeof(Stinky) },
            { "Flatulence", typeof(Flatulence) },
            { "Snorer", typeof(Snorer) },
            { "Narcolepsy", typeof(Narcolepsy) },
            { "Thriver", typeof(Thriver) },
            { "Loner", typeof(Loner) },
            { "StarryEyed", typeof(StarryEyed) },
            { "GlowStick", typeof(GlowStick) },
            { "RadiationEater", typeof(RadiationEater) },
            { "EarlyBird", typeof(EarlyBird) },
            { "NightOwl", typeof(NightOwl) },
            { "Claustrophobic", typeof(Claustrophobic) },
            { "PrefersWarmer", typeof(PrefersWarmer) },
            { "PrefersColder", typeof(PrefersColder) },
            { "SensitiveFeet", typeof(SensitiveFeet) },
            { "Fashionable", typeof(Fashionable) },
            { "Climacophobic", typeof(Climacophobic) },
            { "SolitarySleeper", typeof(SolitarySleeper) },
            { "Workaholic", typeof(Workaholic) }
        };

        public override bool CanUse(MinionIdentity minionIdentity)
        {
            var traits = minionIdentity.GetComponent<Traits>().GetTraitIds();
            return traits.Any(t => ModAssets.badTraits.Contains(t));
        }

        public override void OnUse(Worker worker)
        {
            var traits = worker.GetComponent<Traits>();

            var badTrait = traits.TraitList.Find(t => !t.PositiveTrait && ModAssets.badTraits.Contains(t.Id));
            if (badTrait != null)
            {
                if (componentTraitsById.TryGetValue(badTrait.Id, out var componentType))
                {
                    var component = worker.GetComponent(componentType);
                    Destroy(component);
                }
                traits.Remove(badTrait);
            }
        }

        public override string GetStatusString(IAssignableIdentity minionIdentity)
        {
            var str = (string)STRINGS.ITEMS.STATUSITEMS.PRINTINGPODRECHARGE_ASSIGNEDTO.NAME;

			Log.Debuglog($"0 {str}");
			Log.Debuglog($"name {minionIdentity.GetProperName()}");
			str = str.Replace("{Assignee}", minionIdentity.GetProperName());

            Log.Debuglog($"1 {str}");

            GetMinionIdentity(assignee, out var identity, out var storedIdentity);

            List<string> traits = null;

            if (identity != null)
                traits = identity.GetComponent<Traits>().GetTraitIds();
            else if (storedIdentity != null)
                traits = storedIdentity.GetComponent<Traits>().GetTraitIds();

            if (traits == null)
                return str;

            foreach (var trait in traits)
            {
                if (ModAssets.badTraits.Contains(trait))
                {
                    str = str.Replace("{Data}", Db.Get().traits.Get(trait).Name);
					Log.Debuglog($"2 {str}");
					break;
                }
            }

			Log.Debuglog(str);
			return str;
        }
    }
}
