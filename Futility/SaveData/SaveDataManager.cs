using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Linq;

namespace FUtility.SaveData
{
    public class SaveDataManager<T> where T : IUserSetting, new()
    {
        public T Settings { get; set; }

        public Action<T> OnRead;

        public bool localExists;
        public bool externalExists;
        public bool saveExternal;

        private readonly string localPath;
        private readonly string externalPath;
        private readonly string externalFolder;
        private readonly string externalRoot;
        private readonly string localFolder;

        private FileSystemWatcher watcher;

        public SaveDataManager(string localPath) : this(localPath, true, true, "settings", new JsonConverter[] { new StringEnumConverter() })
        {
        }

        public SaveDataManager(string localPath, bool readImmediately = true, bool writeIfDoesntExist = true, string filename = "settings", JsonConverter[] converters = null)
        {
            this.localPath = Path.Combine(localPath, filename + ".json");
            externalFolder = Path.Combine(Util.RootFolder(), "mods", "settings", "akismods", Log.modName.ToLowerInvariant());
            externalPath = Path.Combine(externalFolder, filename + ".json");
            localFolder = localPath;

            Log.Debuglog("external path set to", externalPath);

            if (readImmediately)
            {
                Settings = Read();
            }

            if (writeIfDoesntExist)
            {
                WriteIfDoesntExist(false, converters);
            }
        }

        public void WatchForChanges()
        {
            if (watcher is null)
            {
                Log.Debuglog(GetPath());

                watcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(GetPath()),
                    Filter = Path.GetFileName(GetPath()),
                    EnableRaisingEvents = true
                };
            }

            watcher.Changed += new FileSystemEventHandler((sender, e) => Settings = Read());
            watcher.Changed += new FileSystemEventHandler((sender, e) => Log.Info("Settings reloaded."));
        }

        public void OnFileChanged(Action<object, FileSystemEventArgs> sender)
        {
            watcher.Changed += new FileSystemEventHandler(sender);
        }

        public void WriteIfDoesntExist(bool useExternal, JsonConverter[] converters)
        {
            if ((useExternal && !externalExists) || (!useExternal && !localExists))
            {
                Write(useExternal, converters);
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
            T result = ReadJson(out string json) ? JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            }) : new T();

            OnRead?.Invoke(result);
            return result;
        }

        private string GetPath()
        {
            if(externalExists)
            {
                return externalPath;
            }
            else if(localExists)
            {
                return localPath;
            }

            if (File.Exists(externalPath))
            {
                externalExists = true;
                return externalPath;
            }
            else if (File.Exists(localPath))
            {
                localExists = true;
                return localPath;
            }

            return default;
        }

        private bool ReadJson(out string result)
        {
            result = default;

            string path = GetPath();
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

        public void Write(bool useExternal = false, JsonConverter[] converters = null, bool cleanUp = true)
        {
            try
            {
                var path = useExternal ? externalFolder : localFolder;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string json = JsonConvert.SerializeObject(Settings, Formatting.Indented, converters);
                string path1 = useExternal ? externalPath : localPath;
                File.WriteAllText(path1, json);

                Log.Debuglog("saved config to " + path1);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not write configuration file: " + e.Message);
            }
        }
    }
}
