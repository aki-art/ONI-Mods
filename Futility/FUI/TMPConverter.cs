using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class TMPConverter
    {
        static TMP_FontAsset NotoSans;
        static TMP_FontAsset GrayStroke;

        public TMPConverter() => Initialize();

        public void Initialize()
        {
            var fonts = new List<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>());
            NotoSans = fonts.FirstOrDefault(f => f.name == "NotoSans-Regular");
            GrayStroke = fonts.FirstOrDefault(f => f.name == "GRAYSTROKE REGULAR SDF");
        }

        public void ReplaceAllText(GameObject parent)
        {
            var textComponents = parent.GetComponentsInChildren(typeof(Text), true);

            foreach (Text text in textComponents)
            {
                string TMPData = text.GetComponent<Text>().text;
                GameObject obj = text.gameObject;
                TMPSettings data = ExtractTMPData(TMPData, obj);

                if (data != null)
                {

                    var LT = obj.gameObject.AddComponent<LocText>();
                    LT.font = data.Font.Contains("GRAYSTROKE") ? GrayStroke : NotoSans;
                    LT.fontStyle = data.FontStyle;
                    LT.fontSize = data.FontSize;
                    LT.maxVisibleLines = data.MaxVisibleLines;
                    LT.enableWordWrapping = data.EnableWordWrapping;
                    LT.autoSizeTextContainer = data.AutoSizeTextContainer;
                    LT.text = data.Content;
                    LT.color = new Color(data.Color[0], data.Color[1], data.Color[2]);
                    LT.key = "";
                    // alignment isn't carried over instantiation, so its stored in .text and reapplied later}
                }
            }
        }

        private static TMPSettings ExtractTMPData(string TMPData, GameObject obj)
        {
            if (!TMPData.StartsWith("{\"Font\":")) return null;
            TMPSettings data = JsonConvert.DeserializeObject<TMPSettings>(TMPData);
            Object.DestroyImmediate(obj.GetComponent<Text>());
            return data;
        }
    }
}
