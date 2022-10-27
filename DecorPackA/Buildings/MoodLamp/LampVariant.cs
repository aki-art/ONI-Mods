using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
    public class LampVariant : Resource
    {
        public Color color;
        public string kAnimFile;
        public string on;
        public string off;
        public KAnim.PlayMode mode;
        public bool hidden;

        public LampVariant(string id, string name, float r, float g, float b, string kAnimFile = "moodlamp_kanim", KAnim.PlayMode mode = KAnim.PlayMode.Paused, bool hidden = false) : base(id, name)
        {
            if (!Mod.Settings.MoodLamp.VibrantColors)
            {
                r = Mathf.Clamp01(r);
                g = Mathf.Clamp01(g);
                b = Mathf.Clamp01(b);
            }

            color = new Color(r, g, b, 1f) * 0.5f;

            this.kAnimFile = kAnimFile;
            this.mode = mode;
            this.hidden = hidden;

            on = id + "_on";
            off = id + "_off";
        }
    }
}
