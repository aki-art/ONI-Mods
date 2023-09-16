namespace Moonlet.Loaders
{
	public abstract class ContentLoader(string path)
	{
		public string path = path;

		public virtual void Reload(MoonletMod mod)
		{
		}
	}
}
