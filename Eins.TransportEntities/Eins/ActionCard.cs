﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class ActionCard : Card
    {
        public ActionCardType CardType { get; set; }
        public ActionCard()
        {
            this.Value = -1;
        }
        public Task<bool> DoThing()
        {
            return Task.FromResult(true);
        }

        public enum ActionCardType
        {
            Draw2,
            Draw4,
            Skip,
            Switch,
            Wish
        }
    }
}