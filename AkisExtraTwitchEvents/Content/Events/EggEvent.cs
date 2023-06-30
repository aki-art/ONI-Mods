﻿using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	public class EggEvent : ITwitchEvent
	{
		public const string ID = "Egg";

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			AkisTwitchEvents.Instance.eggFx.Activate();
		}
	}
}
