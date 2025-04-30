using FUtility.FLocalization;
using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Critters;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Defs.Meds;

namespace Twitchery
{
	public class STRINGS
	{
		public static class DEATHS
		{
			public static class AETE_LASERED
			{
				public static LocString NAME = "Death Ray";
				public static LocString DESCRIPTION = "{Target} was carbonized in an instant.";
			}
		}

		public static class BUILDINGS
		{
			public static class PREFABS
			{
				public static class WATERCOOLER
				{
					public static class OPTION_TOOLTIPS
					{
						public static LocString AETE_HONEY = $"{FUtility.Utils.FormatAsLink("Honey", Elements.Honey.ToString())}\n" +
							$"Honey nourishes Duplicants, healing them and elevating their morale.";
					}
				}

				public static class AKISEXTRATWITCEVENTS_LEAFWALL
				{
					public static LocString NAME = "Leaf Wall";
					public static LocString DESC = "A pleasant wall of Greenery.";
					public static LocString EFFECT = "Provides Decor and prevents gas and liquid loss in Space.";
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
			public static class LEMONRAIN
			{
				public static LocString TOAST = "When chat gives lemons...";
				public static LocString DESC = "It's raining lemons!";
			}

			public static class FROZENFOODEXPRESS
			{
				public static LocString TOAST = "Frozen Food Express";
				public static LocString DESC = "Food has spawned, frozen extra fresh!";
			}

			public static class SPAWNMAGICALFLOX
			{
				public static LocString TOAST = "Spawn Magical Flox";
				public static LocString DESC = "A Magical Flox has spawned.";
			}

			public static class MACARONI
			{
				public static LocString TOAST = "Hey Macaroni";
				public static LocString DESC = "A serving of macaroni has been placed.";
			}

			public static class TINYCRABS
			{
				public static LocString TOAST = "Crab Attack";
				public static LocString DESC = "Aww look how cuu.... ow, ey, ouch, stop it!!";
			}

			public static class HEAVYEGGRAIN
			{
				public static LocString TOAST = "Heavy Egg Rain";
				public static LocString DESC = "It's raining eggs!";
			}

			public static class SUPERDUPE
			{
				public static LocString TOAST = "Super {Name}";
				public static LocString DESC = "{Name} is now a Super Duper Duplicant! (All stats up)";
				public static LocString DESC_NOTFOUND = "{Previous} was not around. Instead, {Name} is now a Super Duper Duplicant! (All stats up)";
			}

			public static class TRANSMUTATION
			{
				public static LocString TOAST = "Transmutation";
				public static LocString DESC = "All nearby {0} has been changed into {1}.";
				public static LocString DESC_FAIL = "Nothing to transmute here. Take this fart instead.";
			}

			public static class BLIZZARDMEDIUM
			{
				public static LocString TOAST = "Blizzard";
				public static LocString DESC = "It's snowing on {Asteroid}!";
			}

			public static class BLIZZARDDEADLY
			{
				public static LocString TOAST = "Dreadful Blizzard";
				public static LocString DESC = "A quiet snowfall descends on {Asteroid}. The cold bites...";
			}

			public static class SANDSTORMMEDIUM
			{
				public static LocString TOAST = "Sandstorm";
				public static LocString TITLE = "Mild Sandstorm";
				public static LocString DESC = "A sandstorm has enveloped {Asteroid}.";
			}

			public static class SANDSTORMDEADLY
			{
				public static LocString TOAST = "Sandstorm";
				public static LocString DESC_EXTRA = "The ground faintly vibrates beneath your feet.";
			}

			public static class SANDSTORMVICIOUS
			{
				public static LocString TOAST = "Sandstorm";
				public static LocString TITLE = "Vicious Sandstorm";
				public static LocString DESC_EXTRA = "THE EARTH IS RUMBLING.";
			}

			public static class FLUSH
			{
				public static LocString TOAST = "Flush!";
				public static LocString DESC = "Someone flushed {Percent} of all my liquids.";
			}

			public static class ALLOFTHEOTHERS
			{
				public static LocString TOAST = "All Others";
			}

			public static class LEAKYCURSORSAFE
			{
				public static LocString TOAST = "Leaky Cursor";
				public static LocString DESC = "Someone call a plumber! My cursor is dripping with {Element}!";
			}

			public static class LEAKYCURSOREXTREME
			{
				public static LocString TOAST = "Leaky Cursor";
			}

			public static class BEACHED
			{
				public static LocString TOAST = "Beached";
				public static LocString DESC = "Beached alpha release has just dropped: It's just sand. Lots of sand.";
			}

			public static class PIMPLES
			{
				public static LocString TOAST = "Pimples";
				public static LocString DESC = "They pop when I click them!";
			}

