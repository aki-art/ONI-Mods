namespace FUtility
{
	public class ModHashes
	{
		private readonly int value;
		private readonly string name;
		private readonly GameHashes hash;

		public ModHashes(string name)
		{
			this.name = name;
			value = Hash.SDBMLower(name);
			hash = (GameHashes)value;
		}

		public static implicit operator GameHashes(ModHashes modHashes) => modHashes.hash;

		public static implicit operator int(ModHashes modHashes) => modHashes.value;

		public static implicit operator string(ModHashes modHashes) => modHashes.name;

		public override string ToString() => name;
	}
}
