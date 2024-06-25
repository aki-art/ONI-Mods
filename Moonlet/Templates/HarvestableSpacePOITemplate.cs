using Moonlet.Templates.SubTemplates;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class HarvestableSpacePOITemplate : SpacePOITemplateBase
	{
		public MinMaxC Capacity { get; set; }

		public MinMaxC Recharge { get; set; }

		public bool CanProvideArtifact { get; set; }

		public string ArtifaceProvider { get; set; }

		public List<string> OrbitalBehavior { get; set; }

		public int MaxOrbitingObjects { get; set; }

		public List<ElementEntry> Elements { get; set; }

		public HarvestableSpacePOITemplate()
		{
			ArtifaceProvider = "HarvestablePOIArtifacts";
			OrbitalBehavior =
			[
				"iceRock",
				"rocky",
				"frozenOre"
			];
		}

		[Serializable]
		public class ElementEntry
		{
			public string Element { get; set; }

			public FloatNumber Weight { get; set; }

			public bool Optional { get; set; }
		}
	}
}
