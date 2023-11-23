using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Utils
{
	public class MobLocationUtil
	{
		public static readonly Dictionary<string, Mob.Location> Lookup = new();

		public static Mob.Location Register(string name)
		{
			var value = (Mob.Location)Hash.SDBMLower(name);
			Lookup.Add(name, value);

			return value;
		}
	}
}
