using Eins.TransportEntities.Eins;
using Eins.TransportEntities.EventArgs;
using Eins.TransportEntities.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Test
{
    class Program
    {
        static HubConnection connection { get; set; }
        static async Task Main(string[] args)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:49155/lobby")
                .Build();

            await connection.StartAsync();

            connection.On("Ack", HeartBeat);

            connection.On<int, AuthenticatedEventArgs>("Authenticated", AuthHappened);

            await connection.SendAsync("Heartbeat");

            await Task.Delay(-1);
        }

        private static async void HeartBeat()
        {
            await connection.SendAsync("Heartbeat");
            Console.WriteLine("Hearbeat happened");
        }

        private static void AuthHappened(int arg1, AuthenticatedEventArgs arg2)
        {
            Console.WriteLine(arg1);
        }
    }
}
