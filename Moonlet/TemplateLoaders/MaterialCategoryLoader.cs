using HarmonyLib;
using Moonlet.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace Moonlet.TemplateLoaders
{
	public class MaterialCategoryLoader(MaterialCategoryTemplate template, string sourceMod) : TemplateLoaderBase<MaterialCategoryTemplate>(template, sourceMod)
	{
		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
		}

		public static Dictionary<string, Func<List<Tag>>> storageFiltersLookup = new()
		{
			{ nameof(STORAGEFILTERS.NOT_EDIBLE_SOLIDS), () => STORAGEFILTERS.NOT_EDIBLE_SOLIDS },
			{ nameof(STORAGEFILTERS.FOOD), () => STORAGEFILTERS.FOOD },
			{ nameof(STORAGEFILTERS.DEHYDRATED), () => STORAGEFILTERS.DEHYDRATED },
			{ nameof(STORAGEFILTERS.SWIMMING_CREATURES), () => STORAGEFILTERS.SWIMMING_CREATURES },
			{ nameof(STORAGEFILTERS.PAYLOADS), () => STORAGEFILTERS.PAYLOADS },
			{ nameof(STORAGEFILTERS.LIQUIDS), () => STORAGEFILTERS.LIQUIDS },
			{ nameof(STORAGEFILTERS.SPECIAL_STORAGE), () => STORAGEFILTERS.SPECIAL_STORAGE },
			{ nameof(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD), () => STORAGEFILTERS.STORAGE_LOCKERS_STANDARD },
			{ nameof(STORAGEFILTERS.GASES), () => STORAGEFILTERS.GASES },
			{ nameof(STORAGEFILTERS.BAGABLE_CREATURES), () => STORAGEFILTERS.BAGABLE_CREATURES },
			{"BAGGABLE_CREATURES", () => STORAGEFILTERS.BAGABLE_CREATURES },
			{ nameof(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE), () => null }
		};

		public List<Tag> GetStorageFilterFromName(string name)
		{
			if (storageFiltersLookup.TryGetValue(name, out var filter))
				return filter();

			Warn($"Invalid Storage Filter ID {name}");

			return null;
		}

		public override string GetTranslationKey(string partialKey)
		{
			if (partialKey == "NAME")
				return $"STRINGS.MISC.TAGS.{template.Id.ToUpperInvariant()}";

			else if (partialKey == "DESCRIPTION")
				return $"STRINGS.MISC.TAGS.{template.Id.ToUpperInvariant()}_DESC";

			return string.Empty;
		}

		public void LoadContent()
		{
			var tag = TagManager.Create(template.Id);

			if (GameTags.IgnoredMaterialCategories.Contains(tag))
				GameTags.IgnoredMaterialCategories.Remove(tag);

			if (!GameTags.MaterialCategories.Contains(tag))
				GameTags.MaterialCategories.Add(tag);

			template.AddTo?.Do(filter => AddToStorageFilter(filter, tag));
		}

		private void AddToStorageFilter(string filter, Tag category)
		{
			if (filter == nameof(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
			{
				if (!STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE.Contains(category))
					STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE = STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE.AddToArray(category);

				return;
			}

			var filterList = GetStorageFilterFromName(filter);

			if (filterList != null && !filterList.Contains(id))
			{
				Debug("Added material category to " + filter);
				filterList.Add(id);
			}
		}
	}
}
