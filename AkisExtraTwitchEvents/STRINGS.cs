using FUtility.FLocalization;
using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;

namespace Twitchery
{
	public class STRINGS
	{
		public static class BUILDINGS
		{
			public static class PREFABS
			{
				public static class WATERCOOLER
				{
					public static class OPTION_TOOLTIPS
					{
						public static LocString HONEY = $"{FUtility.Utils.FormatAsLink("Honey")}\n" +
							$"Honey nourishes Duplicants, healing them and elevating their morale.";
					}
				}

				public static class AKISEXTRATWITCHEVENTS_PUZZLEDOOR
				{
					public static LocString NAME = "Ultra-Secure Door";
					public static LocString DESC = "Send a green signal from the connected Logic Sensor to open.";
					public static LocString EFFECT = "A door designed by Gravitas to keep grubby hands to themselves.";
				}
			}
		}

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
			public static class PLACEAQUARIUM
			{
				public static LocString TOAST = "Fishbowl";
				public static LocString DESC = "Happy little fishies just for you!";
			}

			public static class POLYMOPRH
			{
				public static LocString TOAST = "Polymorph {Name}";
				[Note("Reference to Noita Polymorph potion. Turns a dupe into a critter")]
				public static LocString TOAST_ALT = "Polymorph Potion";
				public static LocString DESC_NOTFOUND = "{TargetDupe} no longer around. The potion found {Dupe} instead...\n";
				public static LocString DESC = "Woosh! {Dupe} is now a {Critter}";
				public static LocString EVENT_END_NOTIFICATION = "Polymorph over";
			}

			public static class CARCERS_CURSE
			{
				public static LocString TOAST = "Carcer's Curse";
			}

			public static class MEGAFART
			{
				public static LocString TOAST = "Megafart";
			}

			public static class WEREVOLE
			{
				public static LocString EVENT_NAME = "Can a Vole awoooo?";
				public static LocString TOAST = "Can a Vole awoooo?";
				public static LocString DESC = "Yes, it can! {Name} is now a Werevole!";
			}

			public static class GIANT_CRAB
			{
				[Note("Make this conspicous if possible")]
				public static LocString TOAST = "Spawn a regular crab";
				public static LocString DESC = "Behold: a regular crab!";
			}

			public static class REVIVE_DUPE
			{
				public static LocString TOAST = "Revive {0}";
				public static LocString DESC = "{0} is back for more!";
			}


			public static class HOTTUB
			{
				public static LocString TOAST = "Spicy Flood";
				public static LocString JUST_KIDDING = "Just kidding!";
				public static LocString HOT = "HOT HOT HOT HOT";
				public static LocString SPICY = "Spice Up!";
			}

			public static class REGULAR_PIP
			{
				[Note("Make this conspicous if possible")]
				public static LocString TOAST = "Spawn a regular pip";
				public static LocString DESC = "Something has spawned";
			}

			public static class PET_REGULAR_PIP
			{
				public static LocString TOAST = "Pet {Name}";
				public static LocString DESC = "Everyone in the chat took turns to pet {Name}." +
					"\n" +
					"\n{Name} has increased productivity for a cycle!";
			}

			public static class ENCOURAGE_REGULAR_PIP
			{
				public static LocString TOAST = "Encourage {Name}";
				public static LocString DESC_FIRST = "The chat has collectively encouraged {Name}! {Name} is now smarter.";
				public static LocString DESC = "The chat has collectively encouraged {Name}! {Name} has now learnt {Skill}.";

				public static LocString OHNO = "Oh no!";
				public static LocString SOMETHING_WENT_WRONG = "Something went wrong, {Name} does not seem to be around anymore.";
				public static LocString INSTEAD = "The chat gave love to {Name} instead.";
				[Note("shows if there are no pips to update, because all of them died between the vote and the event running.")]
				public static LocString NOONE = "The chat finds no one to pet, and goes home. Sad day.";
			}

			public static class HULK
			{
				public static LocString TOAST = "Spawn The Hulk";
				public static LocString DESC = "The Hulk has arrived!";
			}

			public static class TREE
			{
				public static LocString TOAST = "Tree of inconvenience";
				public static LocString DESC = "It would be a shame if I put a tree right here...";
				public static LocString DESC2 = "And also here";
			}

			public static class EGG
			{
				public static LocString TOAST = "Egg";
			}

