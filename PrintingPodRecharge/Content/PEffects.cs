using FUtility;

namespace PrintingPodRecharge.Content
{
    public class PEffects
    {
        public const string EGGNOG = "PrintingPodRecharge_Effect_Eggnog";

        public static void Register(ModifierSet set)
        {
            var attributes = Db.Get().Attributes;

			new EffectBuilder(EGGNOG, Consts.CYCLE_LENGTH, false)
				.Modifier(attributes.QualityOfLife.Id, 1)
				.Modifier(attributes.Athletics.Id, 1f)
				.Modifier(attributes.Strength.Id, 1f)
				.Modifier(attributes.Digging.Id, 1f)
				.Modifier(attributes.Construction.Id, 1f)
				.Modifier(attributes.Art.Id, 1f)
				.Modifier(attributes.Caring.Id, 1f)
				.Modifier(attributes.Learning.Id, 1f)
				.Modifier(attributes.Machinery.Id, 1f)
				.Modifier(attributes.Cooking.Id, 1f)
				.Modifier(attributes.Botanist.Id, 1f)
				.Modifier(attributes.Ranching.Id, 1f)
				.Modifier(attributes.SpaceNavigation.Id, 1f)
				.Name("Eggnogged")
				.Description("This duplicant feels great after a refreshing and sweet drink.")
				.Add(set);
		}
    }
}
