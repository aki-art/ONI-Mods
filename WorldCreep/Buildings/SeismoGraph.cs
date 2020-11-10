using WorldCreep.WorldEvents;

namespace WorldCreep.Buildings
{
    public class SeismoGraph : KMonoBehaviour
    {
        private static Notifier notifier;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            WorldEventScheduler.Instance.Subscribe((int)WorldEventHashes.EventScheduled, OnEventScheduled);
        }

        private void OnEventScheduled(object obj)
        {
            var worldEvent = obj as WorldEvent;
            var notification = new Notification(
                title: worldEvent.PrefabID().ProperNameStripLink() + " coming!",
                type: NotificationType.Bad,
                group: HashedString.Invalid,
                tooltip: null,
                tooltip_data: null,
                expires: true,
                delay: 0f,
                custom_click_callback: null,
                custom_click_data: null,
                click_focus: worldEvent.transform);

            notifier = worldEvent.gameObject.AddComponent<Notifier>();
            notifier.Add(notification); ;
        }
    }
}
