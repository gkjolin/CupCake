﻿using CupCake.Command;
using CupCake.Command.Source;
using CupCake.Permissions;

namespace CupCake.DefaultCommands.Commands.Utility
{
    public class RestartCommand : UtilityCommandBase
    {
        [MinGroup(Group.Admin)]
        [Command("restart")]
        [CorrectUsage("")]
        private void Run(IInvokeSource source, ParsedCommand message)
        {
            source.Reply("Restarting...");

            this.HostService.Restart();
        }
    }
}