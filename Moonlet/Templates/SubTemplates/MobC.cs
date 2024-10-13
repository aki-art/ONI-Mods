using HarmonyLib;
using Moonlet.Utils;
using ProcGen;
using System;
using System.Linq;

namespace Moonlet.Templates.SubTemplates
{
	public class MobC : IShadowTypeBase<Mob>
	{
		public string PrefabName { get; set; }

		public MinMaxC Density { get; set; }

		public float AvoidRadius { get; set; }

		public PointGenerator.SampleBehaviour SampleBehaviour { get; set; }

		public bool DoAvoidPoints { get; set; }

		public bool DontRelaxChildren { get; set; }

		public MinMaxC BlobSize { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string Location { get; set; }

		public string InElement { get; set; }

		public MobC()
		{
			DoAvoidPoints = true;
			DontRelaxChildren = false;
			BlobSize = new MinMaxC();
			Density = new MinMaxC();
		}

		public Mob Convert(Action<string> log = null)
		{
			if (!Enum.TryParse(Location, out Mob.Location location))
			{
				if (!MobLocationUtil.Lookup.TryGetValue(Location, out location))
				{
					log($"{Location} is not a valid location. Options are: {Enum.GetNames(typeof(Mob.Location)).Join()}, {MobLocationUtil.Lookup.Values.Join()}");

					location = Mob.Location.Floor;
				}
			}

			var result = new Mob()
			{
				prefabName = PrefabName,
				width = Width,
				height = Height,
				location = location,
				doAvoidPoints = DoAvoidPoints,
				dontRelaxChildren = DontRelaxChildren,
				blobSize = BlobSize.ToMinMax(),
				density = Density.ToMinMax(),
				sampleBehaviour = SampleBehaviour,
				avoidRadius = AvoidRadius
			};

			return result;
		}
	}
}
