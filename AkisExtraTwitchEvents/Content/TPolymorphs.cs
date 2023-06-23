using UnityEngine;

namespace Twitchery.Content
{
	public class TPolymorphs : ResourceSet<Polymorph>
	{
		public const string
			PIP = SquirrelConfig.ID,
			DRECKO = DreckoConfig.ID,
			MUCKROOT = BasicForagePlantConfig.ID,
			CRAB = CrabConfig.ID;

		public TPolymorphs()
		{
			Add(new Polymorph(
				PIP,
				global::STRINGS.CREATURES.SPECIES.SQUIRREL.NAME,
				"squirrel_kanim",
				Polymorph.NavigatorType.FLOOR,
				new Vector2(0, 0.3f), 
				"sq_head",
				Vector2.zero));

			Add(new Polymorph(
				DRECKO,
				global::STRINGS.CREATURES.SPECIES.DRECKO.NAME,
				"drecko_kanim",
				Polymorph.NavigatorType.FLOOR,
				new Vector2(0, 0.3f),
				"",
				Vector2.zero));

			Add(new Polymorph(
				CRAB,
				global::STRINGS.CREATURES.SPECIES.CRAB.NAME,
				"pincher_kanim",
				Polymorph.NavigatorType.FLOOR,
				new Vector2(0, 0.3f),
				"",
				Vector2.zero));

			/*			Add(new Polymorph(
							MUCKROOT,
							global::STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.NAME,
							"muckrootVegetable_kanim",
							Polymorph.NavigatorType.FLOOR,
							new Vector2(0, 0.3f),
							"",
							Vector2.zero));*/
		}

		public Polymorph GetRandom() => resources.GetRandom();
	}
}
