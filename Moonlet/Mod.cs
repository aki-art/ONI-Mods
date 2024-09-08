using FMODUnity;
using HarmonyLib;
using KMod;
using Moonlet.Console;
using Moonlet.Console.Commands;
using Moonlet.DocGen;
using Moonlet.Loaders;
using Moonlet.TemplateLoaders;
using Moonlet.TemplateLoaders.EntityLoaders;
using Moonlet.TemplateLoaders.WorldgenLoaders;
using Moonlet.Templates;
using Moonlet.Templates.CodexTemplates;
using Moonlet.Templates.EntityTemplates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using Moonlet.Utils.MxParser;
using PeterHan.PLib.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Path = System.IO.Path;

namespace Moonlet
{
	public class Mod : UserMod2
	{
		public static HashSet<string> loadedModIds;

		public static List<IYamlTemplateLoader> loaders;

		public static SpritesLoader spritesLoader;
		public static FMODBanksLoader FMODBanksLoader;
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
		public static TemperaturesLoader temperaturesLoader;
		public static MTemplatesLoader templatesLoader;
		public static TemplatesLoader<DecorPlantLoader> decorPlantsLoader;
		public static EntitiesLoader<ItemLoader, ItemTemplate> itemsLoader;
		public static TemplatesLoader<SeedLoader> seedsLoader;
		public static TemplatesLoader<GenericEntityLoader> genericEntitiesLoader;
		public static TemplatesLoader<ArtifactLoader> artifactsLoader;
		public static TemplatesLoader<TemplateLoaders.BuildingLoader> buildingsLoader;
		public static TemplatesLoader<TileLoader> tilesLoader;
		public static TemplatesLoader<HarvestableSpacePOILoader> harvestableSpacePOIsLoader;
		public static TemplatesLoader<ArtableLoader> artablesLoader;
		public static TemplatesLoader<MaterialCategoryLoader> materialCategoriesLoader;
		public static TemplatesLoader<SpiceLoader> spicesLoader;
		public static CodexEntriesLoader codexLoader;

		public static HashSet<string> loadBiomes = [];
		public static HashSet<string> loadFeatures = [];
		public static HashSet<string> loadNoise = [];

		public static List<TraitSwapEntry> traitSwaps = [];

		public class TraitSwapEntry
		{
			public string worldId;
			public string originalTrait;
			public string replacementTrait;
		}

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

			ModDb.OnModInitialize();


			UnityEngine.Debug.developerConsoleVisible = true;
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
			DevConsole.RegisterCommand(new LayoutCommand());
		}

		// TODO: load order
		private static void SetupLoaders()
		{
			FMODBanksLoader = new FMODBanksLoader();
			temperaturesLoader = new TemperaturesLoader("worldgen/temperatures.yaml");
			spritesLoader = new SpritesLoader();
			translationsLoader = new TranslationsLoader();
			effectsLoader = new EffectsLoader("effects");
			elementsLoader = new ElementsLoader("elements");
			templatesLoader = new MTemplatesLoader();
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
			decorPlantsLoader = new TemplatesLoader<DecorPlantLoader>("entities/plants/decor");
			itemsLoader = new EntitiesLoader<ItemLoader, ItemTemplate>("entities/items");
			seedsLoader = new TemplatesLoader<SeedLoader>("entities/plants/seeds");
			genericEntitiesLoader = new TemplatesLoader<GenericEntityLoader>("entities/generic");
			artifactsLoader = new TemplatesLoader<ArtifactLoader>("entities/artifacts");
			buildingsLoader = new TemplatesLoader<TemplateLoaders.BuildingLoader>("buildings");
			tilesLoader = new TemplatesLoader<TileLoader>("tiles");
			harvestableSpacePOIsLoader = new TemplatesLoader<HarvestableSpacePOILoader>("space_destinations/spaced_out/harvestable");
			artablesLoader = new TemplatesLoader<ArtableLoader>("artworks");
			materialCategoriesLoader = new TemplatesLoader<MaterialCategoryLoader>("material_categories");
			spicesLoader = new TemplatesLoader<SpiceLoader>("spices");
			codexLoader = new CodexEntriesLoader("codex");
		}

		public static bool AreAnyOfTheseEnabled(string[] mods)
		{
			if (mods == null)
				return true;

			if (loadedModIds == null)
				return false;

			foreach (var mod in mods)
			{
				if (loadedModIds.Contains(mod))
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
				elementsLoader.LoadYamls<ElementTemplate>(mod, false);
				elementsLoader.LoadInfos();

				temperaturesLoader.LoadYamls<TemperatureTemplate>(mod, true);
				temperaturesLoader.ApplyToActiveTemplates(template => template.CacheRanges());

				effectsLoader.LoadYamls<EffectTemplate>(mod, false);
				clustersLoader.LoadYamls<ClusterTemplate>(mod, true);
				templatesLoader.LoadYamls<TemplateTemplate>(mod, true);
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
				seedsLoader.LoadYamls<SeedTemplate>(mod, true);
				decorPlantsLoader.LoadYamls<DecorPlantTemplate>(mod, true);
				itemsLoader.LoadYamls<ItemTemplate>(mod, true);
				genericEntitiesLoader.LoadYamls<EntityTemplate>(mod, true);
				artifactsLoader.LoadYamls<ArtifactTemplate>(mod, true);
				buildingsLoader.LoadYamls<BuildingTemplate>(mod, true);
				tilesLoader.LoadYamls<TileTemplate>(mod, true);
				harvestableSpacePOIsLoader.LoadYamls<HarvestableSpacePOITemplate>(mod, true);
				artablesLoader.LoadYamls<ArtableTemplate>(mod, true);
				materialCategoriesLoader.LoadYamls<MaterialCategoryTemplate>(mod, false);
				spicesLoader.LoadYamls<SpiceTemplate>(mod, true);
				codexLoader.LoadYamls<CodexEntryTemplate>(mod, true);
			}

			materialCategoriesLoader.ApplyToActiveTemplates(item => item.LoadContent());

			OptionalPatches.OnAllModsLoaded(harmony);

			if (KFMOD.didFmodInitializeSuccessfully)
				LoadFMOD();

			stopWatch.Stop();
			Log.Info($"Moonlet initialized in {stopWatch.ElapsedMilliseconds} ms");

#if DOCS
			var docs = new Docs();
			Log.Info("Generating documentation");
			var docsPath = Path.Combine(FUtility.Utils.ModPath, "docs");
			docs.Generate(Path.Combine(docsPath, "pages"), Path.Combine(docsPath, "template.html"));
#endif
		}

		public static void LoadFMOD()
		{
			RuntimeManager.StudioSystem.getBankList(out var banks);
			foreach (var bank in banks)
			{
				bank.getPath(out var path);
				Log.Debug($"- {path}");
			}

			MoonletMods.Instance.moonletMods.Do(mod =>
			{
				FMODBanksLoader.LoadContent(mod.Value, FileUtil.delimiter);
			});

			/*			App.OnPreLoadScene += () =>
						{
							MoonletMods.Instance.moonletMods.Do(mod =>
							{
								Mod.FMODBanksLoader.UnLoadContent();
							});
						};*/

			EventReference oceanPalace = RuntimeManager.PathToEventReference("event:/beached/Music/ocean_palace");
			var eventDesc = RuntimeManager.GetEventDescription(oceanPalace.Guid);
			eventDesc.getPath(out var path2);
			Log.Debug(path2);
		}
	}
}
