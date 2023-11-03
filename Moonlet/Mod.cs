using HarmonyLib;
using KMod;
using Moonlet.Console;
using Moonlet.Console.Commands;
using Moonlet.Loaders;
using Moonlet.TemplateLoaders;
using Moonlet.TemplateLoaders.WorldgenLoaders;
using Moonlet.Templates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils.MxParser;
using PeterHan.PLib.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Moonlet
{
	public class Mod : UserMod2
	{
		public static HashSet<string> loadedModIds;

		public static SpritesLoader spritesLoader;
		public static TranslationsLoader translationsLoader;
		public static EffectsLoader effectsLoader;
		public static ElementsLoader elementsLoader;
		public static TemplatesLoader<ClusterLoader> clustersLoader;
		public static TemplatesLoader<WorldTraitLoader> traitsLoader;
		public static TemplatesLoader<FeatureLoader> featuresLoader;
		public static TemplatesLoader<WorldLoader> worldsLoader;
		public static TemplatesLoader<SubworldLoader> subWorldsLoader;
		public static ZoneTypesLoader zoneTypesLoader;
		public static TemplatesLoader<BiomeLoader> biomesLoader;
		public static TemplatesLoader<MobLoader> mobsLoader;
		public static TemplatesLoader<LibNoiseLoader> libNoiseLoader;
		public static TemplatesLoader<BorderLoader> borderLoader;
		public static TemplatesLoader<SubworldCategoryLoader> subworldCategoriesLoader;

		public static HashSet<string> loadBiomes = new();
		public static HashSet<string> loadFeatures = new();
		public static HashSet<string> loadNoise = new();

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			FUtility.Log.PrintVersion(this);
			PUtil.InitLibrary();

			SetupLoaders();
			SetupCommands();

			ModActions.Register();
			ModAssets.LoadAssets();

			//mXparser.disableImpliedMultiplicationMode(); // mucks up custom functions and keywords
			MExpression.Setup();
		}

		private void SetupCommands()
		{
			DevConsole.RegisterCommand(new HelpCommand());
			DevConsole.RegisterCommand(new LogIdCommand());
			DevConsole.RegisterCommand(new SetTemperatureCommand());
			DevConsole.RegisterCommand(new ReloadCommand());
			DevConsole.RegisterCommand(new AddEffectCommand());
			DevConsole.RegisterCommand(new RemoveEffectCommand());
			DevConsole.RegisterCommand(new DumpIdsCommand());
			DevConsole.RegisterCommand(new SpawnCommand());
			DevConsole.RegisterCommand(new RepeatCommand());
			DevConsole.RegisterCommand(new MathCommand());
			DevConsole.RegisterCommand(new FeatureCommand());
			DevConsole.RegisterCommand(new SetProjectCommand());
		}

		private static void SetupLoaders()
		{
			spritesLoader = new SpritesLoader();
			translationsLoader = new TranslationsLoader();
			effectsLoader = new EffectsLoader("effects");
			elementsLoader = new ElementsLoader("elements");
			traitsLoader = new TemplatesLoader<WorldTraitLoader>("worldgen/traits").CachePaths();
			featuresLoader = new TemplatesLoader<FeatureLoader>("worldgen/features").CachePaths();
			clustersLoader = new TemplatesLoader<ClusterLoader>("worldgen/clusters").CachePaths();
			worldsLoader = new TemplatesLoader<WorldLoader>("worldgen/worlds").CachePaths();
			zoneTypesLoader = new ZoneTypesLoader("worldgen/zonetypes");
			subWorldsLoader = new TemplatesLoader<SubworldLoader>("worldgen/subworlds");
			biomesLoader = new TemplatesLoader<BiomeLoader>("worldgen/biomes");
			libNoiseLoader = new TemplatesLoader<LibNoiseLoader>("worldgen/noise");
			borderLoader = new TemplatesLoader<BorderLoader>("worldgen/borders.yaml");
			mobsLoader = new TemplatesLoader<MobLoader>("worldgen/mobs.yaml");
			subworldCategoriesLoader = new TemplatesLoader<SubworldCategoryLoader>("worldgen/subworldCategories");
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

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			MoonletMods.Instance.Initialize(mods);

			loadedModIds = mods
				.Where(mod => mod.IsEnabledForActiveDlc())
				.Select(mod => mod.staticID)
				.ToHashSet();

			foreach (var mod in MoonletMods.Instance.moonletMods.Values)
			{
				effectsLoader.LoadYamls<EffectTemplate>(mod, false);
				clustersLoader.LoadYamls<ClusterTemplate>(mod, true);
				elementsLoader.LoadYamls<ElementTemplate>(mod, false);
				traitsLoader.LoadYamls<WorldTraitTemplate>(mod, true);
				featuresLoader.LoadYamls<FeatureTemplate>(mod, true);
				clustersLoader.LoadYamls<ClusterTemplate>(mod, true);
				worldsLoader.LoadYamls<WorldTemplate>(mod, true);
				zoneTypesLoader.LoadYamls<ZoneTypeTemplate>(mod, false);
				subWorldsLoader.LoadYamls<SubworldTemplate>(mod, true);
				biomesLoader.LoadYamls<BiomeTemplate>(mod, true);
				libNoiseLoader.LoadYamls<LibNoiseTemplate>(mod, true);
				borderLoader.LoadYamls<BorderTemplate>(mod, true);
				mobsLoader.LoadYamls<MobTemplate>(mod, true);
				subworldCategoriesLoader.LoadYamls<SubworldCategoryTemplate>(mod, false);
			}

			OptionalPatches.OnAllModsLoaded(harmony);

			stopWatch.Stop();
			Log.Info($"Moonlet initialized in {stopWatch.ElapsedMilliseconds} ms");
		}
	}
}
