using ONITwitchLib;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public abstract class EventBase(string id)
	{
		public readonly string id = id;

		public EventInfo eventInfo;

		public abstract Danger GetDanger();

		public virtual int GetWeight() => Weights.COMMON;

		public abstract void Run();

		public abstract int GetNiceness();

		public static class Intent
		{
			public const int
				EVIL = -1,
				NEUTRAL = 0,
				GOOD = 1;
		}

		public virtual string GetName() => Strings.Get($"STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.{id.ToUpperInvariant()}.NAME");

		public virtual void Initialize()
		{
		}

		public virtual bool Condition() => true;

		public void SetEventName(string name)
		{
			if (eventInfo != null)
				eventInfo.FriendlyName = name;
		}
		public class Weights
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
