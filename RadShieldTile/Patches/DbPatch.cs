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
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Input(SimHashes.Lead.ToString(), 100f, false)
					.Input(SimHashes.Polypropylene.ToString(), 100f, false)
					.Input(SimHashes.Copper.ToString(), 100f, false)
					.Input(SimHashes.Phosphorite.ToString(), 100f, false)
					.Output(RSTElements.RadShield.Tag, 400f)
					.Build();

				RecipeBuilder.Create(SupermaterialRefineryConfig.ID, "todo", 100f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Input(SimHashes.Lead.ToString(), 100f, false)
					.Input(SimHashes.Polypropylene.ToString(), 100f, false)
					.Input(SimHashes.Aluminum.ToString(), 100f, false)
					.Input(SimHashes.Phosphorite.ToString(), 100f, false)
					.Output(RSTElements.RadShield.Tag, 400f)
					.Build();
			}
		}
	}
}
