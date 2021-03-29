using Eins.TransportEntities.Eins;
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
        static async Task Main(string[] args)
        {
            //await Task.Delay(5000);
            //var g = new Game(0, new List<Player>(), default);
            ////g.GetRandomCard();
            //
            //var connection = new HubConnectionBuilder()
            //    .WithUrl("https://localhost:49153/test")
            //    .Build();
            //
            //await connection.StartAsync();
            //
            //await connection.SendAsync("CreateLobby", "Test", "TestPW");
            //
            //await Task.Delay(2500);
            //
            //await connection.SendAsync("GetAllLobbies");
            //
            //await Task.Delay(-1);

            Test("Test", x => x.ToUpper());
        }


        public static void Test(string test, Func<string,string> predicate)
        {
            Console.WriteLine(predicate.Invoke(test));
        }
    }
}
