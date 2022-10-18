using FUtility;

namespace SpookyPumpkinSO.Content
{
    public class SPEffects
    {
        public const string SPOOKED = "SP_Spooked";
        public const string HOLIDAY_SPIRIT = "AHM_HolidaySpirit";
        public const string GHASTLY = "SP_Ghastly";
        public const string PUMPKINED = "SP_Pumpkined";

        internal static void Register(ModifierSet parent)
        {
            var attributes = Db.Get().Attributes;
            var amounts = Db.Get().Amounts;

            new EffectBuilder(SPOOKED, 120f, false)
                .Name(STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME)
                .Description(STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP)
                .Modifier(attributes.Athletics.Id, 8)
                .Modifier(amounts.Bladder.deltaAttribute.Id, 2f / 3f)
                .Add(parent);

            new EffectBuilder(HOLIDAY_SPIRIT, 360f, false)
                .Name(STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME)
                .Description(STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP)
                .Modifier(attributes.Athletics.Id, 1)
                .Modifier(attributes.Art.Id, 1)
                .Modifier(attributes.Botanist.Id, 1)
                .Modifier(attributes.Construction.Id, 1)
                .Modifier(attributes.Caring.Id, 1)
                .Modifier(attributes.Learning.Id, 1)
                .Modifier(attributes.Machinery.Id, 1)
                .Modifier(attributes.Strength.Id, 1)
                .Modifier(attributes.Ranching.Id, 1)
                .Modifier(attributes.Cooking.Id, 1)
                .Modifier(attributes.Digging.Id, 1)
                .Add(parent);

            new EffectBuilder(PUMPKINED, Consts.CYCLE_LENGTH, false)
                .Name(STRINGS.DUPLICANTS.STATUSITEMS.GHASTLY.NAME)
                .Description(STRINGS.DUPLICANTS.STATUSITEMS.GHASTLY.TOOLTIP)
                .Modifier(attributes.Athletics.Id, 4)
                .Add(parent);

            new EffectBuilder(GHASTLY, 0, false)
                .Name(STRINGS.DUPLICANTS.STATUSITEMS.GHASTLY.NAME)
                .Description(STRINGS.DUPLICANTS.STATUSITEMS.GHASTLY.TOOLTIP)
                .Modifier(attributes.Athletics.Id, 4)
                .Add(parent);
        }
    }
}
