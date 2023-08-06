using Database;
using Klei.AI;
using UnityEngine;

namespace Twitchery.Content
{
	public class TSpices
	{
		public static Spice goldFlake;
		public const string GOLD_FLAKE = "AkisExtraTwitchEvents_Spice_GoldFlake";

		public static void Register(Spices spices)
		{
/*			goldFlake = new Spice(
				spices, 
				GOLD_FLAKE,
				new[]
				{
					new Spice.Ingredient
					{
						IngredientSet = new Tag[]
						{
							SimHashes.Gold.CreateTag()
						},
						AmountKG = 5f
					}
				},
				ModAssets.Colors.gold,
				Color.white,
				new AttributeModifier(Db.Get().Attributes.FoodQuality.Id, 1),
				null,
				"akisextratwitchevents_spice_goldflake");*/
		}
	}
}
