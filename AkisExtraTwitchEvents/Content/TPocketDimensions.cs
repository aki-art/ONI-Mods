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

			list.Clear();

			if (DlcManager.IsExpansion1Active())
			{
				// Bees
				AddDimension(
					templateGeneration,
					list,
					3f,
					SubWorld.ZoneType.Sandstone,
					"akis_extra_twitch_events/pocket_dimensions/bees");

				// Radbolt puzzle
				AddDimension(
					templateGeneration,
					list,
					6f,
					SubWorld.ZoneType.BoggyMarsh,
					"akis_extra_twitch_events/pocket_dimensions/radbolt puzzle v5");

				AddDimension(
					templateGeneration,
					list,
					6f,
					SubWorld.ZoneType.Space,
					"akis_extra_twitch_events/pocket_dimensions/rocket v2");
			}

			// Big snow puzzle, easy
			AddDimension(
				templateGeneration,
				list,
				6f,
				SubWorld.ZoneType.MagmaCore,
				"akis_extra_twitch_events/pocket_dimensions/magma puzzle v3");
		}

		private static void AddDimension(Type templateGeneration, IList list, float cyclesActive, SubWorld.ZoneType zoneType, string template)
		{
			var beeDimension = Activator.CreateInstance(templateGeneration, cyclesActive, zoneType, template, null, null);
			list.Add(beeDimension);
			list.Add(beeDimension); // double weight for now
		}
	}
}
