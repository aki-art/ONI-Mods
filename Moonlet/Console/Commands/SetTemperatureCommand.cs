namespace Moonlet.Console.Commands
{
	public class SetTemperatureCommand() : CommandBase("settemp")
	{
		public override CommandResult Run(string[] args)
		{
			if (args.Length != 2)
				return CommandResult.Error("Incorrect amount of arguments.");

			if (!float.TryParse(args[1], out var temp))
				return CommandResult.Error($"{args[1]} is not a valid temperature.");

			var go = SelectTool.Instance.selected;

			if (go == null)
				return CommandResult.Warning("Nothing is selected.");

			if (go.TryGetComponent(out PrimaryElement element))
				element.Temperature = GameUtil.GetTemperatureConvertedToKelvin(temp, GameUtil.TemperatureUnit.Celsius);
			else
				return CommandResult.Warning($"Cannot set temperature of {go.PrefabID()}.");

			return CommandResult.success;
		}
	}
}
