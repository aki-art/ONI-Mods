using HarmonyLib;
using Moonlet.Templates;

namespace Moonlet.Console.Commands
{
	public class SetTemperatureCommand() : CommandBase("settemp")
	{
		public override CommandResult Run(string[] args)
		{
			if (args.Length < 2)
				return CommandResult.Error("Incorrect amount of arguments.");

			var go = SelectTool.Instance.selected;

			if (go == null)
				return CommandResult.Warning("Nothing is selected.");

			if (go.TryGetComponent(out PrimaryElement element))
			{
				var arg = args.Join(delimiter: "");
				arg = arg.Remove(0, args[0].Length);
				arg = arg.Replace(" ", "");

				var temp = new TemperatureNumber();
				temp.SetExpression(arg);

				var k = temp.Calculate();

				if (k == float.NaN)
					return CommandResult.Error("Invalid expression");

				element.Temperature = k;

				var c = GameUtil.GetTemperatureConvertedFromKelvin(element.Temperature, GameUtil.TemperatureUnit.Celsius);
				var f = GameUtil.GetTemperatureConvertedFromKelvin(element.Temperature, GameUtil.TemperatureUnit.Fahrenheit);

				DevConsole.Log($"Set temperature to {k}K / {c}°C / {f}F");
			}
			else
				return CommandResult.Warning($"Cannot set temperature of {go.PrefabID()}.");

			return CommandResult.success;
		}
	}
}
