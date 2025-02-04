﻿using System.Collections.Generic;

namespace Moonlet
{
	public class MoonletMods
	{
		private static MoonletMods instance;

		public System.Action OnLocalizationInit;

		public static MoonletMods Instance
		{
			get
			{
				instance ??= new MoonletMods();
				return instance;
			}
		}

		public Dictionary<string, MoonletMod> moonletMods = [];

		public void Initialize(IReadOnlyList<KMod.Mod> mods)
		{
			foreach (var mod in mods)
			{
				if (mod.IsEnabledForActiveDlc() && MoonletMod.IsMoonletMod(mod))
					moonletMods.Add(mod.staticID, new MoonletMod(mod));
			}
		}

		public string GetAssetsPath(string sourceMod, string contentPath)
		{
			if (moonletMods.TryGetValue(sourceMod, out var mod))
				return mod.GetAssetPath(contentPath);

			return null;
		}

		public string GetDataPath(string sourceMod, string contentPath, string dlcId)
		{
			if (moonletMods.TryGetValue(sourceMod, out var mod))
				return mod.GetDataPath(contentPath, dlcId);

			return null;
		}

		public string GetPrefixedId(string sourceMod, string id)
		{
			var mod = GetModData(sourceMod);

			return mod == null || mod.data.DisableIDPrefix
				? id
				: $"{sourceMod}:{id}";
		}

		public MoonletMod GetModData(string sourceMod)
		{
			if (moonletMods.TryGetValue(sourceMod, out var mod))
				return mod;

			return null;
		}
	}
}
