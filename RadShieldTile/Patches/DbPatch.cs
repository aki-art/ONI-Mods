using FUtility;
using HarmonyLib;
using RadShieldTile.Content;

namespace RadShieldTile.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				RecipeBuilder.Create(SupermaterialRefineryConfig.ID, "todo", 100f)
					.Input(SimHashes.Lead.ToString(), 100f)
					.Input(SimHashes.Polypropylene.ToString(), 100f)
					.Input(SimHashes.Copper.ToString(), 100f)
					.Input(SimHashes.Phosphorite.ToString(), 100f)
					.Output(RSTElements.RadShield.Tag, 400f)
					.Build();

				RecipeBuilder.Create(SupermaterialRefineryConfig.ID, "todo", 100f)
					.Input(SimHashes.Lead.ToString(), 100f)
					.Input(SimHashes.Polypropylene.ToString(), 100f)
					.Input(SimHashes.Aluminum.ToString(), 100f)
					.Input(SimHashes.Phosphorite.ToString(), 100f)
					.Output(RSTElements.RadShield.Tag, 400f)
					.Build();
			}
		}
	}
}
