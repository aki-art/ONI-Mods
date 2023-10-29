using ONITwitchLib;
using System.Linq;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
	public abstract class TwitchEventBase(string id)
	{
		public string id = id;

		public abstract Danger GetDanger();

		public abstract int GetWeight();

		public EventInfo EventInfo { get; private set; }

		public void SetName(string name)
		{
			if (EventInfo != null)
				EventInfo.FriendlyName = name;
		}

		public virtual void ConfigureEvent(EventInfo info)
		{
			EventInfo = info;
			EventInfo.Danger = GetDanger();
			EventInfo.AddCondition(_ => Condition());
			EventInfo.AddListener(_ => Run());

			AkisTwitchEvents.OnDrawFn += OnDraw;
		}

		public static GameObject AddCustomContainerToToast(GameObject toastGo)
		{
			var texts = toastGo.GetComponentsInChildren<LocText>();

			if (texts == null)
				return null;

			var last = texts.Last();

			var go = new GameObject("aete_image");
			go.transform.SetParent(last.transform.parent);

			go.transform.position = last.transform.position;
			go.transform.localScale = last.transform.localScale;

			if (last.TryGetComponent(out RectTransform rectTransform))
			{
				var newRect = go.AddOrGet<RectTransform>();
				newRect.anchoredPosition = rectTransform.anchoredPosition;
				newRect.anchorMin = rectTransform.anchorMin;
				newRect.anchorMax = rectTransform.anchorMax;
				newRect.localScale = rectTransform.localScale;
				newRect.position = newRect.position;
			}

			go.transform.localScale *= 0.8f;

			return go;
		}

		public virtual string GetName() => Strings.Get($"STRINGS.AETE_EVENTS.{id.ToUpperInvariant()}.TOAST");

		public virtual bool Condition() => true;

		public abstract void Run();

		public virtual void OnDraw() { }

		public virtual void OnGameLoad() { }

		public static class WEIGHTS
		{
			public const int
				COMMON = 24,
				UNCOMMON = 19,
				RARE = 10,
				VERY_RARE = 5,
				GUARANTEED = 20000;
		}
	}
}
