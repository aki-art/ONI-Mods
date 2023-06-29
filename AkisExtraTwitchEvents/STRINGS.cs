using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;

namespace Twitchery
{
    public class STRINGS
	{
		public static class AETE_CONFIG
		{
			public static class DOUBLE_TROUBLE
			{
				public static class MAX_DUPES
				{
					public static LocString LABEL = "Max. Duplicants";
					public static LocString TOOLTIP = "Double Trouble will not exceed this number of dupes.";
				}
			}
		}

		public static class AETE_EVENTS
		{
			public static class POLYMOPRH
			{
				public static LocString TOAST = "Turn {Dupe} into a {Critter}";
				public static LocString TOAST_ALT = "Polymorph Potion";
				public static LocString DESC = "Woosh! {Dupe} is now a {Critter}";
				public static LocString EVENT_END_NOTIFICATION = "Polymorph over";
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

			public static class SLIME_RAIN
			{
				public static LocString TOAST = "Goop Rain";
				public static LocString DESC = "Oh no, what is this sticky pink stuff??";
			}

			public static class TREE
			{
				public static LocString TOAST = "Tree of Inconvenience";
				public static LocString DESC = "This tree is probably in the way...";
			}

			public static class HAIL_RAIN
			{
				public static LocString TOAST = "Hailstorm";
				public static LocString DESC = "TODO";
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
				public static class AKISEXTRATWITCHEVENTS_DOUBLETROUBLE
				{
					public static LocString NAME = "Duplicant Duplicant";
					public static LocString DESCRIPTION = "...";
					public static LocString TOOLTIP = "...";
				}

				public static class AKISEXTRATWITCHEVENTS_CAFFEINATEDEFFECT
				{
					public static LocString NAME = "Caffeinated";
					public static LocString TOOLTIP = "This dupe had a much deserved coffee break.";
					public static LocString DESCRIPTION = "This dupe had a much deserved coffee break.";
					public static LocString ADDITIONAL_EFFECTS = "+{0} Work Speed";
				}

				public static class AKISEXTRATWITCHEVENTS_RADISHSTRENGTHEFFECT
				{
					public static LocString NAME = "Rad Strength";
					public static LocString TOOLTIP = "This Duplicant had some great radish recently.";
					public static LocString DESCRIPTION = "This Duplicant had some great radish recently.";
				}

				public static class AKISEXTRATWITCHEVENTS_GOLDSTRUCK
				{
					public static LocString NAME = "Goldstruck";
					public static LocString TOOLTIP = "I have touched this entity with Midas Touch and turned them to Gold. " +
						"\n" +
						"\nTime to recovery: {0}";

					public static LocString DESCRIPTION = "This entity is now made of solid Gold.";
				}

				public static class AKISEXTRATWITCHEVENTS_STEPPEDINSLIME
				{
					public static LocString NAME = "Sticky Feet";
					public static LocString TOOLTIP = "This duplicant has stepped in some Goop.";
					public static LocString DESCRIPTION = "This duplicant has decreased movement speed from stepping in Goop.";
				}

				public static class AKISEXTRATWITCHEVENTS_SOAKEDINSLIME
				{
					public static LocString NAME = "Sticky";
					public static LocString TOOLTIP = "This duplicant has recently covered themselves in some Goop.";
					public static LocString DESCRIPTION = "This duplicant has decreased movement speed from being submerged in Goop.";
				}

				public static class AKISEXTRATWITCHEVENTS_SUGARHIGH
				{
					public static LocString NAME = "Sugar Energy";
					public static LocString TOOLTIP = "This duplicant has excess energy thanks to a large serving of sugary treats.";
					public static LocString DESCRIPTION = "This duplicant has excess energy thanks to a large serving of sugary treats.";
				}
			}
		}

		public static class MISC
		{
			public static LocString MIDAS_STATE = "Gold State";

			public static class STATUSITEMS
			{
				public static class AKISEXTRATWITCHEVENTS_POLYMORPHSTATUS
				{
					public static LocString NAME = "Polymorphed";
					public static LocString TOOLTIP = "This {0} is enjoying the careless life of being a {1}.\n" +
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

				public static class AKISEXTRATWITCHEVENTS_GOLDSTRUCKSTATUS
				{
					public static LocString NAME = "Gold-struck";
					public static LocString TOOLTIP = "Frozen in gold.\n" +
						"\n" +
						"Time remaining: {0}";
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

				public static class AKISEXTRATWITCHEVENTS_GOOP_PARFAIT
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Goop Parfait", GoopParfaitConfig.ID);
					public static LocString DESC = "Layers of sweet custard and pink goop, making a wonderful dessert.";
				}

				// JELLO is added at ELEMENTS
			}
		}

		public static class ELEMENTS
		{
			public static class COFFEE
			{
				public static LocString NAME = "Coffee";
				public static LocString DESC = "Tasty.";
			}

			public static class FROZENCOFFEE
			{
				public static LocString NAME = "Frozen Coffee";
				public static LocString DESC = "Tasty.";
			}

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

			public static class FAKELUMBER
			{
				// name taken from actual Lumber
				public static LocString DESC = "Wood and bark material, can be burnt for fuel.";
			}

			public static class SOAP
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Soap", "Soap");
				public static LocString DESC = "";
			}

			public static class PINKSLIME
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Goop", "PinkSlime");
				public static LocString DESC = "A goopy, sticky, slimy liquid.";
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
					"Contains 1840 kcal per kilogram. (Must be mopped first, Duplicants will not eat a liquid directly from the" +
					" floor, they do have <i>some</i> standards.";
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
