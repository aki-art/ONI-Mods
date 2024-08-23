using org.mariuszgromada.math.mxparser;
using UnityEngine;

namespace Moonlet.Utils.MxParser
{
	public class LerpFunction : FunctionExtension
	{
		double a;
		double b;
		double t;

		public LerpFunction()
		{
		}

		public LerpFunction(double a, double b, double t)
		{
			this.a = a;
			this.b = b;
			this.t = t;
		}

		public double calculate() => Mathf.Lerp((float)a, (float)b, (float)t);

		public FunctionExtension clone() => new LerpFunction(a, b, t);

		public string getParameterName(int parameterIndex)
		{
			if (parameterIndex > 2)
				return "n/a";

			return parameterIndex switch
			{
				0 => "a",
				1 => "b",
				_ => "t",
			};
		}

		public int getParametersNumber() => 3;

		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			switch (parameterIndex)
			{
				case 0:
					a = parameterValue;
					break;
				case 1:
					b = parameterValue;
					break;
				case 2:
					t = parameterValue;
					break;
			}
		}
	}
}
