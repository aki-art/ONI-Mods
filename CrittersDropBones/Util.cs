using System.Collections.Generic;

namespace CrittersDropBones
{
    public class Util
    {
        public class FoodInfoBuilder
        {
            private string ID;
            private string dlcId;
            private float kCalPerUnit;
            private int quality;
            private float preserveTemperature;
            private float rotTemperature;
            private float spoilTime;
            private bool canRot;

            private List<string> effects;

            public static FoodInfoBuilder StandardFood(string ID, string dlcId = DlcManager.VANILLA_ID)
            {
                return new FoodInfoBuilder(ID, dlcId)
                    .Rot(TUNING.FOOD.DEFAULT_ROT_TEMPERATURE)
                    .Spoil(TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE, TUNING.FOOD.SPOIL_TIME.DEFAULT);
            }

            public FoodInfoBuilder(string ID, string dlcId = DlcManager.VANILLA_ID)
            {
                this.ID = ID;
                this.dlcId = dlcId;
                this.canRot = false;
            }

            public FoodInfoBuilder Quality(int quality)
            {
                this.quality = quality;
                return this;
            }

            public FoodInfoBuilder KcalPerUnit(float kCalPerUnit)
            {
                this.kCalPerUnit = kCalPerUnit;
                return this;
            }

            public FoodInfoBuilder Effect(string effect, string[] dlc = null)
            {
                effects = effects ?? new List<string>();

                if(dlc == null || DlcManager.IsDlcListValidForCurrentContent(dlc))
                {
                    effects.Add(effect);
                }

                return this;
            }

            public FoodInfoBuilder Spoil(float preserveTemperature, float spoilTime)
            {
                this.preserveTemperature = preserveTemperature;
                this.spoilTime = spoilTime;
                return this;
            }

            public FoodInfoBuilder Rot(float rotTemperature)
            {
                this.rotTemperature = rotTemperature;
                canRot = true;
                return this;
            }

            public EdiblesManager.FoodInfo Build()
            {
                return new EdiblesManager.FoodInfo(
                    ID,
                    dlcId,
                    kCalPerUnit * 1000f,
                    quality,
                    preserveTemperature,
                    rotTemperature,
                    spoilTime,
                    canRot)
                {
                    Effects = effects
                };
            }
        }
    }
}
