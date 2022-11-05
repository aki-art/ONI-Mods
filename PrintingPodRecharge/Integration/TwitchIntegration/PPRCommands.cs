using OniTwitchLib;
using System.Collections.Generic;
using ActionConfig = System.Tuple<System.Func<bool>, System.Action>;
using CommandConfig = System.Tuple<string, int, float, System.Collections.Generic.Dictionary<string, object>>;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    [TwitchAddonMain]
    public class CDBCommands
    {
        public static List<string> ValidIds() => Mod.Settings.TwitchIntegration ? new List<string>
        {
            PrintingPodLeakCommand.ID,
            UselessPrintsCommand.ID,
            WackyDupeCommand.ID
        } : null;

        public static CommandConfig DefaultConfigForCommand(string commandId)
        {
            switch (commandId)
            {
                case PrintingPodLeakCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.PRINTING_POD_LEAK.NAME, Danger.None, 20f);

                case UselessPrintsCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.USELESS_PRINTS.NAME, Danger.None, 20f);

                case WackyDupeCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.WACKY_DUPE.NAME, Danger.None, 10f);

                default:
                    return null;
            }
        }

        public static ActionConfig ActionForCommand(string commandId, int danger, Dictionary<string, object> data)
        {
            switch (commandId)
            {
                case PrintingPodLeakCommand.ID:
                    return new ActionConfig(PrintingPodLeakCommand.Condition, PrintingPodLeakCommand.Run);

                case UselessPrintsCommand.ID:
                    return new ActionConfig(UselessPrintsCommand.Condition, UselessPrintsCommand.Run);

                case WackyDupeCommand.ID:
                    return new ActionConfig(WackyDupeCommand.Condition, WackyDupeCommand.Run);

                default:
                    return null;
            }
        }

        private static CommandConfig CreateCommandConfig(string name, Danger danger, float weight, Dictionary<string, object> settings = null)
        {
            return new CommandConfig(name, (int)danger, weight, settings);
        }
    }
}
