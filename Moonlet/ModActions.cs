using PeterHan.PLib.Actions;

namespace Moonlet
{
	public class ModActions
	{
		public static PAction OpenConsole { get; private set; }

		public const string OPEN_CONSOLE_ID = "Moonlet_OpenConsole";

		public static void Register()
		{
			OpenConsole = new PActionManager().CreateAction(
				OPEN_CONSOLE_ID,
				"Open Console",
				new PKeyBinding(KKeyCode.Alpha0, Modifier.Alt));
		}
	}
}
