using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace Randomizer.Content.Scripts.Generators.YamlWorld
{
    public class ProceduralGenerator<T>
    {
        public SeededRandom rng;
        private string path;
        public string name;

        public ProceduralGenerator(SeededRandom rng, string path, string namePrefix)
        {
            this.rng = rng;
            this.path = path;

            // i used guuid before but it made debugging hell
            var prefix = GetName(rng, ModDb.prefix);
            var middle = GetName(rng, ModDb.nameparts);
            var suffix = GetName(rng, ModDb.suffix);
            name = $"{namePrefix}_{prefix}_{middle}_{suffix}_{System.DateTime.Now.ToFileTime()}";
        }

        private static string GetName(SeededRandom rng, List<string> list)
        {
            return list[rng.RandomRange(0, list.Count - 1)];
        }

        public string FullName() => $"{path}/{name}";

        public void WriteYaml(T value, string fileName)
        {
            var folder = Path.Combine(Mod.worldGenFolder, path);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var filePath = Path.Combine(Mod.worldGenFolder, path, fileName + ".yaml");
            Log.Debuglog($"writing yaml to {filePath}");

            using StreamWriter writer = new(filePath);

            new SerializerBuilder()
                .EmitDefaults()
                .Build()
                .Serialize(writer, value);
        }
    }
}
