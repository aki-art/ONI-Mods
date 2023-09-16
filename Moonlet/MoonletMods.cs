using System.Collections.Generic;

namespace Moonlet
{
	public class MoonletMods
	{
		private static MoonletMods instance;

		public static MoonletMods Instance
		{
			get
			{
				instance ??= new MoonletMods();
				return instance;
			}
		}

		public List<MoonletMod> moonletMods = new();

		public void Initialize(IReadOnlyList<KMod.Mod> mods)
		{
			foreach (var mod in mods)
			{
				if (mod.IsEnabledForActiveDlc() && MoonletMod.IsMoonletMod(mod))
					moonletMods.Add(new MoonletMod(mod));
			}
		}
	}
}
