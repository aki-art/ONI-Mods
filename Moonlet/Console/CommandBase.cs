using System.Collections.Generic;

namespace Moonlet.Console
{
	public abstract class CommandBase(string id)
	{
		public readonly string id = id;

		public List<ArgumentInfo[]> arguments;

		public abstract CommandResult Run(string[] args);

		public virtual string Description() => Strings.Get($"STRINGS.MOONLET.COMMANDS.{id.ToUpperInvariant()}");

		public virtual void ValidateArguments(string[] args)
		{

		}

		// string[] GetAutofillForArgument(string str, int index)

		public class ArgumentInfo
		{
			public string name;
			public string description;
			public bool optional;
		}

		public abstract class ArgumentInfo<T> : ArgumentInfo
		{
			public abstract bool Parse(string arg, out T result);
		}

		public class IntArgument : ArgumentInfo<int>
		{
			public override bool Parse(string arg, out int result) => int.TryParse(arg, out result);
		}
	}
}
