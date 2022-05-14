using HarmonyLib;
using Klei.AI;
using Slag.Content;

namespace Slag.Patches
{
    public class LegacyModMainPatch
    {
        [HarmonyPatch(typeof(LegacyModMain), "ConfigElements")]
        public class LegacyModMain_ConfigElements_Patch
        {
            public static void Postfix()
            {
                var slag = ElementLoader.FindElementByHash(Elements.Slag);
                slag.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, 120, slag.name));
                slag.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -0.2f, slag.name, true));

                var slagGlass = ElementLoader.FindElementByHash(Elements.SlagGlass);
                slagGlass.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, 0.15f, slagGlass.name, true));
            }
        }
    }
}
