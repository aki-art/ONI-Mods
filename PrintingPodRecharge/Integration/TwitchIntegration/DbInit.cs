using FUtility;
using Klei.AI;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class DbInit
    {
        public static StatusItem assignedStatus;

        public static void OnDbInit()
        {
            assignedStatus = new StatusItem("PrintingPodRecharge_AssignedTo", "ITEMS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);

            assignedStatus.resolveStringCallback = (str, data) =>
            {
                var assignee = ((Assignable)data).assignee;
                if (!assignee.IsNullOrDestroyed())
                {
                    var properName = assignee.GetProperName();
                    str = str.Replace("{Assignee}", properName);

                    GetMinionIdentity(assignee, out var identity, out var storedIdentity);

                    var traits = identity.GetComponent<Traits>().GetTraitIds() ?? storedIdentity.GetComponent<Traits>().GetTraitIds();

                    foreach (var trait in traits)
                    {
                        if (ModAssets.badTraits.Contains(trait))
                        {
                            str = str.Replace("{Skill}", Db.Get().traits.Get(trait).Name);
                            break;
                        }
                    }
                }

                return str;
            };

            Db.Get().DuplicantStatusItems.Add(assignedStatus);
        }

        public static void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
        {
            if (assignableIdentity is MinionAssignablesProxy)
            {
                minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
                storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
                return;
            }

            minionIdentity = (assignableIdentity as MinionIdentity);
            storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
        }

    }
}
