﻿using HarmonyLib;
using System.Collections.Generic;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP;

namespace DecorPackA.Buildings.MoodLamp
{
	public class LampVariants : ResourceSet<LampVariant>
	{
		public static List<LampVariant> modAddedMoodlamps;

		public const string
			CRITTERS = "critters",
			MODS = "mods",
			FIGURES = "figures",
			SPACE = "space",
			MEDIA = "media",
			CREATORS = "creators",
			CUSTOMIZABLE = "customizable",
			MISC = "misc";

		public Dictionary<string, int> categorySortOrders = new()
		{
			{ CRITTERS, 0 },
			{ FIGURES, 10 },
			{ SPACE, 20 },
			{ MODS, 30 },
			{ MEDIA, 40 },
			{ CUSTOMIZABLE, 50 },
			{ CREATORS, 60 },
			{ MISC, 70 },
		};

		public LampVariants()
		{
			Add(new LampVariant("unicorn", VARIANT.UNICORN, 2.25f, 0, 2.13f, FIGURES));
			Add(new LampVariant("morb", VARIANT.MORB, .27f, 2.55f, .08f, CRITTERS));
			Add(new LampVariant("dense", VARIANT.DENSE, 0.07f, 0.98f, 3.35f, CRITTERS));
			Add(new LampVariant("moon", VARIANT.MOON, 1.09f, 1.25f, 1.94f, SPACE));
			Add(new LampVariant("brothgar", VARIANT.BROTHGAR, 2.47f, 1.75f, .62f, CREATORS));
			Add(new LampVariant("saturn", VARIANT.SATURN, 2.24f, 1.33f, .58f, SPACE));
			Add(new LampVariant("pip", VARIANT.PIP, 2.47f, 1.75f, .62f, CRITTERS));
			Add(new LampVariant("d6", VARIANT.D6, 2.73f, 0.35f, .60f, MEDIA));
			Add(new LampVariant("ogre", VARIANT.OGRE, 1.14f, 1.69f, 1.94f, MEDIA));
			Add(new LampVariant("tesseract", VARIANT.TESSERACT, 0.09f, 1.2f, 2.44f, MEDIA, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("cat", VARIANT.CAT, 2.55f, 2.22f, 1.48f, FIGURES));
			Add(new LampVariant("owo", VARIANT.OWO, 2.55f, 1.13f, 2.2f, MODS));
			Add(new LampVariant("star", VARIANT.STAR, 2.47f, 1.75f, .62f, SPACE));

			// v1.2
			Add(new LampVariant("konny87", VARIANT.KONNY87, 0.14f, 1.46f, 2.55f, CREATORS));
			Add(new LampVariant("kleimug", VARIANT.KLEI_MUG, 2.55f, 1f, 0, MEDIA));
			Add(new LampVariant("redstonelamp", VARIANT.REDSTONE_LAMP, 2.55f, 1f, 0, MEDIA));
			Add(new LampVariant("cuddlepip", VARIANT.CUDDLE_PIP, 1.11f, 0.35f, 2.05f, CRITTERS));
			Add(new LampVariant("archivetube", VARIANT.ARCHIVE_TUBE, 0.38f, 2.55f, 0.58f, MEDIA, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("lumaplays", VARIANT.LUMAPLAYS, 0.96f, 2.55f, 0.1f, CREATORS));
			Add(new LampVariant("diamondhatch", VARIANT.DIAMONDHATCH, 0.55f, 0.75f, 2.01f, MODS));
			Add(new LampVariant("beeta", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.BEE.NAME), 0, 2.55f, 0, CRITTERS));
			Add(new LampVariant("glitterpuft", VARIANT.GLITTERPUFT, 0, 0, 0, MODS, mode: KAnim.PlayMode.Loop)
				.ToggleComponent<GlitterLight2D>());

			Add(new LampVariant("ai", VARIANT.AI, 0.38f, 2.55f, 0.58f, MODS, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("slagmite", VARIANT.SLAGMITE, 1.14f, 1.69f, 1.94f, MODS));

			// v1.4.4
			Add(new LampVariant("hamis", VARIANT.HAMIS, 0.98f, 0.55f, 1.36f, MEDIA))
				.ToggleComponent<Hamis>();
			Add(new LampVariant("babybeefalo", VARIANT.BABY_BEEFALO, 2.27f, 1.45f, 0.64f, MEDIA));

			// v1.4.6
			Add(new LampVariant("heart_crystal", VARIANT.HEART_CRYSTAL, 3f, 0.4f, 0.8f, MEDIA, mode: KAnim.PlayMode.Loop))
				.ToggleComponent<ScatterLightLamp>();

			Add(new LampVariant("green_jelly", VARIANT.GREEN_JELLY, 0.58f, 2.53f, 0.52f, MODS));
			Add(new LampVariant("blue_jelly", VARIANT.BLUE_JELLY, 0, 1.03f, 3.3f, MODS));
			Add(new LampVariant("poro", VARIANT.PORO, 1.2f, 1.2f, 1.4f, MEDIA))
				.ToggleComponent<FPSMeter>();

			// v1.5
			Add(new LampVariant("shovevole", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.MOLE.NAME), 1.3f, 0.49f, 1.37f, CRITTERS));
			Add(new LampVariant("bigbird", VARIANT.BIGBIRD, 2.55f, 1.97f, 0.41f, MEDIA, mode: KAnim.PlayMode.Paused))
				.ToggleComponent<BigBird>();
			Add(new LampVariant("discoball", VARIANT.DISCOBALL, 0, 0, 0, MISC, mode: KAnim.PlayMode.Loop))
				.ToggleComponent<GlitterLight2D>()
				.ToggleComponent<ScatterLightLamp>();
			Add(new LampVariant("scattering", VARIANT.SCATTERING, 1f, 1f, 1f, mode: KAnim.PlayMode.Loop))
				.ToggleComponent<TintableLamp>()
				.ToggleComponent<ScatterLightLamp>();
			Add(new LampVariant("moo", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.MOO.NAME), 1.05f, 1.94f, 1.73f, CRITTERS));
			Add(new LampVariant("inigo", VARIANT.INIGO, 0.15f, 0.41f, 2.42f, MEDIA));
			Add(new LampVariant("arrow", VARIANT.ARROW, 1f, 1f, 1f, CUSTOMIZABLE))
				.ToggleComponent<RotatableLamp>()
				.ToggleComponent<TintableLamp>();
			Add(new LampVariant("nanobot", VARIANT.NANOBOT, 2.55f, 0.99f, 2.14f, MODS, mode: KAnim.PlayMode.Loop));

			Add(new LampVariant("dreadpixel", VARIANT.DREADPIXEL, 2.55f, 0, 1.7f, CREATORS, mode: KAnim.PlayMode.Loop)
				.ShiftColors(2.55f, 0, 0, 7f)
				.ToggleComponent<ShiftyLight2D>());

			Add(new LampVariant("soon", VARIANT.SOON, 0, 2.55f, 1.98f));
			Add(new LampVariant("sphere", VARIANT.SPHERE, 1f, 1f, 1f, CUSTOMIZABLE)
				.ToggleComponent<TintableLamp>());

			Add(new LampVariant("pacu", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.PACU.BABY.NAME), 1.47f, 2.06f, 1.24f, CRITTERS));
			Add(new LampVariant("pacucold", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME), 0, 1.89f, 1.89f, CRITTERS));
			Add(new LampVariant("pacutropical", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME), 2.55f, 1.41f, 1.10f, CRITTERS));

			modAddedMoodlamps?.Do(moddedLamp => Add(moddedLamp));
			modAddedMoodlamps = null;
		}

		public LampVariant GetRandom() => resources.GetRandom();
	}
}
