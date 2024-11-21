using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class LampVariant : Resource
	{
		public Color color;
		public string kAnimFile;
		public KAnim.PlayMode mode;
		public bool hidden;
		public string category;
		public string uiName;
		public bool showCustomizableIcon;
		public List<Type> componentTypes;
		public HashSet<HashedString> tags = [];
		public Vector3 offset = Vector3.zero;
		public Dictionary<HashedString, object> data;

		public LampVariant(string id, string name, float r, float g, float b, string category = LampVariants.MISC, string kAnimFile = null, KAnim.PlayMode mode = KAnim.PlayMode.Paused, bool hidden = false, Vector3 offset = default, HashSet<HashedString> tags = null) : base(id, name)
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
			this.offset = offset;
			this.category = category;

			if (category == LampVariants.CUSTOMIZABLE)
				showCustomizableIcon = true;
			this.tags = tags;
		}

		public static T GetCustomDataOrDefault<T>(object data, HashedString key, T defaultValue)
		{
			if (TryGetData<Dictionary<HashedString, object>>(data, "Data", out var dict))
			{
				if (dict.TryGetValue(key, out var value))
				{
					return value == null ? defaultValue : (T)value;
				}
			}

			return defaultValue;
		}

		public static T GetDataOrDefault<T>(object data, HashedString key, T defaultValue)
		{
			if (data is Dictionary<HashedString, object> dict && dict.TryGetValue(key, out var value))
			{
				return value == null ? defaultValue : (T)value;
			}

			return defaultValue;
		}

		public static bool HasTag(object data, HashedString tag)
		{
			return TryGetData<HashSet<HashedString>>(data, "Tags", out var tags) && tags.Contains(tag);
		}

		public static bool TryGetData<T>(object data, HashedString key, out T result)
		{
			result = default;

			if (data is Dictionary<HashedString, object> dict && dict.TryGetValue(key, out var value))
			{
				if (value == null) return false;

				result = (T)value;

				return true;
			}

			return false;
		}

		public static bool TryGetData(Dictionary<HashedString, object> data, HashedString key, out object result)
		{
			result = default;

			if (data == null)
				return false;

			return data.TryGetValue(key, out result);
		}

		public static object GetDataOrDefault(Dictionary<HashedString, object> data, HashedString key, object defaultValue)
		{
			if (data == null)
				return defaultValue;

			return data.TryGetValue(key, out var value) ? value : defaultValue;
		}

		public LampVariant Customizable()
		{
			showCustomizableIcon = true;
			return this;
		}

		public LampVariant SetData(HashedString key, object data)
		{
			this.data ??= new();
			this.data[key] = data;

			return this;
		}

		public LampVariant SetData(Dictionary<HashedString, object> data)
		{
			foreach (var item in data)
				SetData(item.Key, item.Value);

			return this;
		}

		public LampVariant Tags(params HashedString[] tags)
		{
			this.tags ??= new();
			foreach (var tag in tags)
			{
				this.tags.Add(tag);
			}

			return this;
		}
	}
}
