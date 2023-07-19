using FUtility;
using HarmonyLib;
using Klei;
using System.Collections.Generic;
using System.IO;
using TUNING;

namespace Moonlet.Loaders
{
	public class ModMaterialCategoryLoader : BaseLoader
	{
		public List<MaterialCategoryData> materialCategories;

		public string CategoriesFile => Path.Combine(path, data.DataPath, "materialCategories.yaml");

		public ModMaterialCategoryLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
		{
			var path = CategoriesFile;

			if (File.Exists(path))
			{
				materialCategories = FileUtil.Read<CategoryCollection>(path)?.Categories;
				if (materialCategories != null)
				{
					foreach (var category in materialCategories)
					{
						var key = $"STRINGS.MISC.TAGS.{category.Id.ToUpperInvariant()}";
						Strings.Add(key, category.Name);

						var tag = TagManager.Create(category.Id);

						if (GameTags.IgnoredMaterialCategories.Contains(tag))
							GameTags.IgnoredMaterialCategories.Remove(tag);

						if (!GameTags.MaterialCategories.Contains(tag))
							GameTags.MaterialCategories.Add(tag);

						else if (data.DebugLogging)
							Log.Info($"Tried to load MaterialCategory {tag}, but it already exists.");

						category.AddTo?.Do(filter => AddToStorageFilter(filter, tag));

						if (data.DebugLogging)
							Log.Info($"Loaded MaterialCategory {tag}");
					}
				}
			}
		}

		private void AddToStorageFilter(string filter, Tag category)
		{
			switch (filter)
			{
				case nameof(STORAGEFILTERS.NOT_EDIBLE_SOLIDS):
					AddTo(STORAGEFILTERS.NOT_EDIBLE_SOLIDS, category);
					break;
				case nameof(STORAGEFILTERS.FOOD):
					AddTo(STORAGEFILTERS.FOOD, category);
					break;
				case nameof(STORAGEFILTERS.SWIMMING_CREATURES):
					AddTo(STORAGEFILTERS.SWIMMING_CREATURES, category);
					break;
				case nameof(STORAGEFILTERS.PAYLOADS):
					AddTo(STORAGEFILTERS.PAYLOADS, category);
					break;
				case nameof(STORAGEFILTERS.LIQUIDS):
					AddTo(STORAGEFILTERS.LIQUIDS, category);
					break;
				case nameof(STORAGEFILTERS.GASES):
					AddTo(STORAGEFILTERS.GASES, category);
					break;
				case nameof(STORAGEFILTERS.BAGABLE_CREATURES):
					AddTo(STORAGEFILTERS.BAGABLE_CREATURES, category);
					break;
			}
		}

		private static void AddTo(List<Tag> list, Tag id)
		{
			if (!list.Contains(id))
				list.Add(id);
		}

		public class CategoryCollection
		{
			public List<MaterialCategoryData> Categories { get; set; }
		}

		public class MaterialCategoryData
		{
			public string Name { get; set; }

			public string Id { get; set; }

			public string[] AddTo { get; set; }
		}
	}
}
