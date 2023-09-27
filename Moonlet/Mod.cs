using HarmonyLib;
using KMod;
using Moonlet.Console;
using Moonlet.Console.Commands;
using Moonlet.Loaders;
using Moonlet.TemplateLoaders;
using Moonlet.Templates;
using Moonlet.Templates.WorldGenTemplates;
using System.Collections.Generic;
using System.Linq;
using PeterHan.PLib.Core;

namespace Moonlet
{
	public class Mod : UserMod2
	{
		public static HashSet<string> loadedModIds;

		public static SpritesLoader spritesLoader;
		public static TranslationsLoader translationLoader;
		public static EffectsLoader effectsLoader;
		public static TemplatesLoader<ClusterLoader> clustersLoader;

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			FUtility.Log.PrintVersion(this);
			PUtil.InitLibrary();

			SetupLoaders();
			SetupCommands();

			ModActions.Register();
			ModAssets.LoadAssets();
		}

		private void SetupCommands()
		{
			DevConsole.RegisterCommand(new HelpCommand());
		}

		private static void SetupLoaders()
		{
			spritesLoader = new SpritesLoader();
			translationLoader = new TranslationsLoader();
			effectsLoader = new EffectsLoader("effects");
			clustersLoader = new TemplatesLoader<ClusterLoader>("worldgen/clusters");
		}

		public static bool AreAnyOfTheseEnabled(string[] mods)
		{
			if (mods == null)
				return true;

			if (loadedModIds == null)
				return false;

			foreach (var id in loadedModIds)
			{
				if (mods.Contains(id))
					return true;
			}

			return false;
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);

			MoonletMods.Instance.Initialize(mods);
			loadedModIds = mods
				.Where(mod => mod.IsEnabledForActiveDlc())
				.Select(mod => mod.staticID)
				.ToHashSet();

			foreach(var mod in MoonletMods.Instance.moonletMods)
			{
				effectsLoader.LoadYamls<EffectTemplate>(mod, false);
				clustersLoader.LoadYamls<ClusterTemplate>(mod, true);
			}
		}
	}
}
