namespace Moonlet.Scripts.Commands
{
	public class LogCommand : BaseCommand
	{
		public LogData Data { get; set; }

		public class LogData
		{
			public string Message { get; set; }

			public LogData()
			{
				Message = "Empty Message";
			}
		}

		public override void Run(object go)
		{
			Log.Info(Data.Message);
		}
	}
}
