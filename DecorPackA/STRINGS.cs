using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.StainedGlassTile;
using FLocalization;
using UnityEngine.UI;

namespace DecorPackA
{
	public class STRINGS
	{
		public static class TWITCH
		{
			public static class FLOOR_UPGRADE
			{
				public static LocString NAME = "Floor upgrade";
				public static LocString TOAST_GENERIC = "Your {RoomType} floor is being upgraded!";
				public static LocString TOAST_STABLE = "Critters can now also enjoy nice decor.";
			}
		}

		public class BUILDINGS
		{
			public class PREFABS
			{
				public class FLOWERVASEHANGINGFANCY
				{
					public class FACADES
					{
						public class DECORPACKA_COLORFUL
						{
							public static LocString NAME = Utils.FormatAsLink("Pink-Blue Crystal Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Twinkling colors!";
						}

						public class DECORPACKA_BLUEYELLOW
						{
							public static LocString NAME = Utils.FormatAsLink("Yellow-Green Crystal Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Fun colors!";
						}

						public class DECORPACKA_SHOVEVOLE
						{
							public static LocString NAME = Utils.FormatAsLink("Shove Vole Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Drilling colors!";
						}

						public class DECORPACKA_HONEY
						{
							public static LocString NAME = Utils.FormatAsLink("Honey Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Glistening colors!";
						}

						public class DECORPACKA_URANIUM
						{
							public static LocString NAME = Utils.FormatAsLink("Uranium Glass Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Radiant colors!";
						}
					}
				}

				public class DECORPACKA_GLASSSCULPTURE
				{
					public static LocString NAME = Utils.FormatAsLink("Glass Block", GlassSculptureConfig.ID);
					public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
					public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor") + ", contributing to " + Utils.FormatAsLink("Morale") + ".\n\nMust be sculpted by a Duplicant.";
					public static LocString POORQUALITYNAME = "\"Abstract\" Glass Sculpture";
					public static LocString AVERAGEQUALITYNAME = "Mediocre Glass Sculpture";
					public static LocString EXCELLENTQUALITYNAME = "Genius Glass Sculpture";

					public class FACADES
					{
						public class GOLEM
						{
							[Note("Playable character in Binding of Isaac: Repentance mod Fiend Folio:Reheated")]
							public static LocString NAME = Utils.FormatAsLink("Golem", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "From Fiend Folio: Reheated, from the hit mod for the hit game Binding of Isaac: Repentance.";
						}

						public class UNICORN
						{
							public static LocString NAME = Utils.FormatAsLink("Unicorn", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "Fabulus included.";
						}

						public class MUCKROOT
						{
							public static LocString NAME = Utils.FormatAsLink("Muckroot", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "A humble muckroot.";
						}

						public class SWAN
						{
							public static LocString NAME = Utils.FormatAsLink("Swan", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "A majestic swan.";
						}

						public class MEEP
						{
							public static LocString NAME = Utils.FormatAsLink("The creation of Meep", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "Historic piece. Very accurate.";
						}

						public class BROKEN
						{
							public static LocString NAME = Utils.FormatAsLink("Shattered dreams", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "An attempt was made.";
						}

						public class PIP
						{
							public static LocString NAME = Utils.FormatAsLink("Pip", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "Great guardian of the Acorns.";
						}

						public class POKESHELL
						{
							public static LocString NAME = Utils.FormatAsLink("Posh Pokeshell", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "Tribute to the true Daedric overloads of Oblicion: Posh Mudcrabs. (from the mod Posh Mudcrabs)";
						}

						public class HATCH
						{
							public static LocString NAME = Utils.FormatAsLink("Exquisite Chompers the 2nd", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "This time made of lasting glass!";
						}
					}
				}

				public class DECORPACKA_MOODLAMP
				{
					public static LocString NAME = Utils.FormatAsLink("Mood Lamp", Buildings.MoodLamp.MoodLampConfig.ID);
					public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
					public static LocString EFFECT = "Provides " + Utils.FormatAsLink("Light") + " when " + Utils.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";

					public class VARIANT
					{
						public static LocString RANDOM = "Random";

						// v1.0
						public static LocString UNICORN = "Unicorn";
						public static LocString MORB = "Morb";
						public static LocString DENSE = "Dense Puft";
						public static LocString MOON = "Moon";
						[Note("Former ONI Youtuber")]
						public static LocString BROTHGAR = "Brothgar Logo";
						public static LocString SATURN = "Saturn";
						public static LocString PIP = "Pip";
						public static LocString D6 = "D6";
						[Note("Hollow Knight enemy")]
						public static LocString OGRE = "Shrumal Ogre";
						[Note("Marvel Infinity Stone")]
						public static LocString TESSERACT = "Tesseract";
						public static LocString CAT = "Cat";
						[Note("From ONI mod I love Slicksters by Pholith")]
						public static LocString OWO = "OwO Slickster";
						public static LocString STAR = "Star";

