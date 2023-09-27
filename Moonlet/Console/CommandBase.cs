namespace Moonlet.Console
{
	public abstract class CommandBase(string id)
	{
		public readonly string id = id;

		public abstract CommandResult Run(string[] args);

		public virtual string Description() => Strings.Get($"STRINGS.MOONLET.COMMANDS.{id.ToUpperInvariant()}");

		public virtual void ValidateArguments(string[] args)
		{

		}
	}
}
