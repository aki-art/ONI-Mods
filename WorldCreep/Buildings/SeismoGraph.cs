using UnityEngine;
using WorldCreep.WorldEvents;

namespace WorldCreep.Buildings
{
    public class SeismoGraph : KMonoBehaviour
    {
        private static Notifier notifier;
        public float range;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            WorldEventScheduler.Instance.Subscribe((int)WorldEventHashes.EventScheduled, OnEventScheduled);
        }

        private void OnEventScheduled(object obj)
        {
            var worldEvent = obj as WorldEvent;
            if (IsInRange(worldEvent))
                Notify(worldEvent);
        }

        private bool IsInRange(Component target) => Vector2.Distance(transform.position, target.transform.position) <= range;

        private static void Notify(WorldEvent worldEvent)
        {
            var notification = new Notification(
                title: worldEvent.PrefabID().ProperNameStripLink() + " coming!",
                type: NotificationType.Bad,
                group: HashedString.Invalid,
                click_focus: worldEvent.transform);

            notifier = worldEvent.gameObject.AddComponent<Notifier>();
            notifier.Add(notification);
        }
    }
}
