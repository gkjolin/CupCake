﻿using CupCake.Command;
using CupCake.Command.Source;
using CupCake.Messages.Blocks;
using CupCake.Permissions;

namespace CupCake.DefaultCommands.Commands.Utility
{
    public class RedCommand : UtilityCommandBase
    {
        [MinGroup(Group.Moderator)]
        [Command("red")]
        [CorrectUsage("")]
        private void Run(IInvokeSource source, ParsedCommand message)
        {
            this.KeyService.PressKey(Key.Red);
            source.Reply("Pressed red key.");
        }
    }
}