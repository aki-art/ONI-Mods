using Moonlet.LibNoiseExtension.Primitives;
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
			id = GetPathId("noise");
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
			result.links ??= [];

			var extPrimitives = ShadowTypeUtil.CopyDictionary<ExtendedPrimitive, PrimitiveC>(template.Primitives, Issue);
			var primitives = new Dictionary<string, Primitive>();

			if (extPrimitives != null)
			{
				foreach (var prim in extPrimitives)
					primitives[prim.Key] = prim.Value;
			}

			result.primitives = primitives;
			result.filters = ShadowTypeUtil.CopyDictionary<Filter, FilterC>(template.Filters, Issue);
			result.selectors ??= [];
			result.modifiers = ShadowTypeUtil.CopyDictionary<ProcGen.Noise.Modifier, ModifierC>(template.Modifiers, Issue);
			result.combiners = ShadowTypeUtil.CopyDictionary<Combiner, CombinerC>(template.Combiners, Issue);
			result.floats ??= [];
			result.controlpoints ??= [];

			result.transformers = ShadowTypeUtil.CopyDictionary<Transformer, TransformerC>(template.Transformers, Issue) ?? [];

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
