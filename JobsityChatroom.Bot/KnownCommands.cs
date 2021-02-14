using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChatroom.Bot
{
    public static class KnownCommands
    {
        private static Dictionary<string, string> GetAllCommands()
        {
            var commands = new Dictionary<string, string>();
            commands.Add("/STOCK", "JobsityChatroom.Bot.APIConsumer");

            return commands;
        }

        public static bool KnowsCommand(string command)
        {
            return GetAllCommands().ContainsKey(command);
        }

        public static string GetHandlerName(string command)
        {
            return GetAllCommands().GetValueOrDefault(command);
        }

    }
}
