namespace CrittersDropBones.Effects
{
    public class CDBEffects
    {
        public static void RegisterAll(ModifierSet modifierSet)
        {
            modifierSet.effects.Add(new StaminaRegenerationEffect().Create());
        }
    }
}
