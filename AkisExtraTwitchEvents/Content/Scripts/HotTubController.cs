using ONITwitchLib;
using System.Collections;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class HotTubController : KMonoBehaviour
	{
		[SerializeField] public float durationSeconds;
		public float elapsedTime;

		public override void OnSpawn()
		{
			base.OnSpawn();
			AkisTwitchEvents.Instance.hotTubActive = true;
			elapsedTime = 0;

			StartCoroutine(BeginCountDown());

			var lava = Strings.Get("STRINGS.ONITWITCH.EVENTS.FLOOD_LAVA")?.String;
			ToastManager.InstantiateToast(lava, $"{lava}!!!");

			var scalding = $"{(global::STRINGS.CREATURES.STATUSITEMS.SCALDING.NAME)}!";

			var notifier = Game.Instance.FindOrAdd<Notifier>();

			notifier.Add(new Notification(scalding, NotificationType.DuplicantThreatening));
			notifier.Add(new Notification(scalding + " ", NotificationType.DuplicantThreatening));
			notifier.Add(new Notification(scalding + "  ", NotificationType.DuplicantThreatening));
			notifier.Add(new Notification(STRINGS.AETE_EVENTS.HOTTUB.HOT, NotificationType.DuplicantThreatening));
			notifier.Add(new Notification(STRINGS.AETE_EVENTS.HOTTUB.SPICY, NotificationType.DuplicantThreatening));
		}

		private IEnumerator BeginCountDown()
		{
			while (elapsedTime < durationSeconds)
			{
				elapsedTime += Time.deltaTime;
				yield return new WaitForSecondsRealtime(0.1f);
			}

			Util.KDestroyGameObject(gameObject);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			AkisTwitchEvents.Instance.hotTubActive = false;
			ToastManager.InstantiateToast("", STRINGS.AETE_EVENTS.HOTTUB.JUST_KIDDING);
		}
	}
}