			public static class MIDASTOUCH
			{
				public static LocString TOAST = "Midas Touch";
				public static LocString DESC = "Everything I touch is gold!";
			}

			public static class FREEZETOUCH
			{
				public static LocString TOAST = "Freezegun";
				public static LocString DESC = "Everything I touch freezes!";
			}

			public static class SLIMETOUCH
			{
				public static LocString TOAST = "Slime Touch";
				public static LocString DESC = "Oh ew no why?? Everything I touch is slime!";
			}

			public static class JELLO_RAIN
			{
				public static LocString TOAST = "Jello Rain";
				[Note("Reference: Terraria")]
				public static LocString DESC = "Jello is falling from the sky!";
			}

			public static class SLIME_RAIN
			{
				[Note("References Glommer's Goop from Don't Starve")]
				public static LocString TOAST = "Goop Rain";
				public static LocString DESC = "Oh no, what is this sticky pink stuff??";
			}

			public static class PIPSPLOSION
			{
				public static LocString TOAST = "Pipsplosion";
				public static LocString DESC = "Oh no, not the pips, NOT THE PIPS!";
			}

			public static class COFFEEBREAK
			{
				public static LocString TOAST = "Coffee break";
				public static LocString DESC = "All Duplicants are taking a well deserved break.";
			}

			public static class INVISIBLE_LIQUIDS
			{
				public static LocString TOAST = "Invisible Liquids";
				public static LocString DESC = "Wait, where is my water??!";
			}

			public static class BRACKENE_RAIN
			{
				public static LocString TOAST = "Milk Rain";
				public static LocString DESC = "It's raining dairy!";
			}

			public static class DOUBLE_TROUBLE
			{
				[Note("Reference: Pokemon")]
				public static LocString TOAST = "Double Trouble";
				[Note("Reference: Pokemon")]
				public static LocString TITLE = "Prepare for trouble";
				[Note("Reference: Pokemon")]
				public static LocString DESC = "And make it double!";
			}

			public static class PIZZADELIVERY
			{
				public static LocString TOAST = "Pizza Delivery";
				[Note("Reference: Spiderman")]
				public static LocString DESC = "Pizza time!";
				public static LocString DESC_RECIPE = "\nAlso new recipe at Gas Range.";
			}

			public static class RAD_DISH
			{
				[Note("Radioactive Dish but also a Radish. Glowing radish or radiant radish also works.")]
				public static LocString TOAST = "Rad Dish";
				public static LocString DESC = "A singular radish has been spawned on {Asteroid}.";
				public static LocString DESC_RECIPE = "\nAlso new recipe at Electric Grill.";
			}

			public static class RETRO_VISION
			{
				[Note("Reference: Binding of Isaac, pill of the same name")]
				public static LocString TOAST = "Retro Vision";
				public static LocString DESC = "Your video card has been downgraded.";
			}
		}

		public static class DUPLICANTS
		{
			public static class REGULAR_PIP_NAMES
			{
				[Note("Separate names with `/`. Does not need to be direct translations, just vaguely funny pip names. " +
					"Number of names can be anything.")]
				public static LocString NAMES =
					"Pippo/" +
					"Pippin/" +
					"Merry/" +
					"Mirko/" +
					"Sandy/" +
					"Fluffybutt/" +
					"Prof. Arbor/" +
					"Skippy/" +
					"Sheogorath/" +
					"Lettucifer/" +
					"Nuttsy/" +
					"Salame/" +
					"Beelzepip/" +
					"Poofball";

				public static LocString DESCRIPTION = "This {Name} has decided it is bored of being the emissary of Chaos, and has picked up a utility gun.";

				public static LocString GENDER = "???";
				public static LocString GENDER_DESCRIPTION = "If you stare into the Abyss, the Abyss stares back at you.";
			}

			public class PERSONALITIES
			{
				public class AKISEXTRATWITCHEVENTS_HULK
				{
					public static LocString NAME = "The Hulk";
					public static LocString NAME2 = "{ChatterName} The Hulk";
					public static LocString DESC = "HULK SMASH";
				}
			}

			public class TRAITS
			{
				public class AKISEXTRATWITCHEVENTS_ANGRY
				{
					public static LocString NAME = "Angry";
					public static LocString SHORT_DESC = "This Duplicant is always angry.";
					public static LocString SHORT_DESC_TOOLTIP = "Being always angry, this Duplicant is quick at their work, but has a chance to " +
						"break equipment every now and then.";
				}

