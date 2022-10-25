using Database;
using SpookyPumpkinSO.Content.Foods;
using UnityEngine;

namespace SpookyPumpkinSO.Content
{
    internal class SPSpices
    {
        public static Spice PumpkinSpice;
		public const string PUMPKIN_SPICE_ID = "SP_PUMPKIN_SPICE";

		public static void Register(Spices parent)
        {
			PumpkinSpice = new Spice(parent, PUMPKIN_SPICE_ID, new []
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						PumpkinConfig.ID
					},
					AmountKG = 1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						PrickleFlowerConfig.SEED_ID
					},
					AmountKG = 1f
				}
			}, ModAssets.Colors.pumpkinOrange, Color.white, null, null, "spookypumpkin_spice_pumpkin");
		}
	}
}
