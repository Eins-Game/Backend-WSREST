using Eins.TransportEntities;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Eins.GameSocket.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await Task.Delay(5000); //Wait for shit to start up just in case

            var hub = new HubConnectionBuilder()
                .WithUrl("https://localhost:49153/gamelobby")
                .Build();

            await hub.StartAsync();

            hub.On<string>("MessageCreated", async (name) =>
            {
                Console.WriteLine(name);
                await Task.Delay(10);
            });
            ulong i = 0;
            //while (true)
            //{
                await hub.InvokeAsync("RequestLobbies"); 
            //    await Task.Delay(500);
            //}
        }
    }
}
