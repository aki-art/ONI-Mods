using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached
{
    internal class ModAssets
    {
        public static class Colors
        {
            public static Color basalt = new Color32(30, 30, 50, 255);
            public static Color mucus = new Color32(170, 205, 170, 255);
            public static Color mucusUi = new Color32(170, 205, 170, 255);
            public static Color mucusConduit = new Color32(170, 205, 170, 255);
            public static Color saltyOxygen = new Color32(205, 170, 170, 133);
            public static Color bismuth = new Color32(117, 166, 108, 255);
            public static Color bismuthOre = new Color32(117, 166, 108, 255);
            public static Color bismuthGas = new Color32(117, 166, 108, 255);
            public static Color moltenBismuth = new Color32(117, 166, 108, 255);
            //public static Color aquamarine = new Color32(74, 255, 231, 255);
            //public static Color gravel = new Color32(176, 170, 164, 255);
        }

        public static class StatusItems
        {
            public static StatusItem desiccation;
            public static StatusItem secretingMucus;

            public static void Register()
            {
                desiccation = new StatusItem(
                    "Beached_Desiccation",
                    "CREATURES",
                    string.Empty,
                    StatusItem.IconType.Exclamation,
                    NotificationType.Bad,
                    false,
                    OverlayModes.None.ID,
                    true);

                secretingMucus = new StatusItem(
                    "Beached_SecretingMucus",
                    "CREATURES",
                    string.Empty,
                    StatusItem.IconType.Exclamation,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    false);
            }
        }

        public static class Tags
        {
            public class Creatures
            {
                public static readonly Tag SecretingMucus = TagManager.Create("BeachedSecretingMucus");
            }
            public static class Species
            {
                public static readonly Tag Snail = TagManager.Create("BeachedSnailSpecies");
            }
        }

        public class Effects
        {
            public const string OCEAN_BREEZE = "OceanBreeze";

            public static List<Effect> GetEffectsList()
            {
                return new List<Effect>
                {
                    new Effect(OCEAN_BREEZE, "Ocean Breeze", "", 180f, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>()
                        {
                            new AttributeModifier(Db.Get().Attributes.AirConsumptionRate.Id, -0.01f),
                            new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -5f / 600f)
                        }
                    }
                };
            }
        }
    }
}
