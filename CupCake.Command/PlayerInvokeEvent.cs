﻿using CupCake.Core.Events;
using CupCake.Permissions;
using CupCake.Players;

namespace CupCake.Command
{
    public class PlayerInvokeEvent : Event
    {
        public PlayerInvokeEvent(Player player, ParsedCommand message, Group @group)
        {
            this.Player = player;
            this.Message = message;
            this.Group = @group;
        }

        public Player Player { get; set; }
        public ParsedCommand Message { get; set; }
        public Group Group { get; set; }
        public bool Handled { get; set; }
    }
}