﻿using DecorPackB.Content.Defs.Buildings;

namespace DecorPackB
{
	public class STRINGS
	{
		public static LocString DESIGN_BY_DECORPACKB = "\n\n<color=#ec9c03><b>Design added by Decor Pack II.</b></color>";

		public class ITEMS
		{
			public class DECORPACKB_FOSSILNODULE
			{
				public static LocString NAME = "Fossil Nodule";
				public static LocString DESC = "...";
			}
		}

		public static class EFFECTS
		{
			public static class DECORPACKB_INSPIRED_LOW
			{
				public static LocString NAME = "Midly Curious";
				public static LocString DESC = "This duplicant has seen a somewhat interesting thing.";
			}

			public static class DECORPACKB_INSPIRED_OKAY
			{
				public static LocString NAME = "Inspired";
				public static LocString DESC = "This duplicant has seen a decently interesting thing.";
			}

			public static class DECORPACKB_INSPIRED_GREAT
			{
				public static LocString NAME = "Awestruck";
				public static LocString DESC = "This duplicant has seen amazing discoveries and cannot wait to learn more about their world.";
			}

			public static class DECORPACKB_INSPIRED_GIANT
			{
				public static LocString NAME = "Expanded mind";
				public static LocString DESC = "This duplicant is greatly inspired by the wonders of this world.";
			}
		}

		public class TWITCH
		{
			public class LUCKY_POTS
			{
				public static LocString NAME = "Lucky Pots";
				public static LocString TOAST = "Pots Pots Pots Pots Pots Pots Pots Pots";
			}
		}

		public class BUILDINGS
		{
			public class PREFABS
			{
				public class DECORPACKB_FOSSILDISPLAY
				{
					public static LocString NAME = Utils.FormatAsLink("Fossil Display", FossilDisplayConfig.ID);
					public static LocString DESC = "Duplicants who have learned research skills can produce more accurate reconstructions.";
					public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";

					public static LocString ASSEMBLEDBY = "Reconstruction by {duplicantName}";

					public class VARIANT
					{
						public static class HUMAN
						{
							public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Human Skeleton", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "Humans were a species that went extinct in a war against Dodos 4 million years ago. \n" +
								"They had long icky arms with 5 fingers which they used to consume their only nutrition, Cheeto, a type of orange worm. \n" +
								"\n" +
								"This seems inaccurate.";
						}

						public static class DOGGY
						{
							public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Dog Skeleton", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "" +
								"\n" +
								"This seems inaccurate.";
						}


						public static class SPIDER
						{
							public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Spider Skeleton", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "The Spider was a creature obsessed with wrapping other creatures in warm and silky blankets, as a way to show it's friendly nature.\n" +
								"Some species were well known for their incredible pastries.\n" +
								"\n" +
								"This seems inaccurate.";
						}

						public static class UNICORN
						{
							public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Unicorn Skeleton", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "..." +
								"\n" +
								"This seems inaccurate.";
						}

						public static class PAWPRINT
						{
							public static LocString NAME = Utils.FormatAsLink("Mud Prints", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "Various imprints of paw and feet marks.";
						}

						public static class PACU
						{
							public static LocString NAME = Utils.FormatAsLink("Three Pacus", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "<i>Pacumidae</i> are a group of ancient fish that once lived on these Asteroids. " +
								"If only such beautiful creatures could still grace our world...";
						}

