using org.mariuszgromada.math.mxparser;

namespace Moonlet.Utils.MxParser
{
	public class FromCelsiusFunction : FunctionExtension
	{
		double input;

		public FromCelsiusFunction()
		{
		}

		public FromCelsiusFunction(double input) => this.input = input;

		public double calculate() => input + 273.15;

		public FunctionExtension clone() => new FromCelsiusFunction(input);

		public string getParameterName(int parameterIndex) => "kelvin";

		public int getParametersNumber() => 1;

		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
				input = parameterValue;
		}
	}
}
