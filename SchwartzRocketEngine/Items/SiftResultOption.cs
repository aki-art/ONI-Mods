using ProcGen;

namespace SchwartzRocketEngine.Items
{
    public class SiftResultOption : IWeighted
	{
		public Tag Tag { get; private set; }

		public float Mass { get; private set; }

		public float weight { get; set; }

		public SiftResultOption(Tag tag, float amount, float weight = 1f)
		{
			Tag = tag;
			Mass = amount;
			this.weight = weight;
		}
	}
}
