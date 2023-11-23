﻿using System;

namespace Twitchery.Content.Events
{
	public class DiscountEvent : ITwitchEvent
	{
		public const string ID = "Discount";

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{

		}
	}
}