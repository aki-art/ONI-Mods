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

		public List<ArgumentInfo[]> arguments;
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
			if (arguments == null)
				return args.Length == 1 ? CommandResult.success : CommandResult.Warning("This command does not take argumants.");

			bool isValid = false;
			foreach (var argTemplate in arguments)
			{
				if (argTemplate.Length == args.Length - 1)
				{
					for (int i = 0; i < argTemplate.Length; i++)
					{
						ArgumentInfo a = argTemplate[i];
						if (!a.IsValid(args[i + 1]))
							break;
					}

					isValid = true;
					break;
				}
			}

			return !isValid ? CommandResult.Warning("Incorrect arguments.") : CommandResult.success;
		}

		public virtual void SetupArguments()
		{

		}

		public bool GetIntArgument(int index, out int value)
		{
			value = default;
			return GetStringArgument(index, out var str) && int.TryParse(str, out value);
		}

		public bool GetFloatArgument(int index, out float value)
		{
			value = default;
			return GetStringArgument(index, out var str) && float.TryParse(str, out value);
		}

		public bool GetStringArgument(int index, out string value)
		{
			value = null;

			if (argumentStrs == null || argumentStrs.Length <= index)
				return false;

			value = argumentStrs[index];
			return true;
		}

		public class ArgumentInfo(string name, string description, bool optional)
		{
			public string name = name;
			public string description = description;
			public bool optional = optional;

			public virtual bool IsValid(string input) => true;
		}

		public abstract class ArgumentInfo<T> : ArgumentInfo
		{
			public T defaultValue;

			protected ArgumentInfo(string name, string description, T defaultValue, bool optional) : base(name, description, optional)
			{
				this.defaultValue = defaultValue;
			}

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
				return arg != null && int.TryParse(arg, out result);
			}

			public override bool IsValid(string input) => int.TryParse(input, out _);
		}

		public class FloatArgument(string name, string description, float defaultValue = 0f, bool optional = true) : ArgumentInfo<float>(name, description, defaultValue, optional)
		{
			public override bool Parse(string arg, out float result)
			{
				result = defaultValue;
				return arg != null && float.TryParse(arg, out result);
			}

			public override bool IsValid(string input) => float.TryParse(input, out _);
		}
	}
}
