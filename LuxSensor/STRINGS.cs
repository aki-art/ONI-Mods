
using STRINGS;

namespace LuxSensor
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class LS_LOGICLIGHTSENSOR
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Lux Sensor", nameof(LS_LOGICLIGHTSENSOR));
                    public static LocString DESC = "Lux sensors can be used to optimize Shinebug farms or Solar power plants.";
                    public static LocString EFFECT = "Sends a " + global::STRINGS.UI.FormatAsAutomationState("Green Signal", global::STRINGS.UI.AutomationState.Active) + " or a " + global::STRINGS.UI.FormatAsAutomationState("Red Signal", global::STRINGS.UI.AutomationState.Standby) + " when " + global::STRINGS.UI.FormatAsLink("Light", "LIGHT") + " level enters the chosen range.";
                    public static LocString LOGIC_PORT = global::STRINGS.UI.FormatAsLink("Light", "LIGHT") + " Level";
                    public static LocString LOGIC_PORT_ACTIVE = "Sends a " + global::STRINGS.UI.FormatAsAutomationState("Green Signal", global::STRINGS.UI.AutomationState.Active) + " if Light level is within the selected range";
                    public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + global::STRINGS.UI.FormatAsAutomationState("Red Signal", global::STRINGS.UI.AutomationState.Standby);
                }
            }
        }

        public class UI
        {
            public class UISIDESCREENS
            {
                public class THRESHOLD_SWITCH_SIDESCREEN
                {
                    public static LocString LIGHT = "Light level";
                    public static LocString LIGHT_TOOLTIP_ABOVE = "Will send a " + global::STRINGS.UI.FormatAsAutomationState("Green Signal", global::STRINGS.UI.AutomationState.Active) + " if the " + global::STRINGS.UI.PRE_KEYWORD + "Light level" + global::STRINGS.UI.PST_KEYWORD + " is above <b>{0}</b>";
                    public static LocString LIGHT_TOOLTIP_BELOW = "Will send a " + global::STRINGS.UI.FormatAsAutomationState("Green Signal", global::STRINGS.UI.AutomationState.Active) + " if the " + global::STRINGS.UI.PRE_KEYWORD + "Light level" + global::STRINGS.UI.PST_KEYWORD + " is below <b>{0}</b>";
                }
            }
        }
    }
}