namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public abstract class HiddenEventBase(string id) : EventBase(id)
	{
		public override bool Condition() => false;
	}
}
