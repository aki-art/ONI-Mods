using Moonlet.Templates;

namespace Moonlet.Utils
{
	public static class ExtensionMethods
	{
		public static NumberType CalculateOrDefault<NumberType>(this NumberBase<NumberType> expression, NumberType defaultValue = default) where NumberType : struct
		{
			return expression == null ? defaultValue : expression.Calculate();
		}
	}
}