				public class AKISEXTRATWITCHEVENTS_WEREVOLE
				{
					public static LocString NAME = "Werevole";
					public static LocString SHORT_DESC = "This duplicant has a strange desire to dig all day.\n\n" +
						"At night or during Eclipse transforms into a werevole. During this time this Duplicant can only do dig errands, but they are <b>really</b> good at it. \n\n" +
						"If desired, curable with a Silver Milkshake created at the Apothecary.";
				}

				public class AKISEXTRATWITCHEVENTS_ROOKIE
				{
					public static LocString NAME = "Wild";
					public static LocString SHORT_DESC = "This Critter is Wild.";
				}
			}

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

				public static class AKISEXTRATWITCHEVENTS_HONEY
				{
					public static LocString NAME = "Honeyed";
					public static LocString TOOLTIP = "This duplicant has licked Frozen Honey.";
					public static LocString DESCRIPTION = "This duplicant has licked Frozen Honey.";
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
					[Note("Reference: Noita")]
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

				public static class AKISEXTRATWITCHEVENTS_FROZENSTATUS
				{
					public static LocString NAME = "Frozen";
					public static LocString TOOLTIP = "Frozen solid until thawed.";
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
			public class SPICES
			{
				public class AKISEXTRATWITCHEVENTS_SPICE_GOLDFLAKE
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Gold Flakes", TSpices.GOLD_FLAKE);
					public static LocString DESC = $"Makes any food feel a lot more posh!\n\nIncreases the quality of a meal. Does not affect Ambrosial or higher quality foods.";
				}
			}

			public static class FOOD
			{
				public static class AKISEXTRATWITCHEVENTS_RAWRADISH
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Chunk O' Radish", RawRadishConfig.ID);
					public static LocString DESC = "An edible chunk of the godly Radish.";
				}

				public static class AKISEXTRATWITCHEVENTS_COOKEDRADISH
				{
					[Note("Radioactive Dish but also a Radish. Glowing radish or radiant radish also works.")]
					public static LocString NAME = FUtility.Utils.FormatAsLink("Rad Dish", CookedRadishConfig.ID);
					public static LocString DESC = "Delicious grilled radish with a radiantly green crust. It's <i>probably</i> safe.";
				}

				public static class AKISEXTRATWITCHEVENTS_PIZZA
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Pizza", PizzaConfig.ID);
					public static LocString DESC = "A wonderful, filling dish.";
				}

				public static class AKISEXTRATWITCHEVENTS_GRANOLABAR
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Granola Bar", PizzaConfig.ID);
					public static LocString DESC = "A pip's favorite food.";
				}

				public static class AKISEXTRATWITCHEVENTS_GOOPPARFAIT
				{
					[Note("Parfait is a french dessert made of egg custard and some other sweet ingredients, such as jams.")]
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

			public static class FROZENPINKSLIME
			{
				public static LocString NAME = "Frozen Goop";
				public static LocString DESC = "Sticky goop unfortunately frozen solid.";
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
				[Note("References Glommer's Goop from Don't Starve")]
				public static LocString NAME = FUtility.Utils.FormatAsLink("Goop", "PinkSlime");
				public static LocString DESC = "A goopy, sticky, slimy liquid.";
			}

			public static class HONEY
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Honey", "Honey");
				public static LocString DESC = "Sweet nectar harvested from local bears.";
			}

			public static class FROZENHONEY
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Frozen Honey", "FrozenHoney");
				public static LocString DESC = "Please resist the urge to lick this.";
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
			public static class STATUSITEMS
			{
				public static class AKIS_EXTRATWITCHEVENTS_GOINGHOME
				{
					public static LocString NAME = "Finding safety";
					public static LocString TOOLTIP = "This Were-Vole is trying to find a safe place to turn back to their Dupe form.";
				}
			}

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

		public static class UI
		{
			public static class AKIS_EXTRA_TWITCH_EVENTS
			{
				public static class GAMEOBJECTEFFECTS
				{
					public static class DAMAGE_POPS
					{
						public static LocString HULK_SMASH = "<size=250%>Smash!</size>";
					}
				}
			}
		}
	}
}
