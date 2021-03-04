using Eins.TransportEntities.TestEntities;
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

            //await Task.Delay(5000); //Wait for shit to start up just in case

            var hub = new HubConnectionBuilder()
                .WithUrl("https://localhost:49153/gamelobby")
                .Build();

            await hub.StartAsync();

            hub.On<int, string>("JoinGameSuccess", (code, message) =>
            {
                Console.WriteLine($"-- {code} {message}");
                return Task.CompletedTask;
            });
            hub.On<int, string>("PlayCardFailed", async (code, message) =>
            {
                Console.WriteLine($"{code} {message}");
                if (code == 400)
                {
                    _ = Task.Run(() => SendCard(hub));
                }
                await Task.Delay(1);
            });
            hub.On<int, string, Card>("PlayCardSuccess", (code, message, card) =>
            {
                Console.WriteLine($"-- {code} Player:{message}");
                Console.WriteLine($"---- Card Played:{card.Color} {card.Value}");
                return Task.CompletedTask;
            });
            hub.On<int, Card>("TurnNotification", async (code, card) =>
            {
                Console.WriteLine($"{code} Its your turn!");
                Console.WriteLine($"Last card played:{card.Color} {card.Value}");
                _ = Task.Run(() => SendCard(hub));
                await Task.Delay(1);
            });

            await hub.SendAsync("JoinGame");
            await Task.Delay(-1);
        }

        public static async Task SendCard(HubConnection hub)
        {
            Console.WriteLine("Enter Color (1 Red, 2 Green, 4 Yellow, 8 Blue)");
            var cardColor = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Value");
            var cardValue = Convert.ToInt32(Console.ReadLine());
            await hub.InvokeAsync("PlayCard", new Card
            {
                Color = (CardColor)cardColor,
                Value = cardValue
            });
        }
    }
}
