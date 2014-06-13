﻿using CupCake.Core.Events;
using CupCake.Messages.Blocks;

namespace CupCake.Potions
{
    public class PotionCountEvent : Event
    {
        public PotionCountEvent(Potion potion, int count)
        {
            this.Potion = potion;
            this.Count = count;
        }

        public Potion Potion { get; private set; }
        public int Count { get; private set; }
    }
}