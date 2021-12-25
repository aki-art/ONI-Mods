using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace FUtility.SaveData
{
    public class SaveDataManager<T> where T : IUserSetting, new()
    {
        public T Settings { get; set; }

        private readonly string localPath;
        private readonly string externalPath;
        private readonly string externalFolder;
        private readonly string externalRoot;

        public bool localExists;
        public bool externalExists;
        public bool saveExternal;

        public SaveDataManager(string localPath, bool readImmediately = true, bool writeIfDoesntExist = true, string filename = "settings")
        {
            this.localPath = Path.Combine(localPath, filename + ".json");
            externalFolder = Path.Combine(Util.RootFolder(), "mods", "settings", "akismods", Log.modName.ToLowerInvariant());
            externalPath = Path.Combine(externalFolder, filename + ".json");

            Log.Debuglog("external path set to", externalPath);

            if (readImmediately)
            {
                Settings = Read();
            }
        }

        public void WriteIfDoesntExist(bool useExternal)
        {
            if ((useExternal && !externalExists) || (!useExternal && !localExists))
            {
                Write(useExternal);
            }

            // clean up outside folder if needed
            if(!useExternal && externalExists)
            {
                // DeleteExternalFolder();
            }
        }

        private void DeleteExternalFolder()
        {
            try
            {
                if (Directory.Exists(externalFolder)) // delete the mods folder
                {
                    Directory.Delete(externalFolder, true);

                    string akisModsPath = Path.GetDirectoryName(externalFolder);
                    if(DeleteDirIfEmpty(akisModsPath))
                    {
                        string settingsPath = Path.Combine(akisModsPath);
                        DeleteDirIfEmpty(settingsPath);
                    } 
                }
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Tried deleting external mod settings folder, but could not: " + e.Message);
            }
        }

        private bool DeleteDirIfEmpty(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path); // delete settings folder only if it is empty
            if (!Directory.EnumerateFileSystemEntries(path).Any())
            {
                Log.Debuglog($"Deleting folder: {path}");
                dir.Delete(false);
                return true;
            }

            return false;
        }

        public T Read()
        {
            return ReadJson(out string json) ? JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            }) : new T();
        }

        private bool ReadJson(out string result)
        {
            result = default;

            string path = "";

            if (File.Exists(externalPath))
            {
                path = externalPath;
                externalExists = true;
            }
            else if(File.Exists(localPath))
            {
                path = localPath;
                localExists = true;
            }

            if(!path.IsNullOrWhiteSpace())
            {
                result = TryReadFile(path);
                Log.Debuglog("Reading configurations from ", path);
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

        public void Write(bool useExternal = false, bool cleanUp = true)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                File.WriteAllText(useExternal ? externalPath : localPath, json);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not write configuration file: " + e.Message);
            }
        }
    }
}
