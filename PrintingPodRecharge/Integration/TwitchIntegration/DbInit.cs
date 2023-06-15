using PrintingPodRecharge.Content.Items.BookI;

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
                var selfImprovement = data as SelfImprovement;
                if (selfImprovement != null && !selfImprovement.assignee.IsNullOrDestroyed())
                    str = selfImprovement.GetStatusString(selfImprovement.assignee);

                return str;
            };

            Db.Get().DuplicantStatusItems.Add(assignedStatus);
        }
    }
}
