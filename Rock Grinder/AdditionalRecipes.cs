using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace RockGrinder
{
    public class AdditionalRecipes
    {
        private const string FILENAME = "recipes.json";

        public static SimpleRecipe Read() => ReadJson(out string json) ? JsonConvert.DeserializeObject<SimpleRecipe>(json) : new SimpleRecipe();

        private static bool ReadJson(out string result)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FILENAME);
            result = default;

            if (File.Exists(path))
                result = TryRead(path);

            return !result.IsNullOrWhiteSpace();
        }

        private static string TryRead(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}