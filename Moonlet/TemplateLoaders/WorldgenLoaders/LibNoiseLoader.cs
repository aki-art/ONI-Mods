﻿using Klei;
using Moonlet.Templates.SubTemplates.Noise;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using ProcGen.Noise;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class LibNoiseLoader(LibNoiseTemplate template, string sourceMod) : TemplateLoaderBase<LibNoiseTemplate>(template, sourceMod), IWorldGenValidator
	{
		public override void Initialize()
		{
			id = $"noise{relativePath}";
			template.Id = id;
			base.Initialize();
		}

		public void LoadContent()
		{
			var trees = SettingsCache.noise.trees;

			if (trees.TryGetValue(id, out var noise) && noise != null)
				return;

			trees[id] = Get();
		}


		public Tree Get()
		{
			var result = CopyProperties<Tree>();

			result.settings = template.Settings.Convert(Issue);
			result.settings.name = template.Name;
			result.links ??= new List<NodeLink>();
			result.primitives = ShadowTypeUtil.CopyDictionary<Primitive, PrimitiveC>(template.Primitives, Issue);
			result.filters = ShadowTypeUtil.CopyDictionary<Filter, FilterC>(template.Filters, Issue);
			result.selectors ??= new Dictionary<string, Selector>();
			result.modifiers = ShadowTypeUtil.CopyDictionary<ProcGen.Noise.Modifier, ModifierC>(template.Modifiers, Issue);
			result.combiners = ShadowTypeUtil.CopyDictionary<Combiner, CombinerC>(template.Combiners, Issue);
			result.floats ??= new Dictionary<string, FloatList>();
			result.controlpoints ??= new Dictionary<string, ControlPointList>();

			result.transformers = ShadowTypeUtil.CopyDictionary<Transformer, TransformerC>(template.Transformers, Issue) ?? new Dictionary<string, Transformer>();

			YamlIO.Save(result, "C:/Users/Aki/Desktop/yaml tests/" + id.LinkAppropiateFormat() + ".yaml");
			return result;
		}

		public override void RegisterTranslations()
		{
		}

		public void ValidateWorldGen()
		{
			if (template.Settings == null)
				Warn($"Issue with Noise {id}: Noise failed to load.");
		}
	}
}
