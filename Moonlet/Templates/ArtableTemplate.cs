using static Moonlet.Templates.EntityTemplates.EntityTemplate;

namespace Moonlet.Templates
{
	public class ArtableTemplate : BaseTemplate
	{
		public string BuildingId { get; set; }
		public bool Optional { get; set; }

		public string Description { get; set; }

		public AnimationEntry Animation { get; set; }

		public string Quality { get; set; }

		public int BonusDecor { get; set; }
	}
}