						public static LocString ROCKET = "Rocket";

						// v1.2
						[Note("ONI Youtuber")]
						public static LocString LUMAPLAYS = "Luma Plays Logo";
						[Note("ONI Streamer")]
						public static LocString KONNY87 = "Konny87 Logo";
						[Note("Minecraft")]
						public static LocString REDSTONE_LAMP = "Redstone Lamp";
						[Note("Hollow Knight background object from the area \"Teacher's Archives\"")]
						public static LocString ARCHIVE_TUBE = "Archive Tube";
						public static LocString KLEI_MUG = "Klei Mug";
						[Note("From the ONI mod Hatch Morphs by ✯ Erny ✯")]
						public static LocString DIAMONDHATCH = "Diamond Hatch";
						[Note("From the ONI mod Twitch Integration by asquared31415")]
						public static LocString GLITTERPUFT = "Glitter Puft";
						[Note("From the ONI mod AI Controlled Rockets (DLC) by Sgt_Imalas")]
						public static LocString AI = "AI in a jar";
						[Note("From the not yet released ONI mod Slag")]
						public static LocString SLAGMITE = "Slagmite"; // critter from a WIP mod
						public static LocString CUDDLE_PIP = "Cuddle Pip";

						// 1.4.4
						[Note("Noita enemy")]
						public static LocString HAMIS = "Hämis"; // Noita enemy
						[Note("Don't Starve creature")]
						public static LocString BABY_BEEFALO = "Baby Beefalo"; // Don't Starve

						// v1.4.6
						[Note("from Terraria")]
						public static LocString HEART_CRYSTAL = "Heart Crystal"; // Terraria
						[Note("From the not yet released ONI mod Beached")]
						public static LocString GREEN_JELLY = "Rad Jellyfish"; // Beached
						[Note("From the not yet released ONI mod Beached")]
						public static LocString BLUE_JELLY = "Jellyfish"; // Beached
						[Note("from League of Legends")]
						public static LocString PORO = "Poro"; // League of Legends

						// v1.5
						// Shove Vole takes from the critter

					}
				}

				public class DECORPACKA_DEFAULTSTAINEDGLASSTILE
				{
					public static LocString NAME = Utils.FormatAsLink(Utils.FormatAsLink("Stained Glass Tile", DefaultStainedGlassTileConfig.DEFAULT_ID));
					public static LocString STAINED_NAME = Utils.FormatAsLink("{element} Stained Glass Tile", DefaultStainedGlassTileConfig.DEFAULT_ID);
					public static LocString DESC = $"Stained glass tiles are transparent tiles that provide a fashionable barrier against liquid and gas.";
					public static LocString EFFECT = $"Used to build the walls and floors of rooms.\n\n" +
						$"Allows {Utils.FormatAsLink("Light")} and {Utils.FormatAsLink("Decor")} pass through.";
				}
			}
		}

		public class UI
		{
			public class CODEX
			{
				public static LocString PALETTE = "Stained Glass Palette";

				public class CATEGORYNAMES
				{
					public static LocString MODS = "Mods";
				}
			}

			public class UISIDESCREENS
			{
				public class MOODLAMP_SIDE_SCREEN
				{
					public static LocString TITLE = "Lamp type";
				}
			}

			public class BUILDINGEFFECTS
			{
				public static LocString THERMALCONDUCTIVITYCHANGE = "Thermal Conductivity: {0}";

				public class TOOLTIP
				{
					public static LocString HIGHER = "higher";
					public static LocString LOWER = "lower";

					public static LocString THERMALCONDUCTIVITYCHANGE = "The dye {dyeElement} has {higherOrLower} thermal conductivity than {baseElement}, modifying it by {percent}.";
				}
			}

			public class USERMENUACTIONS
			{
				public class FABULOUS
				{
					public class ENABLED
					{
						public static LocString NAME = "Fabulous On";
						public static LocString TOOLTIP = "Bring the magic!";
					}
					public class DISABLED
					{
						public static LocString NAME = "Fabulous Off";
						public static LocString TOOLTIP = "Take away the magic.";
					}
				}

				public class HAMIS_MAID
				{
					public class ENABLED
					{
						public static LocString NAME = "Upgrade";
						public static LocString TOOLTIP = "Perfection";
					}
					public class DISABLED
					{
						public static LocString NAME = "Downgrade";
						public static LocString TOOLTIP = "Back to normalcy.";
					}
				}
			}
		}

		public class MISC
		{
			public class TAGS
			{
				public static LocString DECORPACKA_STAINEDGLASSMATERIAL = "Glass Dye";
			}
		}
	}
}
