using CrittersDropBones.Integration.TwitchIntegration.Commands;
using OniTwitchLib;
using System;
using System.Collections.Generic;
using ActionConfig = System.Tuple<System.Func<bool>, System.Action>;
using CommandConfig = System.Tuple<string, int, float, System.Collections.Generic.Dictionary<string, object>>;

namespace CrittersDropBones.Integration.TwitchIntegration
{
    [TwitchAddonMain]
    public class CDBCommands
    {
        public static List<string> ValidIds()
        {
            return new List<string>
            {
                MessyMessHallCommand.ID,
                SoupRainCommand.ID,
            };
        }

        public static CommandConfig DefaultConfigForCommand(string commandId)
        {
            switch (commandId)
            {
                case MessyMessHallCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.MESSY_MESS_HALL.NAME, Danger.Minimal, 10f, MessyMessHallCommand.GetDefaultConfig());

                case SoupRainCommand.ID:
                    return CreateCommandConfig(STRINGS.TWITCH.SOUP_RAIN.NAME, Danger.None, 10f);

                default:
                    return null;
            }
        }

        public static ActionConfig ActionForCommand(string commandId, int danger, Dictionary<string, object> data)
        {
            switch (commandId)
            {
                case MessyMessHallCommand.ID:
                    return CreateActions(MessyMessHallCommand.Condition, () => MessyMessHallCommand.Run(danger, data));

                case SoupRainCommand.ID:
                    return CreateActions(SoupRainCommand.Condition, SoupRainCommand.Run);

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
}
