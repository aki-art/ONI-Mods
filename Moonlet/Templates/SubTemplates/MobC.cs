using HarmonyLib;
using Moonlet.Utils;
using ProcGen;
using System;
using System.Linq;

namespace Moonlet.Templates.SubTemplates
{
	public class MobC : ShadowTypeBase<Mob>
	{
		public string PrefabName { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string Location { get; set; }

		public string InElement { get; set; }

		public override Mob Convert(Action<string> log = null)
		{
			Log.Debug("converting " + Location);

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
			};

			return result;
		}
	}
}
