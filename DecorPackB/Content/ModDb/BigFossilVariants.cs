using DecorPackB.Content.Defs.Items;

namespace DecorPackB.Content.ModDb
{
	public class BigFossilVariants : ResourceSet<BigFossilVariant>
	{
		public BigFossilVariants()
		{
			Add(new BigFossilVariant(
				"trex",
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.TREX.NAME,
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.TREX.DESCRIPTION,
				"decorpackb_giantfossil_trex_kanim",
				GiantFossilFragmentConfigs.TREX,
				false));

			Add(new BigFossilVariant(
				"livayatan",
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.LIVAYATAN.NAME,
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.LIVAYATAN.DESCRIPTION,
				"decorpackb_giantfossil_livayatan_kanim",
				GiantFossilFragmentConfigs.LIVAYATAN,
				true));

			Add(new BigFossilVariant(
				"triceratops",
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.TRICERATOPS.NAME,
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.TRICERATOPS.DESCRIPTION,
				"decorpackb_giantfossil_triceratops_kanim",
				GiantFossilFragmentConfigs.TRICERATOPS,
				false));

			Add(new BigFossilVariant(
				"bronto",
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.BRONTO.NAME,
				STRINGS.BUILDINGS.PREFABS.DECORPACKB_GIANTFOSSILDISPLAY.VARIANT.BRONTO.DESCRIPTION,
				"decorpackb_giantfossil_bronto_kanim",
				GiantFossilFragmentConfigs.BRONTO,
				false));
		}
	}
}
