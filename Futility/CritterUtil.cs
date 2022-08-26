using Klei.AI;

namespace FUtility
{
    public class CritterUtil
    {
        public static Trait CreateCritterBaseTrait(string ID, string name, float maxCal, float deltaCalPerCycle, float HP, float maxAge)
        {
            var trait = Db.Get().CreateTrait(ID, name, name, null, false, null, true, true);
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, maxCal, name));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, deltaCalPerCycle / 600f, name));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, HP, name));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, maxAge, name));

            return trait;
        }
    }
}
