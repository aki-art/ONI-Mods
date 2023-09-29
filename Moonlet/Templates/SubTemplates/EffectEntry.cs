namespace Moonlet.Templates.SubTemplates
{
	public class EffectEntry
	{
		public string Id { get; set; }

		public bool ExclusiveWithTag { get; set; }

		public int Priority { get; set; }

		public string[] Tags { get; set; }

		public string[] RemoveEffects { get; set; }

		public string[] ImmuneToEffects { get; set; }
	}
}
