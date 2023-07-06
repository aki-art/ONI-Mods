using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class LampVariant : Resource
	{
		public Color color;
		public Color color2;
		public float shiftDuration;
		public string kAnimFile;
		public KAnim.PlayMode mode;
		public bool hidden;
		public bool shifty;
		public bool rainbowLights;
		public string category;
		public List<Type> componentTypes;

		public LampVariant(string id, string name, float r, float g, float b, string kAnimFile = null, KAnim.PlayMode mode = KAnim.PlayMode.Paused, bool hidden = false) : base(id, name)
		{
			if (!Mod.Settings.MoodLamp.VibrantColors)
			{
				r = Mathf.Clamp01(r);
				g = Mathf.Clamp01(g);
				b = Mathf.Clamp01(b);
			}

			color = new Color(r, g, b, 1f) * 0.5f;

			this.kAnimFile = kAnimFile ?? $"dpi_moodlamp_{id}_kanim";
			this.mode = mode;
			this.hidden = hidden;
		}

		public LampVariant ToggleComponent<T>() where T : KMonoBehaviour
		{
			componentTypes ??= new();
			componentTypes.Add(typeof(T));
			return this;
		}

		public LampVariant Category(string categoryId)
		{
			category = categoryId;
			return this;
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
