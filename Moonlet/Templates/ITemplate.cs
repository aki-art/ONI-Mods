namespace Moonlet.Templates
{
	public interface ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }

		//public string GetId(); // The ancient Yaml Dot Net we use does not support interface properties yet, so this must be implemented instead
	}
}
