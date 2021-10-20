using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PollingOfONIUITest
{
    public class TMPConverter
    {
        private static TMP_FontAsset NotoSans;
        private static TMP_FontAsset GrayStroke;

        public static void Initialize()
        {
            foreach (var newFont in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
            {
                switch (newFont?.name)
                {
                    case "NotoSans-Regular":
                        NotoSans = newFont;
                        break;
                    case "GRAYSTROKE REGULAR SDF":
                        GrayStroke = newFont;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ReplaceAllText(GameObject parent)
        {
            if (NotoSans == null || GrayStroke == null)
                Initialize();

            var textComponents = parent.GetComponentsInChildren(typeof(Text), true);

            foreach (Text text in textComponents)
            {
                string TMPData = text.GetComponent<Text>().text;
                if (TMPData.IsNullOrWhiteSpace() || !TMPData.StartsWith("{")) continue; 

                GameObject obj = text.gameObject;
                TMPData data = JsonConvert.DeserializeObject<TMPData>(TMPData);
                UnityEngine.Object.DestroyImmediate(obj.GetComponent<Text>());

                var TMPText = obj.gameObject.AddComponent<TextMeshProUGUI>();
                TMPText.font = data.Font.Contains("GRAYSTROKE") ? GrayStroke : NotoSans;
                TMPText.fontStyle = data.FontStyle;
                TMPText.fontSize = data.FontSize;
                TMPText.maxVisibleLines = data.MaxVisibleLines;
                TMPText.enableWordWrapping = true;
                TMPText.autoSizeTextContainer = false;
                TMPText.text = data.Content;
                TMPText.isRightToLeftText = false;
                TMPText.horizontalMapping = TextureMappingOptions.Character;
                TMPText.verticalMapping = TextureMappingOptions.Character;
                TMPText.overflowMode = TextOverflowModes.Overflow;
                Enum.TryParse(data.Alignment, out TextAlignmentOptions alignment);
                TMPText.alignment = alignment;
                TMPText.SetAllDirty();
            }
        }
    }
}
