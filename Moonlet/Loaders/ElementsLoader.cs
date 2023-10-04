using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class ElementsLoader(string path) : TemplatesLoader<TemplateLoaders.ElementLoader>(path)
	{
		public void LoadElements(Dictionary<string, SubstanceTable> substanceTablesByDlc)
		{
			if (templates == null)
				return;

			Log.Debug($"Loading {templates.Count} elements");

			var substances = substanceTablesByDlc[DlcManager.VANILLA_ID].GetList();

			Log.Debug($"y");
			ApplyToActiveTemplates(element => element.LoadContent(ref substances));
		}

		public override void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry)
		{
			base.LoadYamls<TemplateType>(mod, singleEntry);

			if (templates != null && templates.Count > 0)
				OptionalPatches.requests |= OptionalPatches.PatchRequests.Enums;
		}

		public void AddElementYamlCollection(List<ElementLoader.ElementEntry> result)
		{
			Log.Debug("AddElementYamlCollection");

			if (templates == null)
				return;

			foreach (var template in templates)
			{
				var entry = template.ToElementEntry();
				Log.Debug("converted entry");
				result.Add(entry);
			}

			Log.Debug("Added yamls");
		}
	}
}
