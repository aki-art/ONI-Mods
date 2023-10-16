using Moonlet.Utils.MxParser;
using PeterHan.PLib.Core;

namespace Moonlet.Console.Commands
{
	public class MathCommand() : CommandBase("math")
	{
		public override void SetupArguments()
		{
			base.SetupArguments();
			arguments = new()
			{
				new ArgumentInfo[]
				{
					new StringArgument("expression", "Math expression to calculate", optional: false)
				},
			};
		}

		public override CommandResult Run()
		{
			var args = argumentStrs;

			var completeString = args.Join(" ");
			completeString = completeString.Remove(0, "math ".Length);

			var expression = new MExpression(completeString);

			var error = expression.getErrorMessage();

			return error.IsNullOrWhiteSpace()
				? CommandResult.Success(expression.calculate())
				: CommandResult.Error(error);
		}
	}
}
