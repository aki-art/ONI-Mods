using org.mariuszgromada.math.mxparser;

namespace Moonlet.Utils.MxParser
{
	public class CurrentCycleFunction : FunctionExtension
	{
		public CurrentCycleFunction()
		{
		}

		public double calculate() => GameClock.Instance == null ? -1 : GameClock.Instance.GetTimeInCycles();

		public FunctionExtension clone() => new CurrentCycleFunction();

		public string getParameterName(int parameterIndex) => "";

		public int getParametersNumber() => 0;

		public void setParameterValue(int parameterIndex, double parameterValue)
		{
		}
	}
}
