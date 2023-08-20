#if WIP_EVENTS
using FUtility;
using ProcGen;
using System;
using System.Collections;
using System.Reflection;

namespace Twitchery.Content
{
	public class TPocketDimensions
	{
		public static void Register()
		{
			var generatorType = Type.GetType("ONITwitch.Content.PocketDimensionGenerator, ONITwitch");
			if (generatorType == null)
			{
				Log.Warning("PocketDimensionGenerator Type not found");
				return;
			}

			var f_PocketDimensionSettings = generatorType.GetField("PocketDimensionSettings", BindingFlags.Static | BindingFlags.NonPublic);

			if (f_PocketDimensionSettings == null)
			{
				Log.Warning("PocketDimensionSettings field not found");
				return;
			}

			var PocketDimensionSettings = f_PocketDimensionSettings.GetValue(null);

			var templateGeneration = Type.GetType("ONITwitch.Content.TemplatePocketDimensionGeneration, ONITwitch");
			if (templateGeneration == null)
			{
				Log.Warning("TemplatePocketDimensionGeneration Type not found");
				return;
			}

			var list = PocketDimensionSettings as IList;

			// TODO: DLC/Vanilla
			AddDimension(
				templateGeneration,
				list,
				3f,
				SubWorld.ZoneType.Sandstone,
				"akis_extra_twitch_events/pocket_dimensions/bees");
		}

		private static void AddDimension(Type templateGeneration, IList list, float cyclesActive, SubWorld.ZoneType zoneType, string template)
		{
			var beeDimension = Activator.CreateInstance(templateGeneration, cyclesActive, zoneType, template, null, null);
			list.Add(beeDimension);
		}
	}
}
#endif