using org.mariuszgromada.math.mxparser;

namespace Moonlet.Utils.MxParser
{
	public class FromFahrenheitFunction : FunctionExtension
	{
		double input;

		public FromFahrenheitFunction()
		{
		}

		public FromFahrenheitFunction(double input) => this.input = input;

		public double calculate() => GameUtil.GetTemperatureConvertedToKelvin((float)input, GameUtil.TemperatureUnit.Fahrenheit);

		public FunctionExtension clone() => new FromFahrenheitFunction(input);

		public string getParameterName(int parameterIndex) => "kelvin";

		public int getParametersNumber() => 1;

		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
				input = parameterValue;
		}
	}
}
