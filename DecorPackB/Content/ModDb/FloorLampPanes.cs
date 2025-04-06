using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Content.ModDb
{
	public class FloorLampPanes : ResourceSet<FloorLampPane>
	{
		public static Dictionary<string, Color> entries = new()
		{
			{ SimHashes.Glass.ToString(), new Color(2.0f, 1.5f, 0.7f)},
			{ SimHashes.SedimentaryRock.ToString(), Util.ColorFromHex("20dced")},
			{ SimHashes.Salt.ToString(), Util.ColorFromHex("d65f5c")},
		};

		public FloorLampPanes()
		{
			foreach (var entry in entries)
				Add(FloorLampPane.FromElement(entry.Key, entry.Value));
		}
	}
}
