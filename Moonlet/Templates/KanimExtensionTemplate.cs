using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class KanimExtensionTemplate : BaseTemplate
	{
		public string BatchTag { get; set; }

		public List<SoundEventEntry> SoundEvents { get; set; }

		public class SoundEventEntry
		{
			public string Animation { get; set; }

			public Dictionary<int, string> Frames { get; set; }

			public IntNumber LoopMinInterval { get; set; }

			public string LoopingSoundEvent { get; set; }
		}
	}
}
