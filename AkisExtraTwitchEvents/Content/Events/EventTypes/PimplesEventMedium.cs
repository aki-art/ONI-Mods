using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class PimplesEventMedium() : PimplesEventBase(ID)
	{
		public const string ID = "PimplesMedium";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override bool Condition() => AkisTwitchEvents.MaxDanger >= Danger.Medium;

		public override int SpawnRange() => 8;

		protected override void ConfigureSpawner(GameObject go)
		{
			go.GetComponent<Pimple>().allowDangerousElements = true;
		}
	}
}
