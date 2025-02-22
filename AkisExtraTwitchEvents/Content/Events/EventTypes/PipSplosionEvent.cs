using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class PipSplosionEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Pipsplosion";

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.High;

		public override void Run()
		{
			var splosion = new GameObject("Pipsploder").AddComponent<PipSplosionSpawner>();
			splosion.minPips = 10;
			splosion.maxPips = 15;

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.PIPSPLOSION.TOAST, STRINGS.AETE_EVENTS.PIPSPLOSION.DESC);
		}
	}
}
