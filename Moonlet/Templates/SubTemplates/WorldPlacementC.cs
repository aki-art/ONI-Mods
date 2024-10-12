extern alias YamlDotNetButNew;

using Moonlet.Utils;
using ProcGen;
using System;
using static ProcGen.WorldPlacement;

namespace Moonlet.Templates.SubTemplates
{
	public class WorldPlacementC : IShadowTypeBase<WorldPlacement>
	{
		/*		public class WorldMixing
				{
					public List<string> RequiredTags { get; set; }

					public List<string> ForbiddenTags { get; set; }

					public List<ProcGen.World.TemplateSpawnRules> AdditionalWorldTemplateRules { get; set; }

					public List<ProcGen.World.AllowedCellsFilter> AdditionalUnknownCellFilters { get; set; }

					public List<WeightedSubworldName> AdditionalSubworldFiles { get; set; }

					public List<string> AdditionalSeasons { get; set; }

					public WorldMixing()
					{
						RequiredTags = [];
						ForbiddenTags = [];
						AdditionalWorldTemplateRules = [];
						AdditionalUnknownCellFilters = [];
						AdditionalSubworldFiles = [];
						AdditionalSeasons = [];
					}
				}*/

		public string World { get; set; }

		//[YamlMember(Alias = "worldMixing")]
		public WorldMixing WorldMixing { get; set; }

		public MinMaxI AllowedRings { get; set; }

		public int Buffer { get; set; }

		public string LocationType { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public bool StartWorld { get; set; }

		public WorldPlacementC()
		{
			AllowedRings = new MinMaxI(0, 9999);
			Buffer = 2;
			LocationType = WorldPlacement.LocationType.Cluster.ToString();
			WorldMixing = new WorldMixing();
		}

		public WorldPlacement Convert(Action<string> log = null)
		{
			Log.Debug($"loading worldplacement rule with locationtype: {LocationType} -> {EnumUtils.ParseOrHash<LocationType>(LocationType)}");
			return new WorldPlacement()
			{
				startWorld = StartWorld,
				width = Width,
				height = Height,
				x = X,
				y = Y,
				locationType = EnumUtils.ParseOrHash<LocationType>(LocationType),
				buffer = Buffer,
				allowedRings = AllowedRings,
				worldMixing = WorldMixing,
				world = World
			};
		}
	}
}
