using ProcGen;
using System.Collections.Generic;

namespace DecorPackB.Content.ModDb
{
	public class ModDb
	{
		public static FloorLampPanes FloorLampPanes { get; set; }
		public static BigFossilVariants BigFossils { get; set; }

		public static Dictionary<SimHashes, List<IWeighted>> treasureHunterLoottable = new Dictionary<SimHashes, List<IWeighted>>()
		{

		};

		public static void PostDbInit(global::Db __instance)
		{
			FloorLampPanes = new FloorLampPanes();
			BigFossils = new BigFossilVariants();
			DPStatusItems.Register(__instance.BuildingStatusItems);
		}

		public static class BuildLocationRules
		{
			public static BuildLocationRule OnAnyWall = (BuildLocationRule)(-1569291063);
			public static BuildLocationRule GiantFossilRule = (BuildLocationRule)Hash.SDBMLower("DecorPackB_FloorOrHanging");
		}
	}
}
