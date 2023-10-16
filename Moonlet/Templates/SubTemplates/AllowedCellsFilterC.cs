using System.Collections.Generic;

namespace Moonlet.Templates.SubTemplates
{
	public class AllowedCellsFilterC : ProcGen.World.AllowedCellsFilter
	{
		public new List<string> zoneTypes { get; set; }
	}
}
