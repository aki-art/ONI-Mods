using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery
{
    public class ModAssets
    {
        public static class Sounds
        {
            public static string SPLAT = "aete_deltarune_splat";
            public static string DOORBELL = "aete_doorbell";
            public static string TEA = "aete_tea";
        }

        public static float GetSFXVolume() => KPlayerPrefs.GetFloat("Volume_SFX") * KPlayerPrefs.GetFloat("Volume_Master");

        public static void LoadAll()
        {
            var path = Path.Combine(FUtility.Utils.ModPath, "assets");

            AudioUtil.LoadSound(Sounds.SPLAT, Path.Combine(path, "snd_ralseising1.wav"));
            AudioUtil.LoadSound(Sounds.DOORBELL, Path.Combine(path, "370919__sjturia__doorbell.wav"));
            AudioUtil.LoadSound(Sounds.TEA, Path.Combine(path, "22890__stijn__tea.wav"));
        }

        public static Text AddText(Vector3 position, Color color, string msg)
        {
            var gameObject = new GameObject();

            var rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
            gameObject.transform.SetPosition(position);
            rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);

            var text2 = gameObject.AddComponent<Text>();
            text2.font = Assets.DebugFont;
            text2.text = msg;
            text2.color = color;
            text2.horizontalOverflow = HorizontalWrapMode.Overflow;
            text2.verticalOverflow = VerticalWrapMode.Overflow;
            text2.alignment = TextAnchor.MiddleCenter;

            return text2;
        }
    }
}
