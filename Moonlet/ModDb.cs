using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlet
{
	public class ModDb
	{
		public static class BuildingCategories
		{
			public const string POIS = "Moonlet_Pois";

			public static void Register()
			{
				var buildingIDs = new List<string>()
				{
					RocketWallTileConfig.ID,
					GravitasPedestalConfig.ID,
					GravitasDoorConfig.ID,
					GravitasLabLightConfig.ID,
					GravitasContainerConfig.ID,
					TilePOIConfig.ID
				};

				// add a category to put the stuff in
				// calling without subcategory throws them in "uncategorized"
				var planInfo = new PlanScreen.PlanInfo(POIS, false, new List<string>());
				TUNING.BUILDINGS.PLANORDER.Add(planInfo);

				foreach(var item in buildingIDs)
				{
					ModUtil.AddBuildingToPlanScreen(POIS, item);
				}
			}
		}

		// these would be bad to be enabled
		public static HashSet<string> disabled = new()
		{
			// OP
			HeadquartersConfig.ID,
			TemporalTearOpenerConfig.ID,
			GravitasPedestalConfig.ID,

			// DLC content not disabled in vanilla
			RocketEnvelopeWindowTileConfig.ID,

			// rocket interior stuff
			RocketInteriorGasInputConfig.ID,
			RocketInteriorGasInputPortConfig.ID,
			RocketInteriorGasOutputConfig.ID,
			RocketInteriorGasOutputPortConfig.ID,
			RocketInteriorPowerPlugConfig.ID,
			RocketInteriorSolidInputConfig.ID,
			RocketInteriorSolidOutputConfig.ID,
			RocketInteriorLiquidInputConfig.ID,
			RocketInteriorLiquidInputPortConfig.ID,
			RocketInteriorLiquidOutputConfig.ID,
			RocketInteriorLiquidOutputPortConfig.ID,

			// unimplemented / unstable / what even are some of these? why is there an EGG?
			CrewCapsuleConfig.ID,
			WarpPortalConfig.ID,
			AtmoicGardenConfig.ID,
			TeleportalPadConfig.ID,
			StaterpillarGeneratorConfig.ID
		};
	}
}
