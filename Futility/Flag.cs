namespace FUtility
{
	public class Flag
	{
		private int value;

		public Flag(int value = 0)
		{
			this.value = value;
		}

		public bool Has(int flag) => (value & flag) == flag;

		public bool SetValue(int flag) => value == flag;

		public bool Clear() => value == 0;

		public void Set(int flag) => value |= flag;

		public void UnSet(int flag) => value &= ~flag;

		public void Toggle(int flag) => value ^= flag;

		public static implicit operator int(Flag flag) => flag.value;

		public static implicit operator Flag(int intValue) => new Flag(intValue);
	}
}
