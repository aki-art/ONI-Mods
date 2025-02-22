using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class PimplesEventSmall() : PimplesEventBase(ID)
	{
		public const string ID = "Pimples";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override bool Condition() => AkisTwitchEvents.MaxDanger < Danger.Medium;

		public override int SpawnRange() => 8;

		protected override void ConfigureSpawner(GameObject go)
		{
			go.GetComponent<Pimple>().allowDangerousElements = false;
		}
	}
}