						public static class DODO
						{
							public static LocString NAME = Utils.FormatAsLink("Dodo", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class TRILOBITE
						{
							public static LocString NAME = Utils.FormatAsLink("Trilobites", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class MINIPARA
						{
							public static LocString NAME = Utils.FormatAsLink("Tiny Parasaurolophus", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "A regular Parasaurolophus skeleton which appears to have been hit by a shrink ray. " +
								"That, or the artist misjudged the scale.";
						}

						public static class BEEFALO
						{
							public static LocString NAME = Utils.FormatAsLink("Beefalo", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "Ancient humans used to settle around Beefalo herds.";
						}

						public static class HELLHOUND
						{
							public static LocString NAME = Utils.FormatAsLink("Hell Hound", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class CATCOON
						{
							public static LocString NAME = Utils.FormatAsLink("Catcoon", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class GLOMMER
						{
							public static LocString NAME = Utils.FormatAsLink("Oddly cute ancient insect", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}


						public static class VOLGUS
						{
							public static LocString NAME = Utils.FormatAsLink("Volgus", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class MICRORAPTOR
						{
							public static LocString NAME = Utils.FormatAsLink("Microraptor", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class AMMONITE
						{
							public static LocString NAME = Utils.FormatAsLink("Ammonite", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class ANCIENTSPECIMENT
						{
							public static LocString NAME = Utils.FormatAsLink("Ancient Specimen", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class PAWPRINTS
						{
							public static LocString NAME = Utils.FormatAsLink("Mudprints", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class ANCIENTSPECIMENAMBER
						{
							public static LocString NAME = Utils.FormatAsLink("Ancient Amber Inclusion", FossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}
					}
				}

				public class DECORPACKB_GIANTFOSSILDISPLAY
				{
					public static LocString NAME = Utils.FormatAsLink("Giant Fossil Display", GiantFossilDisplayConfig.ID);
					public static LocString DESC = "Duplicants who have learned research skills can produce more accurate reconstructions.";
					public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";

					public class VARIANT
					{
						public static class BRONTO
						{
							public static LocString NAME = Utils.FormatAsLink("Brontosaurus", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class TREX
						{
							public static LocString NAME = Utils.FormatAsLink("Tyrannosaurus Rex", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class TRICERATOPS
						{
							public static LocString NAME = Utils.FormatAsLink("Triceratops", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class PTERODACTYL
						{
							public static LocString NAME = Utils.FormatAsLink("Pterodactyl", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class SPINOSAURUS
						{
							public static LocString NAME = Utils.FormatAsLink("Spinosaurus", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class DEERCLOPS
						{
							public static LocString NAME = Utils.FormatAsLink("Deerclops", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class PUGALISK
						{
							public static LocString NAME = Utils.FormatAsLink("Pugalisk", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "...";
						}

						public static class LIVAYATAN
						{
							public static LocString NAME = Utils.FormatAsLink("Livayatan", GiantFossilDisplayConfig.ID);
							public static LocString DESCRIPTION = "Livayatan Melville was one of the largest predators to have every existed. They were a type of whale, named after the mythical Leviathan and the author of Moby Dick, Herman Melville. Livayatans most likely went extinct in the late Miocene due to globally declining temperatures.";
						}
					}
				}

				public class DECORPACKB_POT
				{
					public static LocString NAME = Utils.FormatAsLink("Clay Block", PotConfig.ID);
					public static LocString DESC = "Duplicants who have learned art skills can produce more decorative clay pottery.";
					public static LocString EFFECT = "Stores materials, while also increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";

					public class VARIANT
					{
						public class ANGRYLETTUCE
						{
							public static LocString NAME = "Lettuce at peace";
							public static LocString DESCRIPTION = "This pip has found inner piece in being a clay pot.";
						}

						public class HATCH
						{
							public static LocString NAME = "Hatch Pot";
							public static LocString DESCRIPTION = "A gaggle of hatches decorate this intricate pottery piece.";
						}

						public class MORB
						{
							public static LocString NAME = "Slimy Pot";
							public static LocString DESCRIPTION = "The extra shiny glaze gives these morbs an eerily realistic look.";
						}

						public class SWIRLIES
						{
							public static LocString NAME = "Fun Swirly Pot";
							public static LocString DESCRIPTION = "This abstract pot design is elegant in its simplicity.";
						}

						public class SWIRLIES_PURPLE
						{
							public static LocString NAME = "Purple Swirly Pot";
							public static LocString DESCRIPTION = "The purples really bring out the beauty of the Mealwood Seeds inside the pot.";
						}

						public class SWIRLIES_BLUEGOLD
						{
							public static LocString NAME = "Blue-Gold Swirly Pot";
							public static LocString DESCRIPTION = "Shimmering hypnotizing swirls on a Clay Pot.";
						}

						public class PINKYFLUFFYLETTUCE
						{
							public static LocString NAME = "Fluffy Pot";
							public static LocString DESCRIPTION = "Somehow the duplicant who crafted this was able to achieve a soft clay surface... wait, is this unfired clay???";
						}

						public class GENERIC_TALL
						{
							public static LocString NAME = "Tall Pot";
							public static LocString DESCRIPTION = "A simple tall pot.";
						}

						public class MUCKROOT
						{
							public static LocString NAME = "Mucky Pot";
							public static LocString DESCRIPTION = "Truly, the most prestigeous patterns of all time: The Muckroot.";
						}

						public class RECTANGULAR
						{
							public static LocString NAME = "Cubular Pot";
							public static LocString DESCRIPTION = "The artist didn't really have any inspiration for this one...";
						}

						public class WEIRD
						{
							public static LocString NAME = "Weird Pot";
							public static LocString DESCRIPTION = "A strangely shaped pot. Surely, an intentional design by the artist to express their deepest emotions.";
						}

						public class RED
						{
							public static LocString NAME = "Red Pot";
							public static LocString DESCRIPTION = "A pot made to look intentionally old and ancient. Or so the artist says.";
						}
					}
				}

				public class DECORPACKB_FOUNTAIN
				{
					public static LocString NAME = Utils.FormatAsLink("Fountain Kit", "");
					public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
					public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
					public static LocString POORQUALITYNAME = "\"Abstract\" Fountain";
					public static LocString AVERAGEQUALITYNAME = "Mediocre Fountain";
					public static LocString GENIUSQUALITYNAME = "Genius Fountain";
				}

				public class DECORPACKB_OILLANTERN
				{
					public static LocString NAME = Utils.FormatAsLink("Lantern", "");
					public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
					public static LocString EFFECT = "Provides " + Utils.FormatAsLink("Light", "LIGHT") + " when supplied with {Element}.\n\nDuplicants can operate buildings more quickly when the building is lit.";
				}
			}
		}

		public class MISC
		{
			public class TAGS
			{
				public static LocString FOSSILBUILDING = "Fossil";
				public static LocString FOSSIL = "Fossil";
				public static LocString DECORPACKB_BUILDINGFOSSILNODULE = "Fossil Nodule";
				public static LocString DECORPACKB_BUILDINGFOSSILNODULE_DESC = "An important hint for the construction of a larger structure.";
				public static LocString DECORPACKB_FOSSILMATERIAL = "Fossil";
				public static LocString DECORPACKB_FOSSILMATERIAL_DESC = "Fossilized imprints";
			}
		}

		public class UI
		{
			public class KLEI_INVENTORY_SCREEN
			{
				public class SUBCATEGORIES
				{
					public static LocString DECORPACKB_FOSSILS = "Fossils";
					public static LocString DECORPACKB_POTS = "Pottery";
					public static LocString DECORPACKB_FOUNTAINS = "Fountains";
				}
			}

			public static class TOOLTIPS
			{
				public static LocString HELP_BUILDLOCATION_ON_ANY_WALL = "Must be placed on the ground, ceiling, or next to a wall.";
			}
		}
	}
}
