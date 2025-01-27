using System.Collections.Generic;
using System.Linq;

namespace Moonlet.Utils
{
	public class Conditions : Dictionary<string, object>
	{
		public string[] DlcIds { get; set; } // defaults to all available if not defined

		public string[] ForbiddenDlcIds { get; set; }

		public string[] Mods { get; set; }

		public string[] ForbiddenMods { get; set; }

		public string[] ClusterTags { get; set; }

		public Conditions()
		{
			DlcIds = [];
			Mods = [];
			ForbiddenDlcIds = [];
			ForbiddenMods = [];
		}

		public bool IsValid(List<string> clusterTags = null)
		{
			if (!DlcManager.IsDlcListValidForCurrentContent(DlcIds))
				return false;

			foreach (var dlc in DlcManager.GetActiveDLCIds())
			{
				if (ForbiddenDlcIds.Contains(dlc))
					return false;
			}

			if (clusterTags != null && !clusterTags.Intersect(ClusterTags).Any())
				return false;

			foreach (var mod in Mods)
			{
				if (!Mod.loadedModIds.Contains(mod))
					return false;
			}

			return true;
		}

		/*		public bool GetBool(string key)
				{
					result = default;

					if (TryGetValue(key, out var objResult))
					{
						if (typeof(T).IsAssignableFrom(objResult.GetType()))
						return (T)(object)objResult;
					}
				}*/
	}
}
