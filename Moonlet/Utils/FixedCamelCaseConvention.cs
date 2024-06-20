extern alias YamlDotNetButNew;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Utils
{
	// the original version removes the _ symbols too which 
	public class FixedCamelCaseConvention : INamingConvention
	{
		public static readonly INamingConvention Instance = new FixedCamelCaseConvention();

		public string Apply(string value)
		{
			if (value.IsNullOrWhiteSpace())
				return value;

			return value[0].ToString().ToUpperInvariant() + value.Substring(1);
		}
	}
}
