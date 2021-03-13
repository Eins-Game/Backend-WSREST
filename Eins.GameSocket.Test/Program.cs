using Eins.TransportEntities.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hub = new HubConnectionBuilder()
                .WithUrl($"https://localhost:49153/gamelobby")
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
                    _ = Task.Run(() => SendCard(hub));

                await Task.Delay(1);
            });
            hub.On<int, IBasePlayer, IBaseCard>("PlayCardSuccess", (code, message, card) =>
            {
                Console.WriteLine($"-- {code} Player:{message.Username}");
                //Console.WriteLine($"---- Card Played:{card.Color} {card.Value}");
                return Task.CompletedTask;
            });
            hub.On<int, IBaseCard, IBasePlayer>("TurnNotification", async (code, card, player) =>
            {
                Console.WriteLine($"{code} Its your turn!");
                //Console.WriteLine($"Last card played:{card.Color} {card.Value}");
                //var cards = player.HeldCards.Select(x => $"{x.Color} {x.Value}");
                //var cardstring = string.Join(",", cards);
                //Console.WriteLine("Your cards are: " + cardstring);
                _ = Task.Run(() => SendCard(hub));
                await Task.Delay(1);
            });
            hub.On<int, IBaseCard>("DrawCardSuccess", (code, card) =>
            {
                //Console.WriteLine($"-- {code} Card drawn: {card.Color} {card.Value}");
                return Task.CompletedTask;
            });

            Console.WriteLine("Enter Username");
            var username = Console.ReadLine();
            await hub.SendAsync("JoinGame", username);
            await Task.Delay(-1);
        }

        public static async Task SendCard(HubConnection hub)
        {
            Console.WriteLine("Want to draw a card? (1=yes)");
            while (Convert.ToInt32(Console.ReadLine()) == 1)
            {
                await hub.SendAsync("DrawCard");
            }
            Console.WriteLine("Enter Color (1 Red, 2 Green, 4 Yellow, 8 Blue)");
            var cardColor = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Value");
            var cardValue = Convert.ToInt32(Console.ReadLine());
            //await hub.InvokeAsync("PlayCard", new IBaseCard
            //{
            //    //Color = (CardColor)cardColor,
            //    Value = cardValue
            //});
        }
    }
}
