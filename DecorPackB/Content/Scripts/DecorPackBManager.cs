﻿using DecorPackB.Content.Defs.Buildings;
using DecorPackB.Settings;
using KSerialization;
using System;

namespace DecorPackB.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DecorPackBManager : KMonoBehaviour
    {
        [Serialize]
        private LiteModeSettings worldLiteModeSettings;

        public static DecorPackBManager Instance;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.Loaded, OnLoaded);
        }

        private void OnLoaded(object obj)
        {
            OnSettingsChanged();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public LiteModeSettings GetLiteModeSettings() => worldLiteModeSettings ?? Mod.LiteModeSettings;

        public void SetLiteModeSettings(LiteModeSettings settings)
        {
            worldLiteModeSettings = settings;
            OnSettingsChanged();
        }

        public void OnSettingsChanged()
        {
            var settings = GetLiteModeSettings();

            var fossilDef = Assets.GetBuildingDef(FossilDisplayConfig.ID);
            var giantFossilDef = Assets.GetBuildingDef(GiantFossilDisplayConfig.ID);

            if (settings.FunctionalFossils)
            {
                fossilDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;

                fossilDef.MaterialCategory = new[]
                {
                    DPTags.trueFossilMaterial.ToString()
                };

                giantFossilDef.Mass = new[]
                {
                    800f,
                    //1f
                };

                giantFossilDef.MaterialCategory = new[]
                {
                    DPTags.trueFossilMaterial.ToString(),
                    //FossilNodileConfig.ID
                };
            }
            else
            {
                fossilDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;

                fossilDef.MaterialCategory = new[]
                {
                    DPTags.liteFossilMaterial.ToString()
                };

                giantFossilDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;

                giantFossilDef.MaterialCategory = new[]
                {
                    DPTags.liteFossilMaterial.ToString()
                };
            }

            fossilDef.MassForTemperatureModification = fossilDef.Mass[0] * 0.2f;
            giantFossilDef.MassForTemperatureModification = giantFossilDef.Mass[0] * 0.2f;

            BuildMenu.Instance.Refresh();
            PlanScreen.Instance.Refresh();
        }
    }
}