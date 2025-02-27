using Moonlet.TemplateLoaders;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.Loaders
{
	public class MTemplatesLoader() : TemplatesLoader<MTemplateLoader>("templates")
	{
		public HashSet<string> templateAddingMods = [];
		public const string MOONLET_TEMPLATES = "Moonlet_Templates";
		public const string MOONLET_TEMPLATES_PREFIX = $"{MOONLET_TEMPLATES}/";

		public PathHolder paths = new();

		public override void Initialize()
		{
			base.Initialize();

			paths = new PathHolder()
			{
				id = MOONLET_TEMPLATES
			};

			ApplyToActiveLoaders(loader =>
			{
				templateAddingMods.Add(loader.sourceMod);
				var split = loader.id.Split('/');
				List<string> path =
				[
					loader.sourceMod, .. split
				];

				PathHolder root = paths;
				foreach (var part in path)
				{
					var holder = root.children.Find(child => child.id == part);
					holder ??= root.AddChild(part);
					root = holder;
				}
			});
		}

		public static void ListChildren(PathHolder parent, int level = 0, int maxDepth = 10)
		{
			if (level >= maxDepth) return;

			foreach (var child in parent.children)
			{
				Log.Debug(string.Concat(Enumerable.Repeat('-', level)) + child.id);
				ListChildren(child, level + 1);
			}
		}

		public PathHolder GetPathHolder(string path)
		{
			var split = path.Split('/');
			PathHolder holder = paths;

			for (int i = 1; i < split.Length; i++)
			{
				holder = holder.children.Find(child => child.id == split[i]);
				if (holder == null)
				{
					Log.Warn("Incorrect template path: " + path);
					return null;
				}
			}

			return holder;
		}

		public class PathHolder
		{
			public string id;
			public List<PathHolder> children = [];

			public PathHolder AddChild(string id)
			{
				PathHolder item = new() { id = id };
				children.Add(item);

				return item;
			}
		}
	}
}
