using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public abstract class ContentLoader
	{
		public string path;
		public readonly int loadOrder;

		public ContentLoader(string path, int loadOrder = 100)
		{
			this.path = path;
			this.loadOrder = loadOrder;
			Mod.allLoaders.Add(this);
		}

		public virtual void Reload(string currentCluster, List<string> currentClusterTags)
		{
		}

		public virtual void LoadContent()
		{
		}

		public virtual void LoadYamls()
		{
		}
	}
}
