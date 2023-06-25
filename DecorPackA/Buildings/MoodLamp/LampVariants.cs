using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP;

namespace DecorPackA.Buildings.MoodLamp
{
	public class LampVariants : ResourceSet<LampVariant>
	{
		public LampVariants()
		{
			Add(new LampVariant("unicorn", VARIANT.UNICORN, 2.25f, 0, 2.13f));
			Add(new LampVariant("morb", VARIANT.MORB, .27f, 2.55f, .08f));
			Add(new LampVariant("dense", VARIANT.DENSE, 0.07f, 0.98f, 3.35f));
			Add(new LampVariant("moon", VARIANT.MOON, 1.09f, 1.25f, 1.94f));
			Add(new LampVariant("brothgar", VARIANT.BROTHGAR, 2.47f, 1.75f, .62f));
			Add(new LampVariant("saturn", VARIANT.SATURN, 2.24f, 1.33f, .58f));
			Add(new LampVariant("pip", VARIANT.PIP, 2.47f, 1.75f, .62f));
			Add(new LampVariant("d6", VARIANT.D6, 2.73f, 0.35f, .60f));
			Add(new LampVariant("ogre", VARIANT.OGRE, 1.14f, 1.69f, 1.94f));
			Add(new LampVariant("tesseract", VARIANT.TESSERACT, 0.09f, 1.2f, 2.44f, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("cat", VARIANT.CAT, 2.55f, 2.22f, 1.48f));
			Add(new LampVariant("owo", VARIANT.OWO, 2.55f, 1.13f, 2.2f));
			Add(new LampVariant("star", VARIANT.STAR, 2.47f, 1.75f, .62f));

			// v1.2
			Add(new LampVariant("konny87", VARIANT.KONNY87, 0.14f, 1.46f, 2.55f));
			Add(new LampVariant("kleimug", VARIANT.KLEI_MUG, 2.55f, 1f, 0));
			Add(new LampVariant("redstonelamp", VARIANT.REDSTONE_LAMP, 2.55f, 1f, 0));
			Add(new LampVariant("cuddlepip", VARIANT.CUDDLE_PIP, 1.11f, 0.35f, 2.05f));
			Add(new LampVariant("archivetube", VARIANT.ARCHIVE_TUBE, 0.38f, 2.55f, 0.58f, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("lumaplays", VARIANT.LUMAPLAYS, 0.96f, 2.55f, 0.1f));
			Add(new LampVariant("diamondhatch", VARIANT.DIAMONDHATCH, 0.55f, 0.75f, 2.01f));
			Add(new LampVariant("beeta", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.BEE.NAME), 0, 2.55f, 0));
			Add(new LampVariant("glitterpuft", VARIANT.GLITTERPUFT, 0, 0, 0, mode: KAnim.PlayMode.Loop).Glitter());
			Add(new LampVariant("ai", VARIANT.AI, 0.38f, 2.55f, 0.58f, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("slagmite", VARIANT.SLAGMITE, 1.14f, 1.69f, 1.94f));

			// v1.4.4
			Add(new LampVariant("hamis", VARIANT.HAMIS, 0.98f, 0.55f, 1.36f));
			Add(new LampVariant("babybeefalo", VARIANT.BABY_BEEFALO, 2.27f, 1.45f, 0.64f));

			// v1.4.6
			Add(new LampVariant("heart_crystal", VARIANT.HEART_CRYSTAL, 3f, 0.4f, 0.8f, mode: KAnim.PlayMode.Loop));
			Add(new LampVariant("green_jelly", VARIANT.GREEN_JELLY, 0.58f, 2.53f, 0.52f));
			Add(new LampVariant("blue_jelly", VARIANT.BLUE_JELLY, 0, 1.03f, 3.3f));
			Add(new LampVariant("poro", VARIANT.PORO, 1.2f, 1.2f, 1.4f));

			// v1.5
			Add(new LampVariant("shovevole", Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.MOLE.NAME), 1.3f, 0.49f, 1.37f));
		}

		public LampVariant GetRandom() => resources.GetRandom();
	}
}
