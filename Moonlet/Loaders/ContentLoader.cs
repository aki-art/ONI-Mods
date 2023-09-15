namespace Moonlet.Loaders
{
	public class ContentLoader<TemplateLoaderType>(string path) where TemplateLoaderType : TemplateLoaderBase
	{
		public TemplateLoaderCollection<TemplateLoaderType> templates = new();

		public string path = path;

		public void LoadYamls(string modFolder)
		{

		}

		public virtual void LoadTemplates()
		{

		}

		public virtual void LoadAssets()
		{

		}

		public virtual void Validate()
		{

		}
	}
}
