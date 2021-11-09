using Newtonsoft.Json;
using System;
using System.IO;

namespace FUtility.SaveData
{
    public class SaveDataManager<T> where T : IUserSetting, new()
    {
        public T Settings { get; set; }

        private readonly string path;

        public SaveDataManager(string path, bool readImmediately = true, string filename = "settings")
        {
            this.path = Path.Combine(path, filename + ".json");
            if (readImmediately)
            {
                Settings = Read();
            }
        }

        public T Read()
        {
            return ReadJson(out string json) ? JsonConvert.DeserializeObject<T>(json) : new T();
        }

        private bool ReadJson(out string result)
        {
            result = default;

            if (File.Exists(path))
            {
                result = TryReadFile(path);
            }

            return !result.IsNullOrWhiteSpace();
        }

        private string TryReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Config file found, but could not be read: ", e.Message);
                return null;
            }
        }

        public void Write()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not write configuration file: " + e.Message);
            }
        }
    }
}
