using System.Collections.Generic;

namespace Twitchery.Content.Defs
{
	public class TGeyserConfigs
	{
		public const string
			PIPSER = "AkisExtraTwitchEvents_Pipser",
			MOLTEN_GLASS_VOLCANO = "AkisExtraTwitchEvents_MoltenGlassGeyser",
			GOOP_GEYSER = "AkisExtraTwitchEvents_GoopGeyser",
			HONEY_GEYSER = "AkisExtraTwitchEvents_HoneyGeyser",
			JELLO_GEYSER = "AkisExtraTwitchEvents_JelloGeyser",
			JAM_GEYSER = "AkisExtraTwitchEvents_RaspberryJamGeyser",
			CREEPER_GEYSER = "AkisExtraTwitchEvents_CreeperGeyser";

		public const string PIPSER_TYPE = "AkisExtraTwitchEvents_PipserType";

		public static void GenerateConfigs(List<GeyserGenericConfig.GeyserPrefabParams> list)
		{
			if (list == null)
			{
				Log.Warning("geyser configs list is null");
				return;
			}
			new GeyserConfigurator.GeyserType(
					PIPSER_TYPE,
					SimHashes.Creature,
					GeyserConfigurator.GeyserShape.Liquid,
					GameUtil.GetTemperatureConvertedToKelvin(38, GameUtil.TemperatureUnit.Celsius),
					100,
					200,
					500f,
					null,
					null,
					// eruption period
					60,
					1140,
					0.1f,
					0.9f,
					// active period
					5 * 600,
					15 * 600,
					0.05f,
					0.1f);

			list.Add(new GeyserGenericConfig.GeyserPrefabParams(
				"aete_glass_volcano_kanim",
				3,
				3,
				new GeyserConfigurator.GeyserType(
					MOLTEN_GLASS_VOLCANO,
					SimHashes.MoltenGlass,
					GeyserConfigurator.GeyserShape.Liquid,
					GameUtil.GetTemperatureConvertedToKelvin(1750, GameUtil.TemperatureUnit.Celsius),
					1000f,
					2000f,
					500f,
					geyserTemperature: 263f,
					requiredDlcIds: null),
				false));

			list.Add(new GeyserGenericConfig.GeyserPrefabParams(
				"aete_goop_volcano_kanim",
				4,
				2,
				new GeyserConfigurator.GeyserType(
					GOOP_GEYSER,
					Elements.PinkSlime,
					GeyserConfigurator.GeyserShape.Liquid,
					263.15f,
					1000f,
					2000f,
					500f,
					geyserTemperature: 263f,
					requiredDlcIds: null),
				false));
		}
	}
}
