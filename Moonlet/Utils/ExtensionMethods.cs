using Moonlet.Templates;

namespace Moonlet.Utils
{
	public static class ExtensionMethods
	{
		public static NumberType CalculateOrDefault<NumberType>(this NumberBase<NumberType> expression, NumberType defaultValue = default) where NumberType : struct
		{
			if (expression == null)
				return defaultValue;

			return expression.Calculate(defaultValue);
		}
	}
}
