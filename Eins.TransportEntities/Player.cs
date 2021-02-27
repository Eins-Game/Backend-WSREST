﻿using Microsoft.AspNetCore.SignalR;
using System;

namespace Eins.TransportEntities
{
    public class Player
    {
        public ulong PlayerID { get; set; }
        public string Username { get; set; }
        public bool Authorized { get; set; } = false;
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
    }
}