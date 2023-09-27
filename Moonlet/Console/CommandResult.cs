namespace Moonlet.Console
{
	public class CommandResult(string message, CommandResult.Severity severity)
	{
		public readonly string message = message;
		public readonly Severity severity = severity;

		public static CommandResult Success(string message = null) => new($"<color=#888888>WARNING: {message}</color>", Severity.Success);

		public static CommandResult Warning(string message) => new($"<color=#FF8800>WARNING: {message}</color>", Severity.Warning);

		public static CommandResult Error(string message) => new($"<color=#FF0000>ERROR: {message}</color>", Severity.Error);

		public enum Severity
		{
			Success,
			Warning,
			Error
		}
	}
}
