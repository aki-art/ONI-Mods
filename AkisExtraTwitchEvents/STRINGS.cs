using Twitchery.Content;
using Twitchery.Content.Defs;

namespace Twitchery
{
	public class STRINGS
	{
		public static class AETE_EVENTS
		{
			public static class POLYMOPRH
			{
				public static LocString TOAST = "Turn {Dupe} into a {Critter}";
				public static LocString TOAST_ALT = "Polymorph Potion";
				public static LocString DESC = "Woosh! {Dupe} is now a {Critter}";
			}

			public static class GIANT_CRAB
			{
				public static LocString TOAST = "Spawn a regular crab";
				public static LocString DESC = "Behold: a regular crab!";
			}

			public static class EGG
			{
				public static LocString TOAST = "Egg";
				public static LocString DESC = "...";
			}

			public static class MIDAS
			{
				public static LocString TOAST = "Midas Touch";
				public static LocString DESC = "Everything I touch is gold!";
			}

			public static class JELLO_RAIN
			{
				public static LocString TOAST = "Jello Rain";
				public static LocString DESC = "Jello is falling from the sky!";
			}

			public static class COFFEE_BREAK
			{
				public static LocString TOAST = "Coffee break";
				public static LocString DESC = "All Duplicants are taking a well deserved break.";
			}

			public static class INVISIBLE_LIQUIDS
			{
				public static LocString TOAST = "Invisible Liquids";
				public static LocString DESC = "Wait, where is my water??!";
			}

			public static class DOUBLE_TROUBLE
			{
				public static LocString TOAST = "Double Trouble";
				public static LocString TITLE = "Prepare for trouble";
				public static LocString DESC = "And make it double!";
			}

			public static class PIZZA_DELIVERY
			{
				public static LocString TOAST = "Pizza Delivery";
				public static LocString DESC = "Pizza time!";
				public static LocString DESC_RECIPE = "\nAlso new recipe at Gas Range.";
			}

			public static class RAD_DISH
			{
				public static LocString TOAST = "Rad Dish";
				public static LocString DESC = "A singular radish has been spawned on {Asteroid}.";
				public static LocString DESC_RECIPE = "\nAlso new recipe at Electric Grill.";
			}

			public static class RETRO_VISION
			{
				public static LocString TOAST = "Retro Vision";
				public static LocString DESC = "Your video card has been downgraded.";
			}
		}

		public static class DUPLICANTS
		{
			public static class MODIFIERS
			{
				public static class AKISEXTRATWITCHEVENTS_CAFFEINATEDEFFECT
				{
					public static LocString NAME = "Caffeinated";
					public static LocString TOOLTIP = "This dupe had a much deserved coffe break.";
					public static LocString DESCRIPTION = "This dupe had a much deserved coffe break.";
					public static LocString ADDITIONAL_EFFECTS = "+{0} Work Speed";
				}

				public static class AKISEXTRATWITCHEVENTS_RADISHSTRENGTHEFFECT
				{
					public static LocString NAME = "Rad Strength";
					public static LocString TOOLTIP = "This Duplicant had some great radish recently.";
					public static LocString DESCRIPTION = "This Duplicant had some great radish recently.";
				}
			}
		}

		public static class MISC
		{
			public static class STATUSITEMS
			{
				public static class AKISEXTRATWITCHEVENTS_POLYMORPHSTATUS
				{
					public static LocString NAME = "Polymorphed";
					public static LocString TOOLTIP = "This is {0} is enjoying the careless life of being a {1}.\n" +
						"\n" +
						"Time remaining: {2}";
				}

				public static class AKISEXTRATWITCHEVENTS_CALORIESTATUS
				{
					public static LocString NAME = "{0}";
					public static LocString TOOLTIP = "This is {0} of raw, delightful radish.";
				}

				public static class AKISEXTRATWITCHEVENTS_DUPEDDUPESTATUS
				{
					public static LocString NAME = "Duplicant Duplicant";
					public static LocString TOOLTIP = "Expires in {0}";
				}
			}

			public static class AKISEXTRATWITCHEVENTS_PIZZABOX
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Pile of Pizzaboxes", PizzaBoxConfig.ID);
				public static LocString DESC = "Full of delicious pizza.";
			}

			public static class AKISEXTRATWITCHEVENTS_GIANTRADISH
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Radish of the Gods", GiantRadishConfig.ID);
				public static LocString DESC = "The singular radish.";
			}
		}

		public static class ITEMS
		{
			public static class FOOD
			{
				public static class AKISEXTRATWITCHEVENTS_RAWRADISH
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Chunk O' Radish", RawRadishConfig.ID);
					public static LocString DESC = "An edible chunk of the godly Radish.";
				}

				public static class AKISEXTRATWITCHEVENTS_COOKEDRADISH
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Rad Dish", CookedRadishConfig.ID);
					public static LocString DESC = "Delicious grilled radish with a radiantly green crust. It's <i>probably</i> safe.";
				}

				public static class AKISEXTRATWITCHEVENTS_PIZZA
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Pizza", PizzaConfig.ID);
					public static LocString DESC = "A wonderful, filling dish.";
				}

				// JELLO is added at ELEMENTS
			}
		}

		public static class ELEMENTS
		{
			public static class RICE
			{
				public static LocString NAME = "Rice";
				public static LocString DESC = "Tasty.";
			}

			public static class TEA
			{
				public static LocString NAME = "Tea";
				public static LocString DESC = "Tasty.";
			}

			public static class FROZENTEA
			{
				public static LocString NAME = "Ice-Tea";
				public static LocString DESC = "It is cold tea.";
			}

			public static class SOAP
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Soap", "Soap");
				public static LocString DESC = "";
			}

			public static class LIQUIDSOAP
			{
				public static LocString NAME = "Liquid Soap";
				public static LocString DESC = "";
			}

			public static class JELLO
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Jello", Elements.Jello.ToString());
				public static LocString DESC = "A jiggly, edible liquid that behaves like Visco Gel. It's kiwi flavored.\n\n" +
					"Contains 1840 kcal per kilogram. (Must be mopped first, Duplicants will not eat a liquid directly from the floor, they do have <i>some</i> standards.";
			}

			public static class FROZENJELLO
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Solid Jello", Elements.Jello.ToString());
				public static LocString DESC = "Solidified Jello. It's too cold and hardened to eat.";
			}
		}

		public static class CREATURES
		{
			public static class SPECIES
			{
				public static class AKISEXTRATWITCHEVENTS_GIANTCRAB
				{
					public static LocString NAME = "Ol' Crab Pal";
					public static LocString DESCRIPTION = "This enormous Pokeshell has lived very long, and has grown to be a truly large lad.";
				}

				public static class AKISEXTRATWITCHEVENTS_POLYMORPHCRITTER
				{
					public static LocString NAME = "Critter (Transformed)";
				}
			}
		}
	}
}