			public static class SOLARSTORMSMALL
			{
				public static LocString TOAST = "Mild Solar Storm";
				public static LocString DESC = "Increased Solar activity is interfering with all electronics on {0}!";
			}

			public static class SOLARSTORMMEDIUM
			{
				public static LocString TOAST = "Solar Storm";
			}

			public static class SUBNAUTICA
			{
				public static LocString TOAST = "Subnautica";
				public static LocString DESC = "Oxygen... <i>not included</i>";
			}

			public static class MUGSHOTS
			{
				public static LocString TOAST = "Mugshots";
				public static LocString DESC = "Mugs fired";
			}

			public static class SINKHOLE
			{
				public static LocString TOAST = "Sinkhole";
				public static LocString DESC = "There goes my stuff...";
			}

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

			public static class CARCERSCURSE
			{
				public static LocString TOAST = "Carcer's Curse";
			}

			public static class COLOSSALFART
			{
				public static LocString TOAST = "Fartocalypse";
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

			public static class GIANTCRAB
			{
				[Note("Make this conspicous if possible")]
				public static LocString TOAST = "Spawn a regular crab";
				public static LocString DESC = "Behold: a regular crab!";
			}

			public static class ROCKPAPERSCISSORS
			{
				public static LocString TOAST = "Rock Paper Scissors";
				public static LocString ROCK = "Rock!";
				public static LocString PAPER = "Paper!";
				public static LocString SCISSORS = "Scissors!\n Something somewhere just got snipped...";
			}

			public static class HOTTUB
			{
				public static LocString TOAST = "Spicy Flood";
				public static LocString JUST_KIDDING = "Just kidding!";
				public static LocString HOT = "HOT HOT HOT HOT";
				public static LocString SPICY = "Spice Up!";
			}

			public static class REGULARPIP
			{
				[Note("Make this conspicous if possible")]
				public static LocString TOAST = "Spawn a regular pip";
				public static LocString DESC = "Something has spawned";
			}

			public static class PETREGULARPIP
			{
				public static LocString TOAST = "Pet {Name}";
				public static LocString DESC = "Everyone in the chat took turns to pet {Name}." +
					"\n" +
					"\n{Name} has increased productivity for a cycle!";
			}

			public static class ENCOURAGEREGULARPIP
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

			public static class VOIDTOUCH
			{
				public static LocString TOAST = "Matter Eater";
				public static LocString DESC = "DESTROY!";
			}

			public static class SLIMETOUCH
			{
				public static LocString TOAST = "Slime Touch";
				public static LocString DESC = "Oh ew no why?? Everything I touch is slime!";
			}

			public static class FORESTTOUCH
			{
				public static LocString TOAST = "Nature's Touch";
				public static LocString DESC = "Reclaim nature!";
			}

			public static class PLACEGEYSER
			{
				public static LocString TOAST = "Spontaneous Eruption";
				public static LocString DESC = "Chat was in an erupting mood!";
			}

			public static class HOTSHOWER
			{
				public static LocString TOAST = "Hot Shower";
				public static LocString DESC = "Your dupes were dirty, have a shower!";
			}

			public static class JELLORAIN
			{
				public static LocString TOAST = "Jello Rain";
				[Note("Reference: Terraria")]
				public static LocString DESC = "Jello is falling from the sky!";
			}

			public static class GOLDRAINHONEY
			{
				public static LocString DESC = "It's raining Liquid Gold!";
			}

			public static class REMOVELIQUIDS
			{
				public static LocString TOAST = "Remove Liquids";
				public static LocString DESC = "All liquids near your cursor were removed.";
			}

			public static class GOOPRAIN
			{
				[Note("References Glommer's Goop from Don't Starve")]
				public static LocString TOAST = "Goop Rain";
				public static LocString DESC = "Oh no, what is this sticky pink stuff??";
			}

			public static class RAINBOWRAIN
			{
				public static LocString TOAST = "Rainbow Rain";
				public static LocString DESC = "A rain of many things has started!";
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

			public static class INVISIBLELIQUIDS
			{
				public static LocString TOAST = "Invisible Liquids";
				public static LocString DESC = "Wait, where is my water??!";
			}

			public static class DEATHLASER
			{
				public static LocString TOAST = "Deathlaser";
				public static LocString DESC = "Please don't point it at eyeballs, or airplanes... or anything, really. ";
			}

			public static class SLIMIERBEDROOMS
			{
				public static LocString TOAST = "Slimier Bedrooms";
				public static LocString DESC = "All bedrooms have been slimed.";
			}

