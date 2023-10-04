using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;

namespace Moonlet.Utils.MxParser
{
	// https://github.com/mariuszgromada/MathParser.org-mXparser/issues/127#issuecomment-367161862
	public class MExpression : org.mariuszgromada.math.mxparser.Expression
	{
		public static List<Function> globalFunctions;
		public static List<Constant> gobalConstants;

		public static void Setup()
		{
			globalFunctions = new()
			{
				new Function("celsius", new FromCelsiusFunction()),
				new Function("fahrenheit", new FromFahrenheitFunction())
			};
		}

		public MExpression(string expressionString) : base(expressionString)
		{
			foreach (var function in globalFunctions)
				addFunctions(function);
		}
	}
}
