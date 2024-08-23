using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;

namespace Moonlet.Utils.MxParser
{
	// https://github.com/mariuszgromada/MathParser.org-mXparser/issues/127#issuecomment-367161862
	public class MExpression : org.mariuszgromada.math.mxparser.Expression
	{
		public static List<Function> globalFunctions;
		public static List<Constant> globalConstants;

		public static void Setup()
		{
			globalFunctions =
			[
				new Function("celsius", new FromCelsiusFunction()),
				new Function("fahrenheit", new FromFahrenheitFunction()),
				new Function("currentcycle", new CurrentCycleFunction()),
				new Function("lerp", new LerpFunction()),
			];

			globalConstants =
			[
				new Constant("_IsSpacedOut_", DlcManager.IsExpansion1Active() ? 1 : 0),
				new Constant("_IsRadiationEnabled_", DlcManager.FeatureRadiationEnabled() ? 1 : 0),
				new Constant("_PlantMutations_", DlcManager.FeaturePlantMutationsEnabled() ? 1 : 0),
				new Constant("_ClusterSpace_", DlcManager.FeatureClusterSpaceEnabled() ? 1 : 0),
				new Constant("_CycleLength_", 600)
			];
		}

		public MExpression(string expressionString) : base(expressionString)
		{
			foreach (var function in globalFunctions)
				addFunctions(function);

			foreach (var constants in globalConstants)
				addConstants(constants);
		}
	}
}
