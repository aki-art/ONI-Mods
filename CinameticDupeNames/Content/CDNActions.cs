using PeterHan.PLib.Actions;

namespace CinematicDupeNames.Content
{
    public class CDNActions
    {
        public static PAction ToggleNamesAction { get; private set; }
        public const string TOGGLE_NAMES_IDENTIFIER = "CinameticDupeNames_ToggleNames";

        public static void Register()
        {
            ToggleNamesAction = new PActionManager().CreateAction(
                TOGGLE_NAMES_IDENTIFIER,
                STRINGS.CINEMATICDUPENAMES.TOGGLE_NAMES_ACTION,
                new PKeyBinding(KKeyCode.S, Modifier.Shift | Modifier.Alt));
        }
    }
}
