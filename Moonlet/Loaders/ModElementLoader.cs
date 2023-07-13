using FUtility;
using Klei;
using Moonlet.Elements;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Moonlet.Loaders
{
	public class ModElementLoader : BaseLoader
	{
		private List<ElementData> elements;
		private Material oreMaterial;
		private Material refinedMaterial;
		private Material gemMaterial;

		public string ElementsFolder => Path.Combine(path, data.DataPath, ELEMENTS);

		public string ElementTexturesFolder => Path.Combine(path, data.AssetsPath, ELEMENTS);

		public bool HasLoadedElements => elements != null && elements.Count > 0;

		public ModElementLoader(KMod.Mod mod, MoonletData data) : base(mod, data) { }

		public void LoadElementsFromYaml()
		{
			var elementsfolder = ElementsFolder;

			if (!Directory.Exists(elementsfolder))
				return;

			elements = new();
		}


		public class ExtendedElementEntryCollection
		{
			public ElementData[] elements { get; set; }
		}

		public void CollectElementsFromYAML()
		{
			elements = new List<ElementData>();
			var path = ElementsFolder;

			if (data.DebugLogging)
				Log.Info($"Attempting to load elements {path}");

			if (!Directory.Exists(path))
			{
				if (data.DebugLogging)
					Log.Info($"No elements data folder found.");

				return;
			}

			var errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();

			foreach (var file in Directory.GetFiles(path, "*.yaml"))
			{
				if (data.DebugLogging)
					Log.Info("Loading element file " + file);

				var elementEntryCollection = YamlIO.LoadFile<ExtendedElementEntryCollection>(file, (error, warning) => errors.Add(error));

				if (elementEntryCollection != null)
					elements.AddRange(elementEntryCollection.elements);

				if (elementEntryCollection.elements.Length > 0)
					PatchTracker.loadsElements = true;
			}


			if (Global.Instance != null && Global.Instance.modManager != null)
				Global.Instance.modManager.HandleErrors(errors);

			errors.Recycle();
		}

		public void LoadElements(ref List<Substance> list)
		{
			oreMaterial = list.Find(e => e.elementID == SimHashes.Cuprite).material;
			refinedMaterial = list.Find(e => e.elementID == SimHashes.Copper).material;
			gemMaterial = list.Find(e => e.elementID == SimHashes.Diamond).material;

			var newElements = new HashSet<Substance>();

			foreach (var element in elements)
			{
				if (data.DebugLogging)
					Log.Info("Loading element" + element.elementId);

				var color = Util.ColorFromHex(element.color);
				element.uiColor ??= element.color;
				element.conduitColor ??= element.color;
				var uiColor = Util.ColorFromHex(element.uiColor);
				var conduitColor = Util.ColorFromHex(element.conduitColor);
				var specularColor = element.specularColor.IsNullOrWhiteSpace() ? Color.black : Util.ColorFromHex(element.specularColor);
				var anim = GetElementAnim(element);

				var info = new ElementInfo(element.elementId, anim, element.state, color);

				var specular = !element.specularTexture.IsNullOrWhiteSpace();
				var material = GetElementMaterial(element, list);
				newElements.Add(info.CreateSubstance(ElementTexturesFolder, specular, material, uiColor, conduitColor, specularColor, element.normalMapTexture));

				element.SimHash = info.SimHash;

				SetRottableAtmosphere(element, info);
			}

			list.AddRange(newElements);
		}

		private static void SetRottableAtmosphere(ElementData element, ElementInfo info)
		{
			if (Enum.TryParse<Rottable.RotAtmosphereQuality>(element.rotAtmosphereQuality, out var rot))
			{
				if (rot != Rottable.RotAtmosphereQuality.Normal)
					Rottable.AtmosphereModifier.Add((int)info.SimHash, rot);
			}
		}

		private Material GetElementMaterial(ElementData element, List<Substance> list)
		{
			if (!element.materialReferene.IsNullOrWhiteSpace())
			{
				var refElement = list.Find(e => e.elementID.ToString() == element.materialReferene);

				if (refElement == null)
				{
					Log.Warning($"{element.elementId} has asked to reference the material of {element.materialReferene}, but there is no such element in the game.");
					return null;
				}

				return refElement.material;
			}

			if (element.specularTexture.IsNullOrWhiteSpace())
				return null;

			if (element.materialCategory == GameTags.RefinedMetal.ToString())
				return refinedMaterial;

			if (element.tags.Contains(GameTags.Metal.ToString()))
				return oreMaterial;

			return gemMaterial;
		}

		private string GetElementAnim(ElementData elementData)
		{
			if (!elementData.debrisAnim.IsNullOrWhiteSpace())
				return elementData.debrisAnim;

			return elementData.state switch
			{
				Element.State.Gas => "gas_tank_kanim",
				Element.State.Liquid => "liquid_tank_kanim",
				_ => elementData.elementId.ToLowerInvariant() + "_kanim",
			};
		}

		public void AddElementYamlCollection(List<ElementLoader.ElementEntry> entries)
		{
			if (HasLoadedElements)
			{
				foreach (var element in elements)
				{
					element.localizationID ??= $"STRINGS.ELEMENTS.{element.elementId.ToUpperInvariant()}.NAME";
					element.description ??= $"STRINGS.ELEMENTS.{element.elementId.ToUpperInvariant()}.DESC";

					ModLoader.locstringKeys.Add(element.localizationID, element.name ?? "");

					if (!element.name.IsNullOrWhiteSpace())
						Strings.Add(element.localizationID, element.name);

					ModLoader.locstringKeys.Add(element.description, element.descriptionText ?? "");

					if (!element.descriptionText.IsNullOrWhiteSpace())
						Strings.Add(element.description, element.descriptionText);
				}

				entries.AddRange(elements.Select(ElementUtil.Convert));
			}
		}

		internal void CreateUnstableFallers(ref List<UnstableGroundManager.EffectInfo> effects, UnstableGroundManager.EffectInfo referenceEffect)
		{
			foreach (var element in elements)
			{
				if (IsUnstable(element))
					effects.Add(CreateEffect(element.SimHash, element.elementId, referenceEffect.prefab));
			}
		}

		private bool IsUnstable(ElementData elementData)
		{
			if (elementData.tags == null)
				return false;

			foreach (var tag in elementData.tags)
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
	}
}
