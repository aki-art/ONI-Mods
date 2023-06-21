namespace Twitchery.Content
{
	public class TPolymorphs : ResourceSet<Polymorph>
	{
		public TPolymorphs()
		{
			Add(new Polymorph(
				SquirrelConfig.ID,
				global::STRINGS.CREATURES.SPECIES.SQUIRREL.NAME,
				"squirrel_kanim",
				Polymorph.NavigatorType.FLOOR));

			Add(new Polymorph(
				DreckoConfig.ID,
				global::STRINGS.CREATURES.SPECIES.DRECKO.NAME,
				"drecko_kanim",
				Polymorph.NavigatorType.FLOOR));
		}

		public Polymorph GetRandom() => resources.GetRandom();
	}
}
