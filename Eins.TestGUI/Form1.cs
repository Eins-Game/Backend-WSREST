using Eins.TransportEntities;
using Eins.TransportEntities.Converters;
using Eins.TransportEntities.EventArgs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eins.TestGUI
{
    public partial class Form1 : Form
    {
        private HubConnection connection { get; set; }
        private SessionUser userSession { get; set; }

        JsonSerializerOptions JsonDefaults = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public Form1()
        {
            InitializeComponent(); 
            connection = new HubConnectionBuilder()
                 .WithUrl("https://localhost:49153/lobby")
                 .Build();
            connection.On("Ack", HeartBeat);
            connection.On<int, AuthenticatedEventArgs>("Authenticated", Authenticated);
            connection.On<int, ExceptionEventArgs>("LobbyException", LobbyException);
            connection.On<int, LobbyCreatedEventArgs>("LobbyCreated", LobbyCreated);
            connection.On<int, LobbyPlayerPromotedEventArgs>("PlayerPromoted", LobbyPlayerPromoted);
            connection.On<int, LobbyRemovedEventArgs>("LobbyRemoved", LobbyRemoved);
            connection.On<int, LobbyPlayerJoinedEventArgs>("PlayerJoined", PlayerJoined);
            connection.On<int, LobbyPlayerLeftEventArgs>("PlayerLeft", PlayerLeft);
            connection.On<int, LobbyGameCreatedEventArgs>("LobbyGameCreated", LobbyGameCreated);
            connection.On<object>("Initialized", Initializedd);
        }

        private async void lobbyConnect_Click(object sender, EventArgs e)
        {
            await connection.StartAsync();
            await connection.SendAsync("Heartbeat");
            this.logToRTB("Connected");
        }

        private async void lobbyDisconnect_Click(object sender, EventArgs e)
        {
            await this.connection.StopAsync();
            this.logToRTB("Disconnected");
        }

        private async void lobbyAuth_Click(object sender, EventArgs e)
        {
            await this.connection.SendAsync("Authenticate", this.lobbyAuthUsername.Text);
            this.logToRTB("Sent Authenticate");
        }

        private async void lobbyReAuth_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.lobbyReAuthSecret.Text))
                await this.connection.SendAsync("ReAuthenticate", this.userSession.Secret);
            else
                await this.connection.SendAsync("ReAuthenticate", this.userSession.Secret, this.lobbyReAuthSecret.Text);
            this.logToRTB("Sent ReAuthenticate");
        }

        private async void lobbyCreateLobby_Click(object sender, EventArgs e)
        {
            await this.connection.SendAsync("CreateLobby",
                this.lobbyCreateLobbyName.Text,
                this.lobbyCreateLobbyPassword.Text);
            this.logToRTB("Sent LobbyCreate");
        }

        private async void lobbyRemoveLobby_Click(object sender, EventArgs e)
        {
            await this.connection.SendAsync("RemoveLobby", Convert.ToUInt64(this.lobbyRemoveLobbyId.Text));
            logToRTB("Sent LobbyRemove");
        }

        private async void lobbyPlayerJoin_Click(object sender, EventArgs e)
        {
            await this.connection.SendAsync("PlayerJoin", this.lobbyPlayerJoinLobbyId.Text, this.lobbyPlayerJoinPassword.Text);
            this.logToRTB("Sent PlayerJoin");
        }

        private async void lobbyPlayerLeft_Click(object sender, EventArgs e)
        {
            await this.connection.SendAsync("PlayerLeft", this.lobbyPlayerLeftLobbyId.Text);
            this.logToRTB("Sent PlayerLeft");
        }

        private async void lobbyCreateGame_Click(object sender, EventArgs e)
        {
            await this.connection.SendAsync("CreateGame", Convert.ToUInt64(this.lobbyCreateGameLobbyId.Text));
            this.logToRTB("Sent GameCreate");
        }

        #region EventHandlers

        private async void HeartBeat()
        {
            await connection.SendAsync("Heartbeat");
            if (this.logHearbeat.Checked)
                this.logToRTB("Heartbeated");
        }

        private void Authenticated(int code, AuthenticatedEventArgs e)
        {
            this.userSession = e.UserSession;
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        private void LobbyException(int code, ExceptionEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        private void LobbyCreated(int code, LobbyCreatedEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        private void LobbyPlayerPromoted(int code, LobbyPlayerPromotedEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        private void LobbyRemoved(int code, LobbyRemovedEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        private void PlayerJoined(int code, LobbyPlayerJoinedEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }
        
        private void PlayerLeft(int code, LobbyPlayerLeftEventArgs e)
        {
            this.logToRTB(e);
        }

        private void LobbyGameCreated(int code, LobbyGameCreatedEventArgs e)
        {
            this.logToRTB(e);
        }

        private void Initializedd(object e)
        {
            var e2 = e.ToString();
            var deserializeOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false
            };
            //deserializeOptions.Converters.Add(new EinsCardListConverter());
            //deserializeOptions.Converters.Add(new EinsActionCardConverter());
            var hm = JsonSerializer.Deserialize<InitializedEventArgs>(e2, deserializeOptions);
            this.logToRTB(e);
        }

        #endregion


        private void logToRTB(string message)
        {
            this.richTextBox1.Text += $"{DateTime.UtcNow} || {message} \n";
        }
        private void logToRTB(object message)
        {
            this.richTextBox1.Text += $"{DateTime.UtcNow} || {JsonSerializer.Serialize(message, JsonDefaults)} \n";
        }

    }
}
