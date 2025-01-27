using Moonlet.TemplateLoaders.WorldgenLoaders;
using Moonlet.Templates.SubTemplates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders
{
	public class ClusterLoader(ClusterTemplate template, string source) : TemplateLoaderBase<ClusterTemplate>(template, source), IWorldGenValidator
	{
		public string nameKey;
		public string descriptionKey;

		public static HashSet<string> referencedWorldsNotLoadedWithMoonlet = [];

		public override void Initialize()
		{
			id = $"clusters{relativePath}";
			template.Id = id;

			var link = relativePath.LinkAppropiateFormat();
			nameKey = $"STRINGS.CLUSTER_NAMES.{link}.NAME";
			descriptionKey = $"STRINGS.CLUSTER_NAMES.{link}.DESCRIPTION";


			base.Initialize();
		}

		public override void Validate()
		{
			if (template.Skip != null)
			{
				var skip = template.Skip.ToLowerInvariant();
				isValid = skip switch
				{
					"always" or "editoronly" => false,
					_ => true,
				};
			}

			if (!isValid)
				return;

			base.Validate();

			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				if (template.Width != null || template.Height != null)
					Issue($"Spaced Out Clusters do not support Width and Height settings, set them on the individual Worlds instead.");
			}
			else
			{
				if (template.Width == null)
					Issue($"No width defined.");

				if (template.Height == null)
					Issue($"No height defined.");
			}
		}

		public ClusterLayout Get()
		{
			var result = CopyProperties<ClusterLayout>();

			result.name = nameKey;
			result.description = descriptionKey;
			result.filePath = id;
			result.clusterCategory = GetClusterCategory(template.ClusterCategory);
			result.difficulty = GetDifficulty(template.Difficulty?.ToLowerInvariant());
			result.skip = ClusterLayout.Skip.Never;
			result.disableStoryTraits = template.DisableStoryTraits;
			result.coordinatePrefix = template.CoordinatePrefix;

			if (!DlcManager.FeatureClusterSpaceEnabled())
			{
				result.width = template.Width.CalculateOrDefault(64);
				result.height = template.Height.CalculateOrDefault(64);
			}

			result.requiredDlcIds = template.RequiredDlcIds ?? [];
			result.startWorldIndex = template.StartWorldIndex.CalculateOrDefault(0);
			result.menuOrder = template.MenuOrder.CalculateOrDefault(0);
			result.numRings = template.NumRings.CalculateOrDefault(12);
			result.fixedCoordinate = template.FixedCoordinate.CalculateOrDefault(-1);

			// Frosty stuff
			result.dlcIdFrom = DlcManager.VANILLA_ID;
			result.forbiddenDlcIds = [];
			result.requiredDlcIds =
			[
				DlcManager.IsExpansion1Active() ? DlcManager.EXPANSION1_ID : DlcManager.VANILLA_ID
			];
			result.clusterTags = template.ClusterTags ?? [];
			result.clusterAudio = new ClusterLayout.ClusterAudioSettings();

			result.welcomeMessage = GetString("WELCOMEMESSAGE");

			result.worldPlacements = ShadowTypeUtil.CopyList<WorldPlacement, WorldPlacementC>(template.WorldPlacements, Warn);

			return result;
		}

		private int GetDifficulty(string difficulty)
		{
			if (difficulty.IsNullOrWhiteSpace())
				return 0;  // TODO: Undefined difficulty

			if (int.TryParse(difficulty, out var result))
				return result;

			switch (difficulty)
			{
				case "ideal":
				case "0":
					return 0;
				case "probable":
				case "1":
					return 1;
				case "likely":
				case "2":
					return 2;
				case "moderate":
				case "3":
					return 3;
				case "marginal":
				case "4":
					return 4;
				case "slim":
				case "5":
					return 5;
			}

			// TODO: custom

			Issue($"Invalid difficulty rating {difficulty}. Defaulting to 0 (ideal).");

			return 0;
		}

		private ClusterLayout.ClusterCategory GetClusterCategory(string category)
		{
			var defaultCagtegory = DlcManager.FeatureClusterSpaceEnabled() ? ClusterLayout.ClusterCategory.SpacedOutVanillaStyle : ClusterLayout.ClusterCategory.Vanilla;

			if (category.IsNullOrWhiteSpace())
			{
				Warn($"{id} has no cluster category set. Defaulting to {defaultCagtegory}.");
				return defaultCagtegory;
			}

			return EnumUtils.ParseOrDefault(category, defaultCagtegory, logFn: Warn);
		}

		public void LoadContent(Dictionary<string, ClusterLayout> clusters)
		{
			if (template == null)
				return;

			var cluster = Get();

			/*			if (cluster == null || (cluster.forbiddenDlcId != null && !DlcManager.IsContentEnabled(cluster.forbiddenDlcId)))
							return;*/

			// TODO. fix dlc ids

			foreach (var world in template.WorldPlacements)
			{
				referencedWorldsNotLoadedWithMoonlet.Add(world.World);
			}

			clusters[cluster.filePath] = cluster;
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.Description);
			if (template.WelcomeMessage != null)
				AddString("WELCOMEMESSAGE", template.WelcomeMessage);
		}

		public void ValidateWorldGen()
		{
			if (template.WorldPlacements == null)
				return;

			foreach (var worldPlacement in template.WorldPlacements)
			{
				foreach (var w in SettingsCache.worlds.worldCache)
					Debug("\t - " + w.Key);

				if (!SettingsCache.worlds.worldCache.ContainsKey(worldPlacement.World))
					Warn($"Issue at cluster {id}: {worldPlacement.World} is not a registered World.");
			}

			if (template.PoiPlacements == null)
				return;

			foreach (var poi in template.PoiPlacements)
			{
				// TODO
			}
		}
	}
}
