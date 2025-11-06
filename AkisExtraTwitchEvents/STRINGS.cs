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
			public static class AETE_EATEN
			{
				public static LocString NAME = "Eaten";
				public static LocString DESCRIPTION = "{Target} was devoured.";
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
			public static class CATEGORIES
			{
				public static readonly LocString A_GENERAL = "General";
				public static readonly LocString E_VISUAL = "Visual";
				public static readonly LocString I_DUPLICANTS = "Duplicant Spawning";
				public static readonly LocString M_FOOD = "Food Values";
				public static readonly LocString Q_WORLDEVENTS = "Long Events";
			}
			public class GENERIC
			{
				public static class RARITY
				{
					public static readonly LocString TITLE = "Event Rarity Multiplier";
					public static readonly LocString TOOLTIP = "Modifies the rarity of all events from this mod only.\n" +
						"If set to 0, all events are effectively disabled.\n\n" +
						"Use Twitch Integration's own settings to adjust individual event weights.";
				}

				public static class CURSED
				{
					public static readonly LocString TITLE = "Cursed Mode";
					public static readonly LocString TOOLTIP = "No warnings. No mercy.\n" +
						"\n" +
						"<b><color=#ff3838>Probably don't turn this on if you want to have fun.</color></b>";

				}
				public static class BIAS
				{
					public static readonly LocString TITLE = "Event Rarity Bias Fix";
					public static readonly LocString TOOLTIP = "Due to how events are selected in the base Twitch Integration mod, if too many extra event mods are installed, \n" +
			"super rare events like Pocket Dimensions will almost never happen anymore." +
			"\nUse this number to equalize weights and give them a better chance." +
			"\n" +
			"\n0: Do not equalize, vanilla behavior." +
			"\n1: Super rare events will be as common as any other event.";

				}
				public static class COLONY_LOST
				{
					public static readonly LocString TITLE = "Suppress Colony Lost Popup";
					public static readonly LocString TOOLTIP = "Suppress Colony Lost message if at least one Regular Pip, Midased Duplicant, Polymorph or Werevole is still alive.";
				}

			}

			public static class DUPLICANTS
			{
				public static class LIFETIME
				{

					public static readonly LocString TITLE = "Temporary Duplicant Lifetime";
					public static readonly LocString TOOLTIP = "How long do temporary duplicants live? Double Trouble, Chat Raid, etc.";
				}
				public static class OXYGEN
				{

					public static readonly LocString TITLE = "Oxygen Consumption Modifier";
					public static readonly LocString TOOLTIP = "g/s oxygen consumption modifier.";
				}
			}

			public static class VISUAL
			{
				public static class SHAKE
				{
					public static readonly LocString TITLE = "Camera Shake";
					public static readonly LocString TOOLTIP = "Shaking is short and temporary for some high impact events.";
				}

				public static class TRAIL
				{
					public static readonly LocString TITLE = "Render Trail";
					public static readonly LocString TOOLTIP = "Enable the orange trail effect on Duplicant who were made Super.";
				}
			}


			public static class WORLD_EVENTS
			{
				public static class SOLAR_STORM_DURATION
				{
					public static readonly LocString TITLE =
			"Solar Storm Duration (cycles)";
					public static readonly LocString TOOLTIP = "How many cycles should Solar Storm last.";
				}
			}

			public static class FOOD
			{
				public static class RADISH
				{
					public static readonly LocString TITLE =
			"Radish Total KCal";
					public static readonly LocString TOOLTIP = "";
				}

				public static class PIZZA
				{
					public static readonly LocString TITLE =
			"Pizza Delivery Total KCal";
					public static readonly LocString TOOLTIP = "";
				}

				public static class FROZEN_HONEY
				{
					public static readonly LocString TITLE =
			"Frozen Honey Kcal/kg";
					public static readonly LocString TOOLTIP = "Tiles usually come in 1000kg masses, making a single tile yield 40000Kcal as is..";
				}
			}

			public static class DOUBLE_TROUBLE
			{
				public static class MAX_DUPES
				{
					public static LocString LABEL = "Max Duplicants";
					public static LocString TOOLTIP = "Your total colony duplicant count will not exceed this number if dupes are spawned in via this mod's events.";
				}
			}
		}

		public static class AETE_EVENTS
		{
			public static LocString NO_TARGETS = "No targetable duplicants alive, cannot execute event\n" +
				"(nothing happened).";

			public static class LADDERHATINGBEES
			{
				public static LocString TOAST = "Ladder hating bees";
				public static LocString DESC = "A swarm of bees ate some ladders!";
				public static LocString LADDERLESS = "Fortunately for you, however, you are ladderless. \n\n(Nothing happened)";
			}

			public static class HARVESTMOON
			{
				public static LocString TOAST = "Harvest Moon";
				public static LocString DESC = "The Harvest Moon has risen on {0}! All plants grow super fast!";
				public static LocString OVER = "The Harvest Moon has has set on {0}. Plants are back to normal.";
				public static LocString FAILED = "FAILED: No eligible world was found anymore.";
			}

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

			public static class JAIL
			{
				public static LocString TOAST_ALT = "Jail Duplicant";
				public static LocString TOAST = "Jail {Name}";
				public static LocString DESC = "{Name} is now in jail.";
				public static LocString DESC_NOTFOUND = "{Previous} was not around. Instead, {Name} is jailed.";
			}

			public static class SUPERDUPE
			{
				public static LocString TOAST = "Super {Name}";
				public static LocString TOAST_ALT = "Super Duplicant";
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

			public static class SPAWNMUCKROOTS
			{
				public static LocString TOAST = "Spawn Muckroots";
				public static LocString DESC = "Muckroots have been spawned";
			}

			public static class BLIZZARDDEADLY
			{
				public static LocString TOAST = "Dreadful Blizzard";
				public static LocString DESC = "A quiet snowfall descends on {Asteroid}. The cold bites...";
			}

			public static class SANDSTORMMEDIUM
			{
				public static LocString TOAST = "Mild Sandstorm";
				public static LocString DESC = "A sandstorm has enveloped {Asteroid}.";
			}

			public static class SANDSTORMHIGH
			{
				public static LocString TOAST = "Sandstorm";
				public static LocString DESC_EXTRA = "The ground faintly vibrates beneath your feet.";
			}

			public static class SANDSTORMDEADLY
			{
				public static LocString TOAST = "Vicious Sandstorm";
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

			public static class MEATBALL
			{
				public static LocString TOAST = "Meatball";
				public static LocString EXTRARARE = "Served extra rare.";
				public static LocString RARE = "Served rare.";
				public static LocString MEDIUMRARE = "Served medium-rare.";
				public static LocString MEDIUM = "Served medium.";
				public static LocString MEDIUMWELLDONE = "Served medium-well.";
				public static LocString WELLDONE = "Served well-done.";
				public static LocString EXTRAWELLDONE = "Served extra well done.";

				public static LocString DESC = "Bon Appétit!";
			}

			public static class SWEATYDUPES
			{
				public static LocString TOAST = "Sweaty Dupes";
				public static LocString DESC1 = "All Duplicants were told a flash quiz is coming up.";
				public static LocString DESC2 = "All Duplicants were told they are being streamed online.";
				public static LocString DESC3 = "Someone turned up the heat.";
				public static LocString DESC4 = "All Duplicants suddenly remembered their most embarrasing memories.";
				public static LocString DESC_END = "\n\nEveryone began sweating.";
			}
			public static class PIMPLES
			{
				public static LocString TOAST = "Pimples";
				public static LocString DESC = "They pop when I click them!";
			}

			public static class PANDORASBOX
			{
				public static LocString TOAST = "Cursed Suprise Box";
				public static LocString DESC = "A Pandora's Box has spawned!";
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
				public static LocString MESSAGE = "Chat plays {Pick}!\n" +
					"You played {PlayerPick}!\n" +
					"<size=85%>You hovered <b>{Thing}</b></size>";
				public static LocString WON = "You Won!";
				public static LocString LOST = "You Lost!";
				public static LocString TIE = "It's a tie!";
				public static LocString PRIZE = "Your prize has spawned.";
				public static LocString ROCK = "Rock!";
				public static LocString ROCK_LOST = "Boulder!";
				public static LocString PAPER = "Paper!";
				public static LocString PAPER_LOST = "All Duplicants got toilet paper stuck to their feet. How embarrasing!";
				public static LocString SCISSORS = "Scissors!";
				public static LocString SCISSORS_LOST = "Something somewhere just got snipped...";
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

			public static class MOP
			{
				public static LocString TOAST = "Mop";
				public static LocString DESC_ASTEROID = "Chat snuck in and mopped up your mess on {Asteroid}!";
				public static LocString DESC_GLOBAL = "Chat snuck in and mopped up your mess!";
				public static LocString DESC_NO_MOPS = "Chat wanted to mop up, but you seem to have handled it already. \n(Nothing happened)";
			}

			public static class FARTINGCURSOR
			{
				public static LocString TOAST = "Farting Cursor";
				public static LocString DESC = "Chat farts in your general direction.";
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

			public static class EDUCATIONAL
			{
				public static LocString TOAST = "Educational";
				public static LocString DESC_DUPES = "A new show about {Skill} is airing on the Research Station screens! \nThe following dupes took up an interest in <b>{Skill}</b>:";
				public static LocString FAIL_NO_DUPES = "But there was nothing to learn. (nothing happened)";
			}

			public static class SLIMETOUCH
			{
				public static LocString TOAST = "Slime Touch";
				public static LocString DESC = "Oh ew no why?? Everything I touch is slime!";
			}

			public static class PIPTOUCH
			{
				public static LocString TOAST = "Pip's Touch";
				public static LocString DESC = "Everything I touch is pips!";
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

			public static class OILEDUP
			{
				public static LocString TOAST = "Oil the dupes";
				public static LocString DESC = "All dupes are glistening with oil!";
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
				public static LocString DESC_DEADLY = "<color=#ff3838>Cursed!</color>\n" +
					"A rain of many terrible things has started!";
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

			public static class ALTSNOWYBEDROOMS
			{
				public static LocString TOAST = "Snowy Bedrooms";
				public static LocString DESC = "Snow was placed in every bedroom...?";
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
						$"If desired, curable with a {FUtility.Utils.FormatAsLink("Volesbane Tea", WereVoleCureConfig.ID)} created at the {FUtility.Utils.FormatAsLink("Apothecary", ApothecaryConfig.ID)}.";
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

				public static class AKISEXTRATWITCHEVENTS_OILEDUP
				{
					public static LocString NAME = "Oiled Up";
					public static LocString TOOLTIP = "This Duplicant is covered in slippery Oil.";
					public static LocString DESCRIPTION = "Oil is causing this Duplicant to slip and fall.";
				}

				public static class AKISEXTRATWITCHEVENTS_HARVESTMOON
				{
					public static LocString NAME = "Bountiful";
					public static LocString TOOLTIP = "During the Harvest Moon this plant is growing extra fast.";
					public static LocString DESCRIPTION = "During the Harvest Moon this plant is growing extra fast.";
				}

				public static class AKISEXTRATWITCHEVENTS_SWEATY
				{
					public static LocString NAME = "Sweaty";
					public static LocString TOOLTIP = "This duplicant is sweating profusely.";
					public static LocString DESCRIPTION = "This duplicant is sweating profusely.";
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

				public static class AKISEXTRATWITCHEVENTS_SUGARHIGH
				{
					public static LocString NAME = "Sugar Energy";
					public static LocString TOOLTIP = "This duplicant has excess energy thanks to a large serving of sugary treats.";
					public static LocString DESCRIPTION = "This duplicant has excess energy thanks to a large serving of sugary treats.";
				}

				public static class AKISEXTRATWITCHEVENTS_TWITCHGUEST
				{
					public static LocString NAME = "Raider";
					public static LocString TOOLTIP = "This duplicant has raided in from a Twitch Event!";
					public static LocString DESCRIPTION = "";
				}

				public static class AKISEXTRATWITCHEVENTS_TOILETPAPERSTUCK
				{
					public static LocString NAME = "Toilet Paper Stuck";
					public static LocString TOOLTIP = "This duplicant had a silly accident leaving the Lavatory.";
					public static LocString DESCRIPTION = "This duplicant has decreased Athletics and looks funny.";
				}

				public static class AKISEXTRATWITCHEVENTS_LEMON
				{
					public static LocString NAME = "C Vitamin";
					public static LocString TOOLTIP = "This duplicant has a strengthened immune system by the power of lemon.";
					public static LocString DESCRIPTION = "This duplicant has a strengthened immune system by the power of lemon.";
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

				public static class AKISEXTRATWITCHEVENTS_PANDORAIMMINENT
				{
					public static LocString NAME = "Uncontainable!";
					public static LocString TOOLTIP = "The energy inside this box is building up and will be released in {Time}.";
				}
			}

			public static class AKISEXTRATWITCHEVENTS_PIZZABOX
			{
				public static LocString NAME = "Pile of Pizzaboxes";
				public static LocString DESC = "Full of delicious pizza. \n\n" +
					"Duplicants will take out Pizzas as needed, or deliver them to a food storage if Pizza is enabled in the filters.";
			}

			public static class AKISEXTRATWITCHEVENTS_GIANTRADISH
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Radish of the Gods", GiantRadishConfig.ID);
				public static LocString DESC = "The singular radish.\n\n" +
					"Duplicants will take out radish slices as needed, or deliver them to a food storage if Raw Radish is enabled in the filters.";
			}
		}

		public static class ITEMS
		{
			public class AKISEXTRATWITCHEVENTS_PANDORASBOX
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Pandora's Box", PandorasBoxConfig.ID);
				public static LocString DESC = "A box of many wondrous and dangerous things. It's shaking with excitement! " +
					"\n\n" +
					"<i>Waiting too long to open it may not be a good idea...</i>";
			}
			public class PILLS
			{
				public class AKISEXTRATWITCHEVENTS_WEREVOLECURE
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Volesbane Tea", WereVoleCureConfig.ID);
					public static LocString DESC = "Cures a Werevole.";
				}
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
				public static LocString NAME = FUtility.Utils.FormatAsLink("Honey", Elements.Honey.ToString());
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

			public static class AETE_PIPIUM
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Pipium", Elements.Pipium.ToString());
				public static LocString DESC = $"A block of densely compressed {FUtility.Utils.FormatAsLink("Pips", SquirrelConfig.ID)}.";
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

			public static class AETE_YELLOWSNOW
			{
				public static LocString NAME = FUtility.Utils.FormatAsLink("Yellow Snow", Elements.YellowSnow.ToString());
				public static LocString DESC = "Mass of crystallized polluted water. ";
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

				public static class AKISEXTRATWITCHEVENTS_MAGICALFLOX
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink($"Magical Flox", MagicalFloxConfig.ID);
					public static LocString DESC = $"A mythical Flox plucked from the memories of this land.";
				}

#if WEREVOLE
				public static class AKISEXTRATWITCHEVENTS_WEREVOLE
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Werevole", WereVoleConfig.ID);
					public static LocString DESCRIPTION = "Once the daylight returns, this Duplicant will return to their original form.";
				}
#endif


				public static class AKISEXTRATWITCHEVENTS_GIANTCRAB
				{
					public static LocString NAME = FUtility.Utils.FormatAsLink("Ol' Crab Pal", GiantCrabConfig.ID);
					public static LocString DESCRIPTION = "This enormous Pokeshell has lived very long, and has grown to be a truly large lad.";
				}

				public static class AKISEXTRATWITCHEVENTS_POLYMORPHCRITTER
				{
					public static LocString NAME = "Critter (Transformed)";
					public static LocString DESC = "A polymorphed Duplicant. Cannot do errands in this state, but also requires no air or food. Does not know how to behave like the animal, so they will just chill around doing nothing.";
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
