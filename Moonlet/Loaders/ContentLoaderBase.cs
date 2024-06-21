namespace Moonlet.Loaders
{
	public abstract class ContentLoader(string path, int loadOrder = 100)
	{
		public string path = path;
		public readonly int loadOrder = loadOrder;

		public virtual void Reload(MoonletMod mod)
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
