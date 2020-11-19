using Klei;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WorldCreep.Geysers
{
    public class GeyserConfigs
    {
        public List<GeyserGenericConfig.GeyserPrefabParams> ReadGeyserData()
        {
            // TODO: read file properly
            string filename = Path.Combine(ModAssets.ModPath, "data", "geysers.json");
            var entries = JsonConvert.DeserializeObject<List<GeyserEntry>>(File.ReadAllText(filename));

            var list = new List<GeyserGenericConfig.GeyserPrefabParams>();

            foreach(var entry in entries)
            {
                var geyser = new GeyserConfigurator.GeyserType(
                        entry.ID,
                        entry.Element,
                        entry.Temperature,
                        entry.MinRatePerCycle,
                        entry.MaxRatePerCycle,
                        entry.MaxPressure,
                        entry.MinIterationLength,
                        entry.MaxIterationLength,
                        entry.MinIterationPercent,
                        entry.MaxIterationPercent,
                        entry.MinYearLength,
                        entry.MaxYearLength,
                        entry.MinYearPercent,
                        entry.MaxYearPercent
                    ); ;

                ConfigureDisease(entry, geyser);

                var geyserParams = new GeyserGenericConfig.GeyserPrefabParams(
                    entry.Anim + "_kanim",
                    entry.Width,
                    entry.Height,
                    geyser);

                list.Add(geyserParams);
            }

            return list;
        }

        private static void ConfigureDisease(GeyserEntry entry, GeyserConfigurator.GeyserType geyser)
        {
            if (!entry.Disease.IsNullOrWhiteSpace())
            {
                byte id = Db.Get().Diseases.GetIndex(entry.Disease);
                if (id != SimUtil.DiseaseInfo.Invalid.idx)
                { 
                    geyser.AddDisease(new SimUtil.DiseaseInfo()
                    {
                        idx = id,
                        count = (int)entry.DiseaseCount
                    });
                }
            }
        }
    }
}
