/*using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PrintingPodRecharge.Content
{
    public class ExtraPersonalityData
    {
        public static Dictionary<string, ExtendedPersonality> personalities = new Dictionary<string, ExtendedPersonality>();
        private static int counter;

        private static string FormatName(string name)
        {
            var text = global::STRINGS.UI.StripLinkFormatting(name);
            text = text.ToUpperInvariant();
            text = text.Replace("_", "");
            text = text.Replace("-", "");

            return text;
        }

        public static Personality CreatePersonality(string name, string gender, string personalityType, string stress, 
            string joy, string sticker, int head, int mouth, int eyes, int hair, int body, Color hairColor, string fallbackPersonalityKey)
        {
            var keyedName = FormatName(name) + "_" + counter;
            counter++;

            var personality = new Personality(
                keyedName,
                keyedName,
                gender,
                personalityType,
                stress,
                joy,
                "",
                "",
                head,
                mouth,
                -1,
                eyes,
                hair,
                body,
                "",
                true)
            {
                Disabled = true // do not naturally print
            };

            personalities[keyedName] = new ExtendedPersonality()
            {
                personality = personality,
                hairColor = hairColor,
                fallbackPersonality = fallbackPersonalityKey
            };

            Db.Get().Personalities.Add(personality);

            return personality;
        }

        public static void Write(ExtendedPersonality personality)
        {
            var p = personality.personality;
            var path = Path.Combine(ModAssets.GetRootPath(), "data", "personalities", p.Id + ".json");
            var info = new PersonalityData()
            {
                ID = p.Id,
                Name = p.Name,
                Gender = p.genderStringKey,
                StickerType = p.stickerType,
                DescKey = personality.fallbackPersonality,
                HairColor = personality.hairColor.ToHexString(),
                Hair = p.hair,
                Body = p.body,
                Mouth = p.mouth,
                Neck = p.neck,
                Eye = p.eyes,
                HeadShape = p.headShape,
                JoyReaction = p.joyTrait,
                StressReaction = p.stresstrait,
                FallbackPersonality = personality.fallbackPersonality
            };

            Write(path, info);
        }

        internal static void OnSave()
        {
            var path = Path.Combine(ModAssets.GetRootPath(), "data", "personalities");
            foreach (var personality in personalities.Values)
            {
                if(personality.existsInWorld)
                {
                    //Write(personality);
                }
            }
        }

        private static void Write(string path, PersonalityData data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText(path, json);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Personality: Could not write file: " + e.Message);
            }
        }

        public static void LoadPersonalities()
        {
            var path = Path.Combine(ModAssets.GetRootPath(), "data", "personalities");
            foreach (var file in Directory.GetFiles(path, "*.json"))
            {
                if (ModAssets.TryReadFile(file, out var json))
                {
                    var personality = JsonConvert.DeserializeObject<PersonalityData>(json);

                    var p = new Personality(
                        personality.ID,
                        personality.ID,
                        personality.Gender,
                        "??",
                        personality.StressReaction,
                        personality.JoyReaction,
                        personality.StickerType,
                        null,
                        personality.HeadShape,
                        personality.Mouth,
                        personality.Neck,
                        personality.Eye,
                        personality.Hair,
                        personality.Body,
                        personality.DescKey,
                        true);

                    personalities[personality.ID] = new ExtendedPersonality()
                    {
                        personality = p,
                        hairColor = Util.ColorFromHex(personality.HairColor),
                        fallbackPersonality = personality.FallbackPersonality
                    };

                    Db.Get().Personalities.Add(p);
                }
            }

            counter = personalities.Count;
        }

        public class ExtendedPersonality
        {
            public Personality personality;
            public Color hairColor = Color.white;
            public string fallbackPersonality = "MEEP";
            public bool existsInWorld = false;
        }

        [Serializable]
        public class PersonalityData
        {
            public string ID { get; set; }

            public string Name { get; set; }

            public string Gender { get; set; }

            public string StickerType { get; set; }

            public string DescKey { get; set; }

            public string HairColor { get; set; }

            public int Hair { get; set; }

            public int Body { get; set; }

            public int Mouth { get; set; }

            public int Neck { get; set; }

            public int Eye { get; set; }

            public int HeadShape { get; set; }

            public string JoyReaction { get; set; }

            public string StressReaction { get; set; }

            public string FallbackPersonality { get; set; }
        }
    }
}
*/