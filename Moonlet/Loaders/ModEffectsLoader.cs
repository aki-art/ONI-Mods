using FUtility;
using System;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.Loaders
{
	public class ModEffectsLoader : BaseLoader
	{
		public static Dictionary<string, EffectEntry> effects;

		public ModEffectsLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
			LoadYAMLs();
		}

		public string EffectsFolder => Path.Combine(path, data.DataPath, EFFECTS);

		public static void Register(ModifierSet set)
		{
			if (effects == null)
				return;

			foreach(var effect in effects.Values)
			{
				Mod.AddStrings("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".NAME", effect.Name);
				Mod.AddStrings("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".TOOLTIP", effect.Tooltip);

				var builder = new EffectBuilder(effect.Id, effect.DurationSeconds, effect.IsBad);

				if(effect.Modifiers != null)
				{
					foreach(var modifier in effect.Modifiers)
					{
						builder.Modifier(modifier.Id, modifier.Value);
					}
				}
			}
		}

		public void LoadYAMLs()
		{
			var path = EffectsFolder;

			if (!Directory.Exists(path))
				return;

			foreach (var file in Directory.GetFiles(path, "*.yaml"))
			{
				if (data.DebugLogging)
					Log.Info("Loading geysers file " + file);

				var collection = FileUtil.Read<EffectsCollection>(file);

				if (collection?.Effects == null)
					continue;

				effects ??= new Dictionary<string, EffectEntry>();
				foreach (var effect in collection.Effects)
					effects[effect.Id] = effect;
			}
		}

		public class EffectsCollection
		{
			public EffectEntry[] Effects { get; set; }
		}


		[Serializable]
		public class EffectEntry
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string Tooltip { get; set; }

			public bool AtmoSuitImmunity { get; set; }

			public string Tint { get; set; }

			public float DurationSeconds { get; set; }

			public bool IsBad { get; set; }

			public bool TriggerFloatingText { get; set; } = true;

			public bool ShowInUI { get; set; } = true;

			public ModifierEntry[] Modifiers { get; set; }

			public class ModifierEntry
			{
				public string Id { get; set; }

				public float Value { get; set; }

				public bool IsMultiplier { get; set; }
			}
		}
	}
}
