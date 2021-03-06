﻿using CupCake.Command;
using CupCake.Command.Source;
using CupCake.Permissions;

namespace CupCake.DefaultCommands.Commands.Utility
{
    public class HelpCommand : UtilityCommandBase
    {
        [MinArgs(1)]
        [MinGroup(Group.Moderator)]
        [Command("help")]
        [CorrectUsage("command")]
        private void Run(IInvokeSource source, ParsedCommand message)
        {
            this.CommandService.Invoke(source, new HelpRequest(message.Args[0]));
        }
    }
}