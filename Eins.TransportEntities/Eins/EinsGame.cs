﻿using Eins.TransportEntities.Eins.CustomEventArgs;
using Eins.TransportEntities.EventArgs;
using Eins.TransportEntities.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class EinsGame : IBaseGame
    {
        #region Properties
        public ulong GameID { get; set; }
        public IEnumerable<IBaseCard> CurrentStack { get; set; } = new Stack<IBaseCard>();
        public Dictionary<int, IBasePlayer> Players { get; set; } = new Dictionary<int, IBasePlayer>();
        public string CurrentPlayer { get; set; } = default;
        public GameStatus Status { get; set; } = GameStatus.NotIntialized;
        public object GameRules { get; set; }
        public Type RulesType { get; set; }
        public bool Reversed { get; set; } = false;

        #region IO

        public bool AwaitingUserInput { get; set; } = false;
        public EinsCard.CardColor UserInputColor { get; set; } = 0;

        #endregion

        private Random _rnd { get; set; } = new Random();
        #endregion

        public EinsGame(ulong id, IList<EinsPlayer> players, object rules)
        {
            this.RulesType = typeof(EinsRules);
            this.GameID = id;

            for (int i = 0; i < players.Count; i++)
                this.Players.Add(i, players[i]);

            this.GameRules = rules;
        }

        public async Task<bool> InitializeGame(Hub hub = default)
        {
            var strippedPlayers = new List<EventArgs.StrippedEntities.Player>();
            foreach (var player in this.Players)
            {
                var rules = this.GameRules as EinsRules;
                for (int i = 0; i < rules.CardsPerPlayer; i++)
                {
                    player.Value.HeldCards.Add(GetRandomCard());
                }
                var sp = new EventArgs.StrippedEntities.Player
                {
                    ConnectionID = player.Value.UserSession.GameConnectionId,
                    ID = player.Value.UserSession.UserId,
                    OrderID = player.Key,
                    HeldCardAmount = player.Value.HeldCards.Count,
                    Username = player.Value.UserSession.UserName
                };
                strippedPlayers.Add(sp);
            }
            this.Status = GameStatus.Initialized;
            foreach (var item in this.Players)
            {
                var initArgs = new InitializedEventArgs
                {
                    Code = 200,
                    Players = strippedPlayers,
                    YourCards = item.Value.HeldCards
                };
                var cl = hub.Clients.Client(item.Value.UserSession.LobbyConnectionId);
                await cl.SendAsync("Initialized", initArgs);
            }
            return true;
        }

        public async Task<GameStartedEventArgs> StartGame(Hub hub = default)
        {
            this.CurrentPlayer = this.Players[0].UserSession.GameConnectionId;

            //TODO: Check card events on intial card
            ((Stack<IBaseCard>)this.CurrentStack).Push(GetRandomCard());
            this.Status = GameStatus.Started;
            var topCard = await GetTopCardAsync();
            var startargs = new GameStartedEventArgs
            {
                Code = 200,
                FirstCard = topCard
            };

            return startargs;
        }

        public Task<bool> CanPlayAsync(string playerConnectionID, Hub hub = default)
            => Task.FromResult(playerConnectionID == this.CurrentPlayer);

        public Task<IBaseCard> GetTopCardAsync(Hub hub = default)
        {
            var stack = this.CurrentStack as Stack<IBaseCard>;
            return Task.FromResult(stack.Peek());
        }

        public Task<IBaseCard> DrawCard(string playerConnectionID, Hub hub = default)
        {
            var card = GetRandomCard();
            var player = this.Players.First(x => x.Value.UserSession.GameConnectionId == playerConnectionID);
            player.Value.HeldCards.Add(card);
            return Task.FromResult(card);
        }

        public async Task<bool> PushCard(string playerConnectionID, IBaseCard card, Hub hub = default)
        {
            var player = this.Players.First(x => x.Value.UserSession.GameConnectionId == playerConnectionID);

            if (card is EinsActionCard actionCard)
            {
                switch (actionCard.CardType)
                {
                    case EinsActionCard.ActionCardType.Draw2:
                        await DoPlus2(hub);

                        //Basically skip the next player, maybe do it with the SkipCard method later
                        var next2 = await GetNextPlayer();
                        this.CurrentPlayer = next2.UserSession.GameConnectionId;
                        break;

                    case EinsActionCard.ActionCardType.Draw4:
                        card = await DoPlus4(hub, actionCard);

                        //Basically skip the next player, maybe do it with the SkipCard method later
                        var next4 = await GetNextPlayer();
                        this.CurrentPlayer = next4.UserSession.GameConnectionId;
                        break;

                    case EinsActionCard.ActionCardType.Skip:
                        await DoSkip(hub);
                        break;

                    case EinsActionCard.ActionCardType.Switch:
                        await DoSwitch(hub);
                        break;

                    case EinsActionCard.ActionCardType.Wish:
                        card = await DoWish(hub, actionCard);
                        break;

                    default:
                        break;
                }
            }
            var asStack = this.CurrentStack as Stack<IBaseCard>;
            asStack.Push(card);
            return true;
        }

        public async Task<IBasePlayer> SetNextPlayer(Hub hub = default)
        {
            var next = await GetNextPlayer();
            this.CurrentPlayer = next.UserSession.GameConnectionId;
            return next;
        }

        public Task<bool> IsGameFinished(Hub hub = default)
            => Task.FromResult(this.Players.Any(x => x.Value.HeldCards.Count == 0));

        private IBaseCard GetRandomCard()
        {
            var colors = Enum.GetValues(typeof(EinsCard.CardColor)) as EinsCard.CardColor[];
            var color = colors[this._rnd.Next(0, 5)];

            if (color != EinsCard.CardColor.Black)
                return new EinsCard(this._rnd.Next(0, 10), color);

            else
            {
                var actions = Enum.GetValues(typeof(EinsActionCard.ActionCardType)) as EinsActionCard.ActionCardType[];
                return new EinsActionCard(color, actions[this._rnd.Next(0, 5)]);
            }
        }

        public Task<EinsPlayer> GetNextPlayer()
        {

            var currentPlayer = this.Players.First(x => x.Value.UserSession.GameConnectionId == this.CurrentPlayer);
            if (this.Reversed)
            {
                var nextID = currentPlayer.Key - 1;
                if (nextID == -1)
                    nextID = this.Players.Count - 1;
                return Task.FromResult(this.Players[nextID] as EinsPlayer);
            }
            else
            {
                var nextID = currentPlayer.Key + 1;
                if (nextID == this.Players.Count)
                    nextID = 0;
                return Task.FromResult(this.Players[nextID] as EinsPlayer);
            }
        }

        private async Task<bool> DoPlus2(Hub hub)
        {
            var nextPlayer = await GetNextPlayer();
            var newCard1 = GetRandomCard();
            var newCard2 = GetRandomCard();
            nextPlayer.HeldCards.Add(newCard1);
            nextPlayer.HeldCards.Add(newCard2);

            var playerConnectionsOthers = hub.Clients.Clients(this.Players
                .Where(x => x.Value.UserSession.GameConnectionId != nextPlayer.UserSession.GameConnectionId)
                .Select(x => x.Value.UserSession.GameConnectionId).ToArray());
            var playerConnectionThem = hub.Clients.Client(nextPlayer.UserSession.GameConnectionId);
            var nextPlayerOrderID = this.Players.First(x => x.Value.UserSession.GameConnectionId == nextPlayer.UserSession.GameConnectionId);


            var drawnArgsOthers1 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key);
            var drawnArgsThem1 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key, newCard1);

            var drawnArgsOthers2 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key);
            var drawnArgsThem2 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key, newCard2);

            await playerConnectionThem.SendAsync("CardDrawn", drawnArgsThem1);
            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers1);
            await playerConnectionThem.SendAsync("CardDrawn", drawnArgsThem2);
            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers2);
            return true;
        }

        private async Task<EinsActionCard> DoPlus4(Hub hub, EinsActionCard card)
        {
            var nextPlayer = await GetNextPlayer();
            var newCard1 = GetRandomCard();
            var newCard2 = GetRandomCard();
            var newCard3 = GetRandomCard();
            var newCard4 = GetRandomCard();
            nextPlayer.HeldCards.Add(newCard1);
            nextPlayer.HeldCards.Add(newCard2);
            nextPlayer.HeldCards.Add(newCard3);
            nextPlayer.HeldCards.Add(newCard4);

            var playerConnectionsOthers = hub.Clients.Clients(this.Players
                .Where(x => x.Value.UserSession.GameConnectionId != nextPlayer.UserSession.GameConnectionId)
                .Select(x => x.Value.UserSession.GameConnectionId).ToArray());
            var playerConnectionThem = hub.Clients.Client(nextPlayer.UserSession.GameConnectionId);
            var nextPlayerOrderID = this.Players.First(x => x.Value.UserSession.GameConnectionId == nextPlayer.UserSession.GameConnectionId);

            var drawnArgsOthers1 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key);
            var drawnArgsThem1 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key, newCard1);

            var drawnArgsOthers2 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key);
            var drawnArgsThem2 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key, newCard2);

            var drawnArgsOthers3 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key);
            var drawnArgsThem3 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key, newCard3);

            var drawnArgsOthers4 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key);
            var drawnArgsThem4 = new CardDrawnEventArgs(200, nextPlayer, nextPlayerOrderID.Key, newCard4);


            card.Color = this.UserInputColor;

            this.UserInputColor = 0;

            await playerConnectionThem.SendAsync("CardDrawn", drawnArgsThem1);
            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers1);
            await playerConnectionThem.SendAsync("CardDrawn", drawnArgsThem2);
            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers2); 
            await playerConnectionThem.SendAsync("CardDrawn", drawnArgsThem3);
            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers3);
            await playerConnectionThem.SendAsync("CardDrawn", drawnArgsThem3);
            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers3);
            return card;
        }

        private Task<EinsActionCard> DoWish(Hub hub, EinsActionCard card)
        {
            card.Color = this.UserInputColor;
            this.UserInputColor = 0;
            return Task.FromResult(card);
        }
        private async Task<bool> DoSkip(Hub hub)
        {
            var playerConnections = hub.Clients.Clients(this.Players.Select(x => x.Value.UserSession.GameConnectionId).ToArray());

            var skipped = await GetNextPlayer();
            var skipargs = new SkippedEventArgs
            {
                Code = 200,
                SkippedPlayerConnectionID = skipped.UserSession.GameConnectionId,
                SkippedPlayerID = skipped.ID,
                SkippedPlayerUsername = skipped.Username
            };
            await playerConnections.SendAsync("SkippedPlayer", skipargs);
            var next2 = await GetNextPlayer();
            this.CurrentPlayer = next2.UserSession.GameConnectionId;
            return true;
        }
        private async Task<bool> DoSwitch(Hub hub)
        {
            var playerConnections = hub.Clients.Clients(Players.Select(x => x.Value.UserSession.GameConnectionId).ToArray());

            this.Reversed = !this.Reversed;

            var next = await GetNextPlayer();
            var switchargs = new SwitchedEventArgs
            {
                Code = 200,
                NextPlayerConnectionID = next.UserSession.GameConnectionId,
                NextPlayerID = next.ID,
                NextPlayerUsername = next.Username
            };

            await playerConnections.SendAsync("SwitchedDirection", switchargs);

            return true;
        }
    }
}
