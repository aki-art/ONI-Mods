/*using OniTwitchLib;
using System;
using System.Collections.Generic;
using ActionConfig = System.Tuple<System.Func<bool>, System.Action>;
using CommandConfig = System.Tuple<string, int, float, System.Collections.Generic.Dictionary<string, object>>;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    [TwitchAddonMain]
    public class CDBCommands
    {
        public static List<string> ValidIds()
        {
            return new List<string>
            {
                PrintingPodLeakCommand.ID,
                UselessPrintsCommand.ID
            };
        }

        public static CommandConfig DefaultConfigForCommand(string commandId)
        {
            switch (commandId)
            {
                case PrintingPodLeakCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.PRINTING_POD_LEAK.NAME, Danger.None, 10f);

                case UselessPrintsCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.USELESS_PRINTS.NAME, Danger.None, 10f);

                default:
                    return null;
            }
        }

        public static ActionConfig ActionForCommand(string commandId, int danger, Dictionary<string, object> data)
        {
            switch (commandId)
            {
                case PrintingPodLeakCommand.ID:
                    return CreateActions(PrintingPodLeakCommand.Condition, PrintingPodLeakCommand.Run);

                case UselessPrintsCommand.ID:
                    return CreateActions(UselessPrintsCommand.Condition, () => UselessPrintsCommand.Run(danger));

                default:
                    return null;
            }
        }

        private static CommandConfig CreateCommandConfig(string name, Danger danger, float weight, Dictionary<string, object> settings = null)
        {
            return new CommandConfig(name, (int)danger, weight, settings);
        }

        private static ActionConfig CreateActions(Func<bool> condition, System.Action run)
        {
            return new ActionConfig(condition, run);
        }
    }
}*/