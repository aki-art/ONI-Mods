using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.StainedGlassTile;
using FUtility.FLocalization;

namespace DecorPackA
{
	public class STRINGS
	{
		public static LocString DESIGN_BY_DECORPACKA = "\n\n<color=#ec9c03><b>Design added by Decor Pack I.</b></color>";

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
				public class DININGTABLE
				{
					public class FACADES
					{
						public class DECORPACKA_DININGTABLE_GLASS
						{
							public static LocString NAME = Utils.FormatAsLink("Glass Table", DiningTableConfig.ID);
							public static LocString DESC = "It is much more difficult to stick chewed gum under this table unnoticed.";
						}
					}
				}

				public class DOOR
				{
					public class FACADES
					{
						public class DECORPACKA_PNEUMATICDOOR_GLASS
						{
							public static LocString NAME = Utils.FormatAsLink("See-through Pneumatic Door", DoorConfig.ID);
							public static LocString DESC = "A fancy glass door.";
						}

						public class DECORPACKA_PNEUMATICDOOR_STAINEDGREEN
						{
							public static LocString NAME = Utils.FormatAsLink("Yellow-Green Stained Pneumatic Door", DoorConfig.ID);
							public static LocString DESC = "A fancy door with yellow and green stained glass panes.";
						}

						public class DECORPACKA_PNEUMATICDOOR_STAINEDRED
						{
							public static LocString NAME = Utils.FormatAsLink("Reddish Stained Pneumatic Door", DoorConfig.ID);
							public static LocString DESC = "A fancy door with red, yellow and blue stained glass panes.";
						}

						public class DECORPACKA_PNEUMATICDOOR_STAINEDPURPLE
						{
							public static LocString NAME = Utils.FormatAsLink("Purplish Stained Pneumatic Door", DoorConfig.ID);
							public static LocString DESC = "A fancy door with purple and blue stained glass panes.";
						}

						public class DECORPACKA_PNEUMATICDOOR_STAINEDVERYPURPLE
						{
							public static LocString NAME = Utils.FormatAsLink("Purple Stained Pneumatic Door", DoorConfig.ID);
							public static LocString DESC = "A fancy door with purple stained glass panes.";
						}
					}
				}

				public class FLOWERVASEHANGINGFANCY
				{
					public class FACADES
					{
						public class DECORPACKA_FLOWERVASEHANGINGFANCY_COLORFUL
						{
							public static LocString NAME = Utils.FormatAsLink("Pink-Blue Crystal Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Twinkling colors!";
						}

						public class DECORPACKA_FLOWERVASEHANGINGFANCY_BLUEYELLOW
						{
							public static LocString NAME = Utils.FormatAsLink("Yellow-Green Crystal Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Fun colors!";
						}

						public class DECORPACKA_FLOWERVASEHANGINGFANCY_SHOVEVOLE
						{
							public static LocString NAME = Utils.FormatAsLink("Shove Vole Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Drilling colors!";
						}

						public class DECORPACKA_FLOWERVASEHANGINGFANCY_HONEY
						{
							public static LocString NAME = Utils.FormatAsLink("Honey Pot", FlowerVaseHangingFancyConfig.ID);
							public static LocString DESC = "Glistening colors!";
						}

						public class DECORPACKA_FLOWERVASEHANGINGFANCY_URANIUM
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

						public class HOUND
						{
							[Note("Don't Starve enemy")]
							public static LocString NAME = Utils.FormatAsLink("Hound", GlassSculptureConfig.ID);
							public static LocString DESCRIPTION = "If you listen closely, you can hear growling... getting louder...";
						}

						public class EXCALIBURVOLE
						{
							[Note("Reference to Soul Eater character Excalibur")]
							public static LocString NAME = Utils.FormatAsLink("Legendary Shove-Volibur", GlassSculptureConfig.ID);

							[Note("Script from the anime Soul Eater, monologue of the Excalibur.")]
							public static LocString DESCRIPTION = "Fool. My legend dates back to the twelfth century you see. " +
								"It began on a midsummers day with the sun blazing overhead. No, wait. It was a blustery Autumn day. I was the " +
								"unsavory fellow back when it all started. Which was in the winter as I recall. I remember the tough crowd, " +
								"all the hot babes fought over me that summer. Yes. Yes that's right, it was summer. A scorching midsummer day." +
								" I was a dangerous man at the time. And yet, refined somehow. Everyone thought so. They still think so to this day." +
								" Although, maybe they didn't think so as much back then. But they definitely said I was dangerous, I'm sure of it..." +
								" And I know I've always been refined so they must have been thinking it. Yes. Yes indeed. " +
								"Everyone thought and said and talked about how amazing I was. I'm still amazing of course, but no longer " +
								"the bad boy I was back then. On that chilled, frozen winter day. I will continue the re-telling of my legend; " +
								"But first, there will be a five minute break. Stand still and await my return.";
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

					public class FACADES
					{
						public class DECORPACKA_MOODLAMP_GLASS
						{
							public static LocString NAME = Utils.FormatAsLink("Glass Desk", DoorConfig.ID);
							public static LocString DESC = "Goes well with the Gravitas Lounge Tables.";
						}

						public class DECORPACKA_MOODLAMP_ROBOTICS
						{
							public static LocString NAME = Utils.FormatAsLink("Gravitas Robotics Desk", DoorConfig.ID);
							public static LocString DESC = "Designed by Robots, for Robots, of Robots!.";
						}

						public class DECORPACKA_MOODLAMP_BOILER
						{
							public static LocString NAME = Utils.FormatAsLink("Steampunk Desk", DoorConfig.ID);
							public static LocString DESC = "This desk is missing a few cogs.";
						}

						public class DECORPACKA_MOODLAMP_MODERNORANGE
						{
							public static LocString NAME = Utils.FormatAsLink("Orange Modern Desk", DoorConfig.ID);
							public static LocString DESC = "A sleek orange desk.";
						}

						public class DECORPACKA_MOODLAMP_MODERNBLUE
						{
							public static LocString NAME = Utils.FormatAsLink("Blue Modern Desk", DoorConfig.ID);
							public static LocString DESC = "A sleek blue desk.";
						}

						public class DECORPACKA_MOODLAMP_MODERNPURPLE
						{
							public static LocString NAME = Utils.FormatAsLink("Purple Modern Desk", DoorConfig.ID);
							public static LocString DESC = "A sleek purple desk.";
						}

						public class DECORPACKA_MOODLAMP_TOUCHSTONE
						{
							public static LocString NAME = Utils.FormatAsLink("Touchstone Desk", DoorConfig.ID);
							public static LocString DESC = "A Duplicant found this weird stone slab in a cave. They weren't sure what it was made for, but it sure looks neat as a decorative table top.";
						}

						public class DECORPACKA_MOODLAMP_THULECITE
						{
							public static LocString NAME = Utils.FormatAsLink("Thulecite Desk", DoorConfig.ID);
							public static LocString DESC = "Made of a strange glimmering yellow material, this table looks ancient and mystical.";
						}

						public class DECORPACKA_MOODLAMP_TREETRUNK
						{
							public static LocString NAME = Utils.FormatAsLink("Tree Trunk", DoorConfig.ID);
							public static LocString DESC = "The stub of an old tree.";
						}

						public class DECORPACKA_MOODLAMP_INDUSTRIAL
						{
							public static LocString NAME = Utils.FormatAsLink("Metal Desk", DoorConfig.ID);
							public static LocString DESC = "Who knew a pile of corroded metal scraps make great home decor!";
						}
					}

					public class CATEGORIES
					{
						public static LocString CRITTERS = "Critters";
						public static LocString MODS = "Mods";
						public static LocString FIGURES = "Figures";
						public static LocString SPACE = "Space";
						public static LocString MEDIA = "Movies & Games";
						public static LocString CREATORS = "Youtube & Twitch";
						public static LocString CUSTOMIZABLE = "Customizable";
						public static LocString MISC = "Miscellaneous";
					}

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
						[Note("From Terraria")]
						public static LocString HEART_CRYSTAL = "Heart Crystal"; // Terraria
						[Note("From the not yet released ONI mod Beached")]
						public static LocString GREEN_JELLY = "Rad Jellyfish"; // Beached
						[Note("From the not yet released ONI mod Beached")]
						public static LocString BLUE_JELLY = "Jellyfish"; // Beached
						[Note("from League of Legends")]
						public static LocString PORO = "Poro"; // League of Legends

						// v1.5
						// Shove Vole takes from the critter
						[Note("Creature from Lobotomy Corporation")]
						public static LocString BIGBIRD = "Big Bird";
						public static LocString DISCOBALL = "Disco Ball";
						public static LocString SCATTERING = "Scattering Lights";
						public static LocString SCATTERINGYELLOW = "Cozy Stars";
						public static LocString SCATTERINGBLUE = "Arcane Stars";
						public static LocString SCATTERINGGREEN = "Radioactivity";
						public static LocString SCATTERINGPURPLE = "Slickster Twister";
						[Note("Modded companion for Skyrim")]
						public static LocString INIGO = "Inigo";
						[Note("Directional arrows, not the shooty ones")]
						public static LocString ARROW = "Arrow";
						[Note("Moddedentity from ONI Mod Diseases Expanded")]
						public static LocString NANOBOT = "Nanobot";
						[Note("DreadVoidWitch & LiveActionPixel, variety streamers, Pixel does ONI")]
						public static LocString DREADPIXEL = "Dreadlamp'O'Love";
						[Note("A joke lamp for LiveActionPixel, who has not sent his moodlamp request for almost a year :D")]
						public static LocString SOON = "Live Action Pixel Definitely 100% Official<sub>TM</sub> Logo";
						public static LocString SPHERE = "Bubble Gum";
						public static LocString CROSS = "Cross";
						public static LocString MUFFIN = "Muffin";
						public static LocString ROCKET2 = "Liftoff!";
						public static LocString FLOX = "Fluff";
						public static LocString GLACIER = "100 year old Glacier";
						public static LocString SPIGOT = "Sealantern";
						public static LocString CUBE = "Cuboid Thing";
						public static LocString GLOMMER = "Glommer";
						public static LocString CANDLE = "Candles";

