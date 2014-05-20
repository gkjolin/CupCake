﻿using System;
using CupCake.API.Muffins;
using CupCake.EE.Events.Receive;
using CupCake.EE.Events.Send;

namespace CupCake.Muffins
{
    public class EEMuffin : CupCakeMuffin
    {
        protected override void Enable()
        {
            this.MuffinLoader.EnableComplete += this.MuffinLoader_EnableComplete;

            this.Events.Bind<InitReceiveEvent>(this.OnInit);
        }

        private void MuffinLoader_EnableComplete(object sender, EventArgs e)
        {
            this.Events.Raise(new InitSendEvent());
        }

        private void OnInit(object sender, InitReceiveEvent e)
        {
            this.Events.Raise(new Init2SendEvent());
        }
    }
}