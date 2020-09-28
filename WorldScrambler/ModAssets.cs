using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using YamlDotNet.Serialization;
using static FUtility.Assets;

namespace WorldScrambler
{
    public class ModAssets
    {
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static Texture2D entropeaAsteroid;

        public static void SaveYAML(object obj, string subPath)
        {
            string filePath = Path.Combine(ModPath, subPath);
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    SerializerBuilder serializerBuilder = new SerializerBuilder();
                    serializerBuilder.DisableAliases();
                    //serializerBuilder.EmitDefaults();
                    string content = serializerBuilder.Build().Serialize(obj);
                    sw.Write(content);

                    Debug.Log($"File saved to: {filePath}");
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Couldn't write to {filePath}, {e.Message}");
            }

        }

        public static int EpochTime()
        {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

            return currentEpochTime;
        }

        internal static void LateLoadAssets()
        {
            entropeaAsteroid = LoadTexture("asteroid_entropea");

            Sprite sprite = Sprite.Create(
                texture: entropeaAsteroid,
                rect: new Rect(0, 0, entropeaAsteroid.width, entropeaAsteroid.height),
                pivot: new Vector2(128, 128));

            Assets.Sprites.Add(new HashedString("asteroid_entropea"), sprite);
        }
    }
}
