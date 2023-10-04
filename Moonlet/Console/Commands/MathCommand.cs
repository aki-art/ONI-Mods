using Moonlet.Utils.MxParser;
using PeterHan.PLib.Core;

namespace Moonlet.Console.Commands
{
	public class MathCommand() : CommandBase("math")
	{
		public override CommandResult Run(string[] args)
		{
			if (args.Length < 2)
				return CommandResult.argCountError;

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
