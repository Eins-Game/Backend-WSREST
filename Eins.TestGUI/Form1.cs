using Eins.TransportEntities;
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
        public Form1()
        {
            InitializeComponent(); 
            connection = new HubConnectionBuilder()
                 .WithUrl("https://localhost:49159/lobby")
                 .Build();
            connection.On("Ack", HeartBeat);
            connection.On<int, AuthenticatedEventArgs>("Authenticated", Authenticated);
            connection.On<int, LobbyCreatedEventArgs>("LobbyCreated", LobbyCreated);
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

        private void LobbyCreated(int code, LobbyCreatedEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        JsonSerializerOptions JsonDefaults = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private void LobbyPlayerPromoted(int code, LobbyPlayerPromotedEventArgs e)
        {
            this.logToRTB(JsonSerializer.Serialize(e, JsonDefaults));
        }

        #endregion


        private void logToRTB(string message)
        {
            this.richTextBox1.Text += $"{DateTime.UtcNow} || {message} \n";
        }
    }
}