						public static LocString CREDIT = "{Lamp}\n" +
							"\n" +
							"<size=80%><color=#db8f23>for {Name}</color></size>";
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
			public static class DECORPACKA_SETTINGSDIALOGB_TMPCONVERTED
			{
				public static LocString REQUIRE_RESTART = "Changes will be applied on game restart.";

				public static class BUTTONS
				{
					public class OK
					{
						public static LocString TEXT = "Apply";
					}

					public class CANCELBUTTON
					{
						public static LocString TEXT = "Cancel";
					}

					public class STEAMBUTTON
					{
						public static LocString TEXT = "Steam Workshop";
					}

					public class GITHUBBUTTON
					{
						public static LocString TEXT = "Github";
					}
				}

				public static class TITLE
				{
					public static LocString TITLETEXT = "Decor Pack I";
				}
			}

			public static class DECORPACKA_SETTINGS
			{
				public static LocString DECOR = "Decor";
				public static LocString RANGE = "Range";
				public static LocString VIBRANT = "Vibrant Colors";
				public static LocString ENABLE_LIGHTRAYS = "Lightrays";
				public static LocString ENABLE_LIGHTRAYS_TOOLTIP = "Render the light rays when the lamp is on.";
				public static LocString VIBRANT_TOOLTIP = "Moodlamps emit stronger, mre colorful light than vanilla buildings.";
				public static LocString LUX = "Lux";
				public static LocString LUX_RANGE = "Lux Range";
				public static LocString EXHAUSTKILOWATTSWHENACTIVE = "Exhaust kW";
				public static LocString EXHAUSTKILOWATTSWHENACTIVE_TOOLTIP = "Exhaust kilowatts when active";
				public static LocString ENERGY = "Power Use";
				public static LocString ENERGY_TOOLTIP = "Energy Consumption in Watts when active";
				public static LocString SELFHEAT_KW = "Self heat";
				public static LocString SELFHEAT_KW_TOOLTIP = "Self heating kilowatts when active";
				public static LocString USE_DYE_TC = "Use Dye TC";
				public static LocString USE_DYE_TC_TOOLTIP = "Use Dye Thermal Conductivity Values when calculating TC for this tile.";
				public static LocString NERF_ABYSSALITE = "Nerf Abyssalite";
				public static LocString NERF_ABYSSALITE_TOOLTIP = "Nerf Abyssalite TC to prevent cheese, makig it only useful for aesthetics.";
				public static LocString DISABLE_COLORSHIFT = "Disable Color Shift";
				public static LocString DISABLE_COLORSHIFT_TOOLTIP = "Disable Color Shift effect when moving around the camera for certain materials, such as Neutronium Alloy of Rocketry Expanded. Very minor performance impact.";
				public static LocString INSULATE_STORAGE = "Insulate Construction Storage";
				public static LocString INSULATE_STORAGE_TOOLTIP = "Temperature Insulated construction storage prevents ice from melting or Magma from continously leaking heat and solidifying while waiting for a construction order.";
				public static LocString DYE_RATIO = "Dye Ratio";
				public static LocString DYE_RATIO_TOOLTIP = "Dye Material used to construct a tile relative to the Glass.";
				public static LocString SPEED = "Speed Bonus";
				public static LocString SPEED_TOOLTIP = "How much faster can duplicants walk on this tile.";
			}

			public class KLEI_INVENTORY_SCREEN
			{
				public class SUBCATEGORIES
				{
					public static LocString DECORPACKA_BUILDING_MOODLAMPDESK = "Moodlamp Desks";
					public static LocString DINING_TABLE = "Mess Tables";
				}
			}

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

				public class SCATTER_LIGHT
				{
					public class ENABLED
					{
						public static LocString NAME = "Enable Light Show";
						public static LocString TOOLTIP = "Pretty Lights!";
					}

					public class DISABLED
					{
						public static LocString NAME = "Disable Light Show";
						public static LocString TOOLTIP = "Regular lamp with no bright lights.";
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
