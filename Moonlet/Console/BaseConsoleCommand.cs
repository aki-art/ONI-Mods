using System.Collections.Generic;

namespace Moonlet.Console
{
	public abstract class BaseConsoleCommand
	{
		public readonly string id;

		public BaseConsoleCommand(string id)
		{
			this.id = id;
			SetupArguments();
		}

		public List<ArgumentInfo[]> expectedArgumentVariations;
		public string[] argumentStrs;

		public virtual CommandResult Run(string[] args)
		{
			var result = ValidateArguments(args);

			if (result.severity == CommandResult.Severity.Success)
				return Run();

			return result;
		}

		public abstract CommandResult Run();

		public virtual string GetAutoComplete(string currentInput)
		{
			return null;
		}

		public virtual string Description() => Strings.Get($"STRINGS.MOONLET.COMMANDS.{id.ToUpperInvariant()}");

		public virtual CommandResult ValidateArguments(string[] args)
		{
			if (expectedArgumentVariations == null || expectedArgumentVariations.Count == 0)
				return args.Length == 1
					? CommandResult.success
					: CommandResult.Warning("This command does not take arguments.");

			// args contains the comand word itself at index 0, the expected arguments do not, so everything is offset by one
			var actualArgumentCount = args.Length - 1;

			var expectedArguments = expectedArgumentVariations[0]; // TODO

			if (actualArgumentCount > expectedArguments.Length)
				return CommandResult.Warning("Too many arguments.");

			for (int i = 0; i < expectedArguments.Length; i++)
			{
				var expectedArg = expectedArguments[i];
				var offsetIdx = i + 1;

				if (args.Length >= offsetIdx)
				{
					return !expectedArg.optional
						? CommandResult.Warning("Too few arguments")
						: CommandResult.success;
				}
				else if (!expectedArg.IsValid(args[offsetIdx]))
					return CommandResult.Warning($"Argument {args[offsetIdx]} is an unexpected value.");
			}

			return CommandResult.success;
		}

		public virtual void SetupArguments()
		{

		}

		public bool GetArgument<ArgumentType, ValueType>(int index, out ValueType value) where ArgumentType : ArgumentInfo<ValueType>
		{
			value = default;

			if (index <= 0)
			{
				Log.Warn("index 0 is the command keyword.");
				return false;
			}

			GetStringAtIndex(index, out var str);

			if (expectedArgumentVariations == null)
			{
				Log.Warn("this command takes no arguments.");
				return false;
			}

			var args = expectedArgumentVariations[0];
			if (args.Length < index)
			{
				Log.Debug($"out of bounds");
				return false;
			}

			var arg = (ArgumentType)args[index - 1];
			return arg.Parse(str, out value);
		}

		public bool GetStringAtIndex(int index, out string value)
		{
			value = null;

			if (argumentStrs == null || argumentStrs.Length <= index)
				return false;

			value = argumentStrs[index];
			return true;
		}

		public bool GetStringArgument(int index, out string value) => GetArgument<StringArgument, string>(index, out value);

		public bool GetFloatArgument(int index, out float value) => GetArgument<FloatArgument, float>(index, out value);

		public bool GetIntArgument(int index, out int value) => GetArgument<IntArgument, int>(index, out value);

		public class ArgumentInfo(string name, string description, bool optional)
		{
			public string name = name;
			public string description = description;
			public bool optional = optional;

			public virtual bool IsValid(string input) => true;
		}

		public abstract class ArgumentInfo<T>(string name, string description, T defaultValue, bool optional) : ArgumentInfo(name, description, optional)
		{
			public T defaultValue = defaultValue;

			public abstract bool Parse(string arg, out T result);
		}

		public class StringArgument(string name, string description, string defaultValue = "", bool optional = true) : ArgumentInfo<string>(name, description, defaultValue, optional)
		{
			public override bool Parse(string arg, out string result)
			{
				result = arg ?? defaultValue;
				return !result.IsNullOrWhiteSpace();
			}
		}

		public class IntArgument(string name, string description, int defaultValue = 0, bool optional = true) : ArgumentInfo<int>(name, description, defaultValue, optional)
		{
			public override bool Parse(string arg, out int result)
			{
				result = defaultValue;
				if (arg != null && int.TryParse(arg, out var userResult))
				{
					result = userResult;
					return true;
				}

				return false;
			}

			public override bool IsValid(string input) => int.TryParse(input, out _);
		}

		public class FloatArgument(string name, string description, float defaultValue = 0f, bool optional = true) : ArgumentInfo<float>(name, description, defaultValue, optional)
		{
			public override bool Parse(string arg, out float result)
			{
				result = defaultValue;
				if (arg != null && float.TryParse(arg, out var userResult))
				{
					result = userResult;
					return true;
				}

				return false;
			}

			public override bool IsValid(string input) => float.TryParse(input, out _);
		}
	}
}
