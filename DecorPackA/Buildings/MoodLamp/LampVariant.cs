using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class LampVariant : Resource
	{
		public Color color;
		public Color color2;
		public float shiftDuration;
		public string kAnimFile;
		public string on;
		public string off;
		public KAnim.PlayMode mode;
		public bool hidden;
		public bool shifty;
		public bool rainbowLights;

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

		public LampVariant ShiftColors(float r, float g, float b, float duration)
		{
			if (!Mod.Settings.MoodLamp.VibrantColors)
			{
				r = Mathf.Clamp01(r);
				g = Mathf.Clamp01(g);
				b = Mathf.Clamp01(b);
			}

			color2 = new Color(r, g, b, 1f) * 0.5f;
			shifty = true;
			shiftDuration = duration;

			return this;
		}

		public LampVariant Glitter()
		{
			rainbowLights = true;
			return this;
		}
	}
}
