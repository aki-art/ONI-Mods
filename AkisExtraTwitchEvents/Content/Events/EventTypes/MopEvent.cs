using ONITwitchLib;

namespace Twitchery.Content.Events.EventTypes
{
	public class MopEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Mop";

		public override bool Condition() => Mod.moppables.Count > 20;

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var activeWorld = ClusterManager.Instance.activeWorld;
			var singleAsteroid = activeWorld != null && Mod.moppables.Count <= 10;

			var moppables = singleAsteroid ? Mod.moppables.GetWorldItems(activeWorld.id) : Mod.moppables.Items;

			if (moppables.Count == 0)
			{
				ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.MOP.TOAST, STRINGS.AETE_EVENTS.MOP.DESC_NO_MOPS);
				return;
			}

			string msg = singleAsteroid
				? STRINGS.AETE_EVENTS.MOP.DESC_ASTEROID.Replace("{Asteroid}", activeWorld.GetProperName())
				: STRINGS.AETE_EVENTS.MOP.DESC_GLOBAL;

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.MOP.TOAST, msg);

			foreach (var moppable in moppables)
			{
				moppable.MopTick(1000000f);
			}
		}
	}
}
