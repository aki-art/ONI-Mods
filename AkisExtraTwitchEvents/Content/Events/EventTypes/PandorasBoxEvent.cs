using ONITwitchLib;
using System;

namespace Twitchery.Content.Events.EventTypes
{
	public class SpawnPandorasBoxEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SpawnPandorasBox";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
