namespace Moonlet.Console
{
	public class CommandResult(string message, CommandResult.Severity severity)
	{
		public readonly string message = message;
		public readonly Severity severity = severity;
		public static readonly CommandResult success = Success();
		public static readonly CommandResult argCountError = Error("Incorrect number of arguments.");
		public static readonly CommandResult emptySelection = Error("Nothing is selected.");

		public static CommandResult Success(object message = null) => new(message == null ? null : $"<color=#888888> {message}</color>", Severity.Success);

		public static CommandResult Warning(object message) => new(message == null ? null : $"<color=#FF8800>WARNING: {message}</color>", Severity.Warning);

		public static CommandResult Error(object message) => new(message == null ? null : $"<color=#FF0000>ERROR: {message}</color>", Severity.Error);

		public enum Severity
		{
			Success,
			Warning,
			Error
		}
	}
}
