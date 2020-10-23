using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace FUtility
{
    public class Utils
    {
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary> Spawns one entity by tag.</summary>
        public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            var prefab = global::Assets.GetPrefab(tag);
            if (prefab == null) return null;
            var go = GameUtil.KInstantiate(global::Assets.GetPrefab(tag), position, sceneLayer);
            go.SetActive(setActive);
            return go;
        }

        /// <summary> Spawns one entity by tag. </summary>
        /// <param name="atGO">Spawn at the position of this gameObject.</param>
        public static GameObject Spawn(Tag tag, GameObject atGO, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            return Spawn(tag, atGO.transform.position, sceneLayer, setActive);
        }

        public class FileManager
        {
            public string folder;
            public string fileName;
            public bool exterior;
            public string Root => Path.Combine(Util.RootFolder(), "mods");
            public string Local => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            public FileManager(string defaultFileName, bool exterior = false, string folder = null)
            {
                this.folder = folder;
                fileName = defaultFileName;
                this.exterior = exterior;
            }

            public void WriteFile(object obj)
            {
                try
                {
                    string path = GetPath();
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string filePath = Path.Combine(path, fileName);

                    using (var sw = new StreamWriter(filePath))
                    {
                        var serializedUserSettings = JsonConvert.SerializeObject(obj, Formatting.Indented);
                        sw.Write(serializedUserSettings);
                        Log.Info($"Settings saved to: {filePath}");
                    }
                }
                catch (Exception e)
                {
                    Log.Warning($"Couldn't write to {Path.Combine(exterior ? Root : Local, folder)}, {e.Message}");
                }
            }

            private string GetPath()
            {
                if (folder == null)
                    return exterior ? Root : Local;
                else
                    return Path.Combine(exterior ? Root : Local, folder);
            }

            public T ReadFile<T>(string path = null)
            {
                try
                {
                    if(path == null)
                        path = Path.Combine(GetPath(), fileName);

                    if (!File.Exists(path))
                        return default;

                    using (var r = new StreamReader(path))
                    {
                        var json = r.ReadToEnd(); 
                        return JsonConvert.DeserializeObject<T>(json);
                    }
                }
                catch (Exception e)
                {
                    Log.Warning($"Couldn't read file, {e.Message}. Using default settings.");
                    return default;
                }
            }

/*            public bool TryGetModdedElement(string name, out Element element)
            {
                SimHashes hash = (SimHashes) Hash.SDBMLower(name);
                return ElementLoader.elementTable.TryGetValue((int)hash, out element);
            }*/
        }
    }
}
