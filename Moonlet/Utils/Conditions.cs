using System.Collections.Generic;

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
