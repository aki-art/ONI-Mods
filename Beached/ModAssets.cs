using Beached.Germs;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Beached
{
    internal class ModAssets
    {
        public static class Colors
        {
            // elements
            public static Color basalt = new Color32(30, 30, 50, 255);
            public static Color mucus = new Color32(170, 205, 170, 255);
            public static Color mucusUi = new Color32(170, 205, 170, 255);
            public static Color mucusConduit = new Color32(170, 205, 170, 255);
            public static Color saltyOxygen = new Color32(205, 170, 170, 45);
            public static Color bismuth = new Color32(117, 166, 108, 255);
            public static Color bismuthOre = new Color32(117, 166, 108, 255);
            public static Color bismuthGas = new Color32(117, 166, 108, 255);
            public static Color moltenBismuth = new Color32(117, 166, 108, 255);
            public static Color aquamarine = new Color32(74, 255, 231, 255);

            // germs
            public static Color plankton = new Color32(0, 0, 255, 255);
        }

        public static class Diseases
        {
            public static Disease plankton;

            public static void Register(Database.Diseases diseases, bool statsOnly)
            {
                Assets.instance.DiseaseVisualization.info.Add(new DiseaseVisualization.Info(PlanktonGerms.ID) { overlayColourName = PlanktonGerms.ID });

                plankton = diseases.Add(new PlanktonGerms(statsOnly));
            }
        }

        public static class ZoneTypes
        {
            public static ZoneType beach;
            public static ZoneType depths;
            public static ZoneType bamboo;
        }

        public static class Deaths
        {
            public static Death desiccation;

            public static void Register()
            {
                new Death(
                    "Desiccation",
                    Db.Get().Deaths,
                    STRINGS.DEATHS.DESICCATION.NAME,
                    STRINGS.DEATHS.DESICCATION.DESCRIPTION,
                    "death_suffocation",
                    "dead_on_back");
            }
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

                desiccation.SetResolveStringCallback((str, data) => data is float timeUntilDeath ? string.Format(str, timeUntilDeath) : str);

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
            public static readonly Tag Bamboo = TagManager.Create("BeachedBamboo");

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
