using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders
{
	public class ClusterLoader(ClusterTemplate template, string source) : TemplateLoaderBase<ClusterTemplate>(template, source)
	{
		public string nameKey;
		public string descriptionKey;

		public override void Initialize()
		{
			id = $"clusters{relativePath}";
			template.Id = id;

			nameKey = $"STRINGS.CLUSTER_NAMES.{relativePath.LinkAppropiateFormat()}.NAME";
			descriptionKey = $"STRINGS.CLUSTER_NAMES.{relativePath.LinkAppropiateFormat()}.DESCRIPTION";

			base.Initialize();
		}

		public override void Validate()
		{
			base.Validate();
			if (DlcManager.IsExpansion1Active())
			{
				if (template.Width != null || template.Height != null)
					Warn($"{id}: Spaced Out Clusters do not support Width and Height settings, set them on the individual Worlds instead.");
			}
			else
			{
				if (template.Width == null)
					Warn($"{id}: No width defined.");

				if (template.Height == null)
					Warn($"{id}: No height defined.");
			}
		}

		public ClusterLayout Get()
		{
			var result = CopyProperties<ClusterLayout>();

			result.name = nameKey;
			result.description = descriptionKey;
			result.filePath = id;
			result.clusterCategory = GetClusterCategory(template.ClusterCategory?.ToLowerInvariant());
			result.difficulty = GetDifficulty(template.Difficulty?.ToLowerInvariant());

			if (!DlcManager.IsExpansion1Active())
			{
				result.width = template.Width.CalculateOrDefault(64);
				result.height = template.Height.CalculateOrDefault(64);
			}

			result.startWorldIndex = template.StartWorldIndex.CalculateOrDefault(0);
			result.menuOrder = template.MenuOrder.CalculateOrDefault(0);
			result.numRings = template.NumRings.CalculateOrDefault(12);
			result.fixedCoordinate = template.FixedCoordinate.CalculateOrDefault(-1);

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

			Warn($"{id} has an invalid difficulty rating {difficulty}. Defaulting to 0 (ideal).");

			return 0;
		}

		private int GetClusterCategory(string category)
		{
			var defaultCagtegory = DlcManager.IsExpansion1Active() ? 1 : 0;

			if (category.IsNullOrWhiteSpace())
			{
				Warn($"{id} has no cluster category set. Defaulting to {defaultCagtegory}.");
				return defaultCagtegory;
			}

			if (int.TryParse(category, out int result))
				return result;

			switch (category)
			{
				case "0":
				case "vanilla":
					return 0;
				case "1":
				case "classic":
					return 1;
				case "2":
				case "spacedout":
					return 2;
				case "3":
				case "lab":
					return 3;
			}

			// TODO: custom category

			Warn($"{id} has an invalid cluster category {category}. Defaulting to {defaultCagtegory}.");

			return defaultCagtegory;
		}

		public void LoadContent(Dictionary<string, ClusterLayout> clusters)
		{
			if (template == null)
				return;

			var cluster = Get();

			if (cluster == null || (cluster.forbiddenDlcId != null && !DlcManager.IsContentActive(cluster.forbiddenDlcId)))
				return;

			clusters[cluster.filePath] = cluster;
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.Description);
		}
	}
}
