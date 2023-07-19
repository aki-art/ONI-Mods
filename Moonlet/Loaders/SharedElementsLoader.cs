using FUtility;
using HarmonyLib;
using Moonlet.Content.Scripts;
using Moonlet.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class SharedElementsLoader
	{
		// original elements
		private List<ExtendedElementEntry> allEntries; // contains duplicate entries by multiple mods
		public List<ExtendedElementEntry> worldFilteredEntries; // contains the entries with conflicts resolved. resets each world load.
																// if no world is loaded (first load), cluster specific priorities are ignored

		// override vanilla or other modded elements
		private List<ExtendedElementEntry> allOverrides;
		public Dictionary<string, ExtendedElementEntry> worldFilteredOverrides; // keyed by elementId

		public HashSet<string> edibleElements;

		public bool ModifiesElements => allOverrides != null && allOverrides.Count > 0;

		private Material oreMaterial;
		private Material refinedMaterial;
		private Material gemMaterial;

		// collect all data on game start, well before mod loader
		// ElementLoader is not accessible yet!!
		public void PreLoadYamls()
		{
			foreach (var mod in Mod.modLoaders)
			{
				var elements = mod.elementLoader.CollectElementsFromYAML();
				if (elements != null && elements.Count > 0)
				{
					allEntries ??= new();
					allEntries.AddRange(elements);

					PatchTracker.loadsElements = true;
				}

				var modOverrides = mod.elementLoader.CollectOverridesFromYAML();
				if (modOverrides?.Elements != null && modOverrides.Elements.Count > 0)
				{
					allOverrides ??= new();
					allOverrides.AddRange(modOverrides.Elements);
				}
			}
		}

		public List<ElementLoader.ElementEntry> GenerateNewElementEntries(string clusterId)
		{
			Log.Debuglog($"GenerateNewElementEntries {clusterId}");
			worldFilteredEntries = WithResolvedConflicts(clusterId);

			if (worldFilteredEntries == null || worldFilteredEntries.Count == 0)
			{
				Log.Debuglog($"No entries");
				return null;
			}

			foreach (var entry in worldFilteredEntries)
				entry.AddStrings();

			Log.Debuglog($"Loaded {worldFilteredEntries.Count} entries");
			return worldFilteredEntries?.Select(entry => entry.CreateEntry()).ToList();
		}

		private List<ExtendedElementEntry> WithResolvedConflicts(string worldId)
		{
			var filtered = new List<ExtendedElementEntry>();

			foreach (var element in allEntries)
			{
				if (filtered.Any(data => data.ElementId == element.ElementId))
					continue;

				var conflicting = allEntries.Where(data => data.ElementId == element.ElementId).ToList();

				if (conflicting.Count == 1)
				{
					filtered.Add(element);
					continue;
				}

				var winner = GetHighestPriorityMod(element.ElementId, worldId, conflicting);

				// merge tags
				var tags = new List<string>();

				foreach (var conflict in conflicting)
				{
					if (conflict.Tags != null)
						tags.AddRange(conflict.Tags);
				}

				winner.Tags = tags.Distinct().ToArray();

				var addedBy = new List<string>();
				foreach (var conflict in conflicting)
					addedBy.AddRange(conflict.addedBy);

				winner.addedBy = addedBy.Distinct().ToList();
				filtered.Add(winner);
			}

			return filtered;
		}

		private static ExtendedElementEntry GetHighestPriorityMod(string elementId, string worldId, List<ExtendedElementEntry> conflicting)
		{
			var required = conflicting.FindAll(data => data.IsAlwaysRequired);
			if (required.Count > 1)
			{
				var str = required.Join(r => r.addedBy.Join());

				// TODO: display a popup error?
				Log.Warning($"Multiple mods declared {elementId} as required, and disallowing merging, resulting in a conflict." +
					$" One or more mods may not function as intended. Please contact the authors of these mods about this: {str}");
			}
			else if (required.Count == 1)
			{
				return required[0];
			}


			return conflicting.OrderByDescending(data => data.GetPriorityForCluster(worldId)).First();
		}

		internal void AddElementYamlCollection(List<ElementLoader.ElementEntry> result)
		{
			var elements = GenerateNewElementEntries(Moonlet_Mod.loadedClusterId);
			result.AddRange(elements);
		}

		internal void ApplyOverrides(List<ElementLoader.ElementEntry> result)
		{
			// TODO
		}

		public void LoadElements(ref List<Substance> list)
		{
			worldFilteredEntries ??= WithResolvedConflicts(null);

			oreMaterial = list.Find(e => e.elementID == SimHashes.Cuprite).material;
			refinedMaterial = list.Find(e => e.elementID == SimHashes.Copper).material;
			gemMaterial = list.Find(e => e.elementID == SimHashes.Diamond).material;

			var newElements = new HashSet<Substance>();

			foreach (var element in worldFilteredEntries)
			{
				var color = Util.ColorFromHex(element.Color);
				var uiColor = Util.ColorFromHex(element.UiColor);
				var conduitColor = Util.ColorFromHex(element.ConduitColor);
				var specularColor = element.SpecularColor.IsNullOrWhiteSpace() ? Color.black : Util.ColorFromHex(element.SpecularColor);
				var anim = GetElementAnim(element);

				var info = new ElementInfo(element.ElementId, anim, element.State, color);

				var specular = !element.SpecularTexture.IsNullOrWhiteSpace();
				var material = GetElementMaterial(element, list);
				newElements.Add(info.CreateSubstance(element.textureFolder, specular, material, uiColor, conduitColor, specularColor, element.NormalMapTexture));

				element.simHash = info.SimHash;

				SetRottableAtmosphere(element, element.simHash);
			}

			list.AddRange(newElements);
		}

		public void CreateUnstableFallers(ref List<UnstableGroundManager.EffectInfo> effects, UnstableGroundManager.EffectInfo referenceEffect)
		{
			foreach (var element in worldFilteredEntries)
			{
				if (IsUnstable(element))
					effects.Add(CreateEffect(element.simHash, element.ElementId, referenceEffect.prefab));
			}
		}

		private bool IsUnstable(ExtendedElementEntry elementData)
		{
			if (elementData.Tags == null)
				return false;

			foreach (var tag in elementData.Tags)
				if (tag == "Unstable")
					return true;

			return false;
		}

		private static UnstableGroundManager.EffectInfo CreateEffect(SimHashes element, string prefabId, GameObject referencePrefab)
		{
			var prefab = UnityEngine.Object.Instantiate(referencePrefab);
			prefab.name = $"Unstable{prefabId}";

			return new UnstableGroundManager.EffectInfo()
			{
				prefab = prefab,
				element = element
			};
		}

		private Material GetElementMaterial(ExtendedElementEntry element, List<Substance> list)
		{
			if (!element.MaterialReference.IsNullOrWhiteSpace())
			{
				var refElement = list.Find(e => e.elementID.ToString() == element.MaterialReference);

				if (refElement == null)
				{
					Log.Warning($"{element.ElementId} has asked to reference the material of {element.MaterialReference}, but there is no such element in the game.");
					return null;
				}

				return refElement.material;
			}

			if (element.SpecularTexture.IsNullOrWhiteSpace())
				return null;

			if (element.MaterialCategory == GameTags.RefinedMetal.ToString())
				return refinedMaterial;

			if (element.MaterialCategory == GameTags.Metal.ToString())
				return oreMaterial;

			return gemMaterial;
		}

		private string GetElementAnim(ExtendedElementEntry elementData)
		{
			if (!elementData.DebrisAnim.IsNullOrWhiteSpace())
				return elementData.DebrisAnim;

			return elementData.State switch
			{
				Element.State.Gas => "gas_tank_kanim",
				Element.State.Liquid => "liquid_tank_kanim",
				_ => elementData.ElementId.ToLowerInvariant() + "_kanim",
			};
		}

		private static void SetRottableAtmosphere(ExtendedElementEntry element, SimHashes id)
		{
			if (element.RotAtmosphereQuality.IsNullOrWhiteSpace())
				return;

			if (Enum.TryParse<Rottable.RotAtmosphereQuality>(element.RotAtmosphereQuality, out var rot))
			{
				if (rot != Rottable.RotAtmosphereQuality.Normal)
					Rottable.AtmosphereModifier[(int)id] = rot;
			}
		}

		public void SetExposureValues(Dictionary<SimHashes, float> customExposureRates)
		{
			if (worldFilteredEntries == null)
				return;

			foreach (var element in worldFilteredEntries)
				if (element.EyeIrritationStrength.HasValue)
					customExposureRates[element.simHash] = element.EyeIrritationStrength.Value;
		}

		public ExtendedElementEntry GetEntry(string id)
		{
			foreach(var entry in worldFilteredEntries)
				if(entry.ElementId == id)
					return entry;

			return null;
		}
	}
}
