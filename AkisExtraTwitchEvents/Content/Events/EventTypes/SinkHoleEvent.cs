using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class SinkHoleEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SinkHole";

		public override Danger GetDanger() => Danger.High;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override bool Condition()
		{
			return Db.Get().Techs.IsTechItemComplete(ExteriorWallConfig.ID);
		}

		public override void Run()
		{
			FUtility.Utils.Spawn(SinkHoleSpawnerConfig.ID, PosUtil.ClampedMousePosWithRange(0))
				.GetComponent<AETE_SinkHole>()
				.Begin();

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.SINKHOLE.TOAST,
				STRINGS.AETE_EVENTS.SINKHOLE.DESC);
		}
	}
}
