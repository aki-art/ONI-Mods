using PeterHan.PLib.Actions;

namespace Backwalls
{
	internal class BWActions
	{
		public static PAction SmartBuildAction { get; private set; }
		public const string TOGGLE_NAMES_IDENTIFIER = "Backwalls_SmartBuild";

		public static void Register()
		{
			SmartBuildAction = new PActionManager().CreateAction(
				TOGGLE_NAMES_IDENTIFIER,
				STRINGS.UI.BACKWALLS_ACTIONS.SMART_BUILD,
				new PKeyBinding(KKeyCode.None, Modifier.Ctrl));
		}
	}
}
