using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

namespace FUtility.FUI
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
                GameObject obj = text.gameObject;
                TMPSettings data = JsonConvert.DeserializeObject<TMPSettings>(TMPData);
                UnityEngine.Object.DestroyImmediate(obj.GetComponent<Text>());

                var TMPText = obj.gameObject.AddComponent<TextMeshProUGUI>();
                TMPText.font = data.Font.Contains("GRAYSTROKE") ? GrayStroke : NotoSans;
                TMPText.fontStyle = data.FontStyle;
                TMPText.fontSize = data.FontSize;
                Enum.TryParse(data.Alignment, out TextAlignmentOptions alignment);
                TMPText.maxVisibleLines = data.MaxVisibleLines;
                TMPText.enableWordWrapping = data.EnableWordWrapping;
                TMPText.autoSizeTextContainer = data.AutoSizeTextContainer;
                TMPText.text = data.Content;
                TMPText.SetLayoutDirty();
            }
        }
    }
}