			public static class HOMERENOVATION
			{
				public static LocString TOAST = "House Flipper";
				public static LocString DESC = "Enjoy your new furniture!";
			}

			public static class BRACKENERAIN
			{
				public static LocString TOAST = "Milk Rain";
				public static LocString DESC = "It's raining dairy!";
			}

			public static class CHATRAID
			{
				public static LocString TOAST = "Chat Raid";
				public static LocString DESC = "We have arrived!";
			}

			public static class DOUBLETROUBLE
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

			public static class RADDISH
			{
				[Note("Radioactive Dish but also a Radish. Glowing radish or radiant radish also works.")]
				public static LocString TOAST = "Rad Dish";
				public static LocString DESC = "A singular radish has been spawned on {Asteroid}.";
				public static LocString DESC_RECIPE = "\nAlso new recipe at Electric Grill.";
			}

			public static class RETROVISION
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

				public static class AKISEXTRATWITCHEVENTS_BIONICSOLARZAP
				{
					public static LocString NAME = "Magnetic Interference";
					public static LocString TOOLTIP = "The Solar Storm is interfering with the circuits of this Duplicant.";
					public static LocString DESCRIPTION = "This Duplicant is being stressed out, but not as much when wet.";
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

				public static class AKISEXTRATWITCHEVENTS_SUPERDUPE
				{
					public static LocString NAME = "Super Duplicant";
					public static LocString TOOLTIP = "This duplicant is feeling Incredible!";
					public static LocString DESCRIPTION = "";
				}

				public static class AKISEXTRATWITCHEVENTS_TWITCHGUEST
				{
					public static LocString NAME = "Raider";
					public static LocString TOOLTIP = "This duplicant has raided in from a Twitch Event!";
					public static LocString DESCRIPTION = "";
				}

				public static class AKISEXTRATWITCHEVENTS_SUGARHIGH
				{
					public static LocString NAME = "Sugar Energy";
					public static LocString TOOLTIP = "This duplicant has excess energy thanks to a large serving of sugary treats.";
					public static LocString DESCRIPTION = "This duplicant has excess energy thanks to a large serving of sugary treats.";
				}

				public static class AKISEXTRATWITCHEVENTS_LEMON
				{
					public static LocString NAME = "C Vitamin";
					public static LocString TOOLTIP = "This duplicant has a strongethened immune system by the power of lemon.";
					public static LocString DESCRIPTION = "This duplicant has a strongethened immune system by the power of lemon.";
				}

				public static class AKISEXTRATWITCHEVENTS_COMFORTFOOD
				{
					public static LocString NAME = "Comfort Food";
					public static LocString TOOLTIP = "This duplicant had a quick lazy comfort meal.";
					public static LocString DESCRIPTION = "";
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

			public static class TAGS
			{
				public static LocString AKISEXTRATWITCHEVENTS_USELESS = "Useless";
			}

			public static class AKISEXTRATWITCHEVENTS_PIMPLE
			{
				public static LocString NAME = "{0} Pimple";
			}

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
			public class PILLS
			{
				public class AKISEXTRATWITCHEVENTS_LEMONADE
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Lemonade", LemonadeConfig.ID);
					public static LocString DESC = $"A super refreshing drink for hot days.";
				}

				public class AKISEXTRATWITCHEVENTS_MELONADE
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Melonade", MelonadeConfig.ID);
					public static LocString DESC = $"The friction between this drink's matter and the laws of the Universe keeps this drink ever so hot. Perfect for a chilly day!";
				}
			}

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
				public static class BEACHED_ASTROBAR
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Astrobar", BeachedAstrobarConfig.ID);
					public static LocString DESC = "Delicious and nutritious candy bar, with a sticky and gooey filling that sticks to the roof of the mouth.";
				}

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

				public static class AKISEXTRATWITCHEVENTS_LEMON
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Lemon", LemonConfig.ID);
					public static LocString DESC = $"A very sour juicy fruit. Can be eaten or squeezed into {FUtility.Utils.FormatAsLink("Lemonade", LemonadeConfig.ID)} at the {FUtility.Utils.FormatAsLink("Apothecary", ApothecaryConfig.ID)}.";
				}

				public static class AKISEXTRATWITCHEVENTS_MACANDCHEESE
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Mac N' Cheese", MacAndCheeseConfig.ID);
					public static LocString DESC = "This dish was played 80's power ballads while cooking to ensure maximum cheesyness.";
				}

				// JELLO is added at ELEMENTS
			}
		}

		public static class ELEMENTS
		{
			public static class AETE_FROZENPINKSLIME
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Frozen Goop", Elements.FrozenPinkSlime.ToString());
				public static LocString DESC = "Sticky goop unfortunately frozen solid.";
			}

			public static class AETE_FAKELUMBER
			{
				// name taken from actual Lumber
				public static LocString DESC = "Wood and bark material, can be burnt for fuel.";
			}

			public static class AETE_PINKSLIME
			{
				[Note("References Glommer's Goop from Don't Starve")]
				public static LocString NAME = FUtility.Utils.FormatAsLink("Goop", Elements.PinkSlime.ToString());
				public static LocString DESC = "A goopy, sticky, slimy liquid.";
			}

			public static class AETE_HONEY
			{
				//public static LocString NAME = FUtility.Utils.FormatAsLink("Honey", Elements.Honey.ToString());
				public static LocString NAME = FUtility.Utils.FormatAsLink("<indent=15%><rotate=180>Honey</rotate></indent>", Elements.Honey.ToString());
				public static LocString DESC = "Sweet nectar harvested from local bears.";
			}

			public static class AETE_MACARONI
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Macaroni", Elements.Macaroni.ToString());
				public static LocString DESC = $"Elbow shaped crunchy and dry pasta. \n\nCan be eaten raw, or cooked into {FUtility.Utils.FormatAsLink("Mac N' Cheese", MacAndCheeseConfig.ID)}.";
			}

			public static class AETE_FROZENHONEY
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Frozen Honey", Elements.FrozenHoney.ToString());
				public static LocString DESC = "Please resist the urge to lick this.";
			}

			public static class AETE_RASPBERRYJAM
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Raspberry Jam", Elements.RaspberryJam.ToString());
				public static LocString DESC = "<b>Work In Progress</b>.\n\nNothing should spawn this (yet). But if you got some, put it away for later, I heard jams make for a good reserve.";
			}

			public static class AETE_JELLO
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Jello", Elements.Jello.ToString());
				public static LocString DESC = "A jiggly, edible liquid that behaves like Visco Gel. It's kiwi flavored.\n\n" +
					"Contains 1840 kcal per kilogram. (Must be mopped first, Duplicants will not eat a liquid directly from the" +
					" floor, they do have <i>some</i> standards.";
			}

			public static class AETE_PLASMA
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Plasma", Elements.Plasma.ToString());
				public static LocString DESC = "A jiggly, edible liquid that behaves like Visco Gel. It's kiwi flavored.\n\n" +
					"Contains 1840 kcal per kilogram. (Must be mopped first, Duplicants will not eat a liquid directly from the" +
					" floor, they do have <i>some</i> standards.";
			}

			public static class AETE_EARWAX
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Earwax", Elements.EarWax.ToString());
				public static LocString DESC = "Waxy substance from the ear canals of an unknown creature. The scientific name for this material is \"Cerumen\".";
			}

			public static class AETE_FROZENJELLO
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Solid Jello", Elements.FrozenJello.ToString());
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
				public static class GEYSER
				{
					public static class AKISEXTRATWITCHEVENTS_PIPSER
					{
						public static LocString NAME = FUtility.Utils.FormatAsLink("Pipser", $"GEYSER_GENERIC_{TGeyserConfigs.PIPSER}");
						public static LocString DESC = $"Periodically erupts with {FUtility.Utils.FormatAsLink("Pips", SquirrelConfig.ID)}.";
					}

					public static class AKISEXTRATWITCHEVENTS_GOOPGEYSER
					{
						public static LocString NAME = FUtility.Utils.FormatAsLink("Goop Geyser", $"GEYSER_GENERIC_{TGeyserConfigs.GOOP_GEYSER}");
						public static LocString DESC = "Periodically emits a refreshing pink slushie.";
					}

					public static class AKISEXTRATWITCHEVENTS_MOLTENGLASSGEYSER
					{
						public static LocString NAME = FUtility.Utils.FormatAsLink("Molten Glass Volcano", $"GEYSER_GENERIC_{TGeyserConfigs.MOLTEN_GLASS_VOLCANO}");
						public static LocString DESC = $"Periodically erupts with {FUtility.Utils.FormatAsLink("Molten Glass", SimHashes.MoltenGlass.ToString())}";
					}
				}

				public static class AKISEXTRATWITCHEVENTS_GIANTCRAB
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Ol' Crab Pal", GiantCrabConfig.ID);
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
				public static LocString CARCERPROMPT = "ooh dupes vomit goop while erupting in pacu?";
				public static LocString PROTECTED = "Protected";
				public static LocString RESISTED = "Resisted";
				public static LocString COULD_NOT_PLACE = "Placement Failed";

				public static class UNLOAD
				{
					public static LocString LABEL = "Unload";
					public static LocString TOOLTIP = "Extract contents to the floor. Duplicants can deliver from this container to storages either way.";
				}

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
