
namespace Eins.TestGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lobbyDisconnect = new System.Windows.Forms.Button();
            this.lobbyReAuthSecret = new System.Windows.Forms.TextBox();
            this.logHearbeat = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lobbyConnect = new System.Windows.Forms.Button();
            this.lobbyCreateGameLobbyId = new System.Windows.Forms.TextBox();
            this.lobbyCreateGame = new System.Windows.Forms.Button();
            this.lobbyPlayerLeftLobbyId = new System.Windows.Forms.TextBox();
            this.lobbyPlayerLeft = new System.Windows.Forms.Button();
            this.lobbyPlayerJoinPassword = new System.Windows.Forms.TextBox();
            this.lobbyPlayerJoinLobbyId = new System.Windows.Forms.TextBox();
            this.lobbyPlayerJoin = new System.Windows.Forms.Button();
            this.lobbyChangeGameModeSettings = new System.Windows.Forms.Button();
            this.lobbyChangeLobbyGeneralSettings = new System.Windows.Forms.Button();
            this.lobbyRemoveLobbyId = new System.Windows.Forms.TextBox();
            this.lobbyRemoveLobby = new System.Windows.Forms.Button();
            this.lobbyCreateLobbyPassword = new System.Windows.Forms.TextBox();
            this.lobbyCreateLobbyName = new System.Windows.Forms.TextBox();
            this.lobbyCreateLobby = new System.Windows.Forms.Button();
            this.lobbyGetAllLobbies = new System.Windows.Forms.Button();
            this.lobbyAuthUsername = new System.Windows.Forms.TextBox();
            this.lobbyReAuth = new System.Windows.Forms.Button();
            this.lobbyAuth = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(548, 622);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lobbyDisconnect);
            this.tabPage1.Controls.Add(this.lobbyReAuthSecret);
            this.tabPage1.Controls.Add(this.logHearbeat);
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Controls.Add(this.lobbyConnect);
            this.tabPage1.Controls.Add(this.lobbyCreateGameLobbyId);
            this.tabPage1.Controls.Add(this.lobbyCreateGame);
            this.tabPage1.Controls.Add(this.lobbyPlayerLeftLobbyId);
            this.tabPage1.Controls.Add(this.lobbyPlayerLeft);
            this.tabPage1.Controls.Add(this.lobbyPlayerJoinPassword);
            this.tabPage1.Controls.Add(this.lobbyPlayerJoinLobbyId);
            this.tabPage1.Controls.Add(this.lobbyPlayerJoin);
            this.tabPage1.Controls.Add(this.lobbyChangeGameModeSettings);
            this.tabPage1.Controls.Add(this.lobbyChangeLobbyGeneralSettings);
            this.tabPage1.Controls.Add(this.lobbyRemoveLobbyId);
            this.tabPage1.Controls.Add(this.lobbyRemoveLobby);
            this.tabPage1.Controls.Add(this.lobbyCreateLobbyPassword);
            this.tabPage1.Controls.Add(this.lobbyCreateLobbyName);
            this.tabPage1.Controls.Add(this.lobbyCreateLobby);
            this.tabPage1.Controls.Add(this.lobbyGetAllLobbies);
            this.tabPage1.Controls.Add(this.lobbyAuthUsername);
            this.tabPage1.Controls.Add(this.lobbyReAuth);
            this.tabPage1.Controls.Add(this.lobbyAuth);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(540, 594);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "LobbyHub";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lobbyDisconnect
            // 
            this.lobbyDisconnect.Location = new System.Drawing.Point(288, 308);
            this.lobbyDisconnect.Name = "lobbyDisconnect";
            this.lobbyDisconnect.Size = new System.Drawing.Size(246, 23);
            this.lobbyDisconnect.TabIndex = 21;
            this.lobbyDisconnect.Text = "lobbyDisconnect";
            this.lobbyDisconnect.UseVisualStyleBackColor = true;
            this.lobbyDisconnect.Click += new System.EventHandler(this.lobbyDisconnect_Click);
            // 
            // lobbyReAuthSecret
            // 
            this.lobbyReAuthSecret.Location = new System.Drawing.Point(194, 39);
            this.lobbyReAuthSecret.Name = "lobbyReAuthSecret";
            this.lobbyReAuthSecret.Size = new System.Drawing.Size(340, 23);
            this.lobbyReAuthSecret.TabIndex = 20;
            // 
            // logHearbeat
            // 
            this.logHearbeat.AutoSize = true;
            this.logHearbeat.Checked = true;
            this.logHearbeat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logHearbeat.Location = new System.Drawing.Point(7, 337);
            this.logHearbeat.Name = "logHearbeat";
            this.logHearbeat.Size = new System.Drawing.Size(91, 19);
            this.logHearbeat.TabIndex = 19;
            this.logHearbeat.Text = "logHearbeat";
            this.logHearbeat.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(7, 362);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(527, 217);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // lobbyConnect
            // 
            this.lobbyConnect.Location = new System.Drawing.Point(7, 308);
            this.lobbyConnect.Name = "lobbyConnect";
            this.lobbyConnect.Size = new System.Drawing.Size(275, 23);
            this.lobbyConnect.TabIndex = 18;
            this.lobbyConnect.Text = "lobbyConnect";
            this.lobbyConnect.UseVisualStyleBackColor = true;
            this.lobbyConnect.Click += new System.EventHandler(this.lobbyConnect_Click);
            // 
            // lobbyCreateGameLobbyId
            // 
            this.lobbyCreateGameLobbyId.Location = new System.Drawing.Point(195, 279);
            this.lobbyCreateGameLobbyId.Name = "lobbyCreateGameLobbyId";
            this.lobbyCreateGameLobbyId.Size = new System.Drawing.Size(339, 23);
            this.lobbyCreateGameLobbyId.TabIndex = 17;
            // 
            // lobbyCreateGame
            // 
            this.lobbyCreateGame.Location = new System.Drawing.Point(7, 279);
            this.lobbyCreateGame.Name = "lobbyCreateGame";
            this.lobbyCreateGame.Size = new System.Drawing.Size(181, 23);
            this.lobbyCreateGame.TabIndex = 16;
            this.lobbyCreateGame.Text = "lobbyCreateGame";
            this.lobbyCreateGame.UseVisualStyleBackColor = true;
            // 
            // lobbyPlayerLeftLobbyId
            // 
            this.lobbyPlayerLeftLobbyId.Location = new System.Drawing.Point(194, 250);
            this.lobbyPlayerLeftLobbyId.Name = "lobbyPlayerLeftLobbyId";
            this.lobbyPlayerLeftLobbyId.Size = new System.Drawing.Size(340, 23);
            this.lobbyPlayerLeftLobbyId.TabIndex = 15;
            // 
            // lobbyPlayerLeft
            // 
            this.lobbyPlayerLeft.Location = new System.Drawing.Point(7, 250);
            this.lobbyPlayerLeft.Name = "lobbyPlayerLeft";
            this.lobbyPlayerLeft.Size = new System.Drawing.Size(181, 23);
            this.lobbyPlayerLeft.TabIndex = 14;
            this.lobbyPlayerLeft.Text = "lobbyPlayerLeft";
            this.lobbyPlayerLeft.UseVisualStyleBackColor = true;
            // 
            // lobbyPlayerJoinPassword
            // 
            this.lobbyPlayerJoinPassword.Location = new System.Drawing.Point(396, 220);
            this.lobbyPlayerJoinPassword.Name = "lobbyPlayerJoinPassword";
            this.lobbyPlayerJoinPassword.Size = new System.Drawing.Size(138, 23);
            this.lobbyPlayerJoinPassword.TabIndex = 13;
            // 
            // lobbyPlayerJoinLobbyId
            // 
            this.lobbyPlayerJoinLobbyId.Location = new System.Drawing.Point(194, 220);
            this.lobbyPlayerJoinLobbyId.Name = "lobbyPlayerJoinLobbyId";
            this.lobbyPlayerJoinLobbyId.Size = new System.Drawing.Size(195, 23);
            this.lobbyPlayerJoinLobbyId.TabIndex = 12;
            // 
            // lobbyPlayerJoin
            // 
            this.lobbyPlayerJoin.Location = new System.Drawing.Point(7, 219);
            this.lobbyPlayerJoin.Name = "lobbyPlayerJoin";
            this.lobbyPlayerJoin.Size = new System.Drawing.Size(181, 24);
            this.lobbyPlayerJoin.TabIndex = 11;
            this.lobbyPlayerJoin.Text = "lobbyPlayerJoin";
            this.lobbyPlayerJoin.UseVisualStyleBackColor = true;
            // 
            // lobbyChangeGameModeSettings
            // 
            this.lobbyChangeGameModeSettings.Location = new System.Drawing.Point(7, 189);
            this.lobbyChangeGameModeSettings.Name = "lobbyChangeGameModeSettings";
            this.lobbyChangeGameModeSettings.Size = new System.Drawing.Size(527, 23);
            this.lobbyChangeGameModeSettings.TabIndex = 10;
            this.lobbyChangeGameModeSettings.Text = "lobbyChangeGameModeSettings";
            this.lobbyChangeGameModeSettings.UseVisualStyleBackColor = true;
            // 
            // lobbyChangeLobbyGeneralSettings
            // 
            this.lobbyChangeLobbyGeneralSettings.Location = new System.Drawing.Point(7, 159);
            this.lobbyChangeLobbyGeneralSettings.Name = "lobbyChangeLobbyGeneralSettings";
            this.lobbyChangeLobbyGeneralSettings.Size = new System.Drawing.Size(527, 23);
            this.lobbyChangeLobbyGeneralSettings.TabIndex = 9;
            this.lobbyChangeLobbyGeneralSettings.Text = "lobbyChangeLobbyGeneralSettings";
            this.lobbyChangeLobbyGeneralSettings.UseVisualStyleBackColor = true;
            // 
            // lobbyRemoveLobbyId
            // 
            this.lobbyRemoveLobbyId.Location = new System.Drawing.Point(194, 129);
            this.lobbyRemoveLobbyId.Name = "lobbyRemoveLobbyId";
            this.lobbyRemoveLobbyId.Size = new System.Drawing.Size(340, 23);
            this.lobbyRemoveLobbyId.TabIndex = 8;
            // 
            // lobbyRemoveLobby
            // 
            this.lobbyRemoveLobby.Location = new System.Drawing.Point(7, 128);
            this.lobbyRemoveLobby.Name = "lobbyRemoveLobby";
            this.lobbyRemoveLobby.Size = new System.Drawing.Size(181, 24);
            this.lobbyRemoveLobby.TabIndex = 7;
            this.lobbyRemoveLobby.Text = "lobbyRemoveLobby";
            this.lobbyRemoveLobby.UseVisualStyleBackColor = true;
            // 
            // lobbyCreateLobbyPassword
            // 
            this.lobbyCreateLobbyPassword.Location = new System.Drawing.Point(396, 99);
            this.lobbyCreateLobbyPassword.Name = "lobbyCreateLobbyPassword";
            this.lobbyCreateLobbyPassword.Size = new System.Drawing.Size(138, 23);
            this.lobbyCreateLobbyPassword.TabIndex = 6;
            // 
            // lobbyCreateLobbyName
            // 
            this.lobbyCreateLobbyName.Location = new System.Drawing.Point(194, 99);
            this.lobbyCreateLobbyName.Name = "lobbyCreateLobbyName";
            this.lobbyCreateLobbyName.Size = new System.Drawing.Size(195, 23);
            this.lobbyCreateLobbyName.TabIndex = 5;
            // 
            // lobbyCreateLobby
            // 
            this.lobbyCreateLobby.Location = new System.Drawing.Point(7, 98);
            this.lobbyCreateLobby.Name = "lobbyCreateLobby";
            this.lobbyCreateLobby.Size = new System.Drawing.Size(181, 24);
            this.lobbyCreateLobby.TabIndex = 4;
            this.lobbyCreateLobby.Text = "lobbyCreateLobby";
            this.lobbyCreateLobby.UseVisualStyleBackColor = true;
            this.lobbyCreateLobby.Click += new System.EventHandler(this.lobbyCreateLobby_Click);
            // 
            // lobbyGetAllLobbies
            // 
            this.lobbyGetAllLobbies.Location = new System.Drawing.Point(7, 68);
            this.lobbyGetAllLobbies.Name = "lobbyGetAllLobbies";
            this.lobbyGetAllLobbies.Size = new System.Drawing.Size(527, 23);
            this.lobbyGetAllLobbies.TabIndex = 3;
            this.lobbyGetAllLobbies.Text = "lobbyGetAllLobbies";
            this.lobbyGetAllLobbies.UseVisualStyleBackColor = true;
            // 
            // lobbyAuthUsername
            // 
            this.lobbyAuthUsername.Location = new System.Drawing.Point(194, 8);
            this.lobbyAuthUsername.Name = "lobbyAuthUsername";
            this.lobbyAuthUsername.Size = new System.Drawing.Size(340, 23);
            this.lobbyAuthUsername.TabIndex = 2;
            // 
            // lobbyReAuth
            // 
            this.lobbyReAuth.Location = new System.Drawing.Point(7, 37);
            this.lobbyReAuth.Name = "lobbyReAuth";
            this.lobbyReAuth.Size = new System.Drawing.Size(181, 25);
            this.lobbyReAuth.TabIndex = 1;
            this.lobbyReAuth.Text = "lobbyReAuth";
            this.lobbyReAuth.UseVisualStyleBackColor = true;
            this.lobbyReAuth.Click += new System.EventHandler(this.lobbyReAuth_Click);
            // 
            // lobbyAuth
            // 
            this.lobbyAuth.Location = new System.Drawing.Point(7, 7);
            this.lobbyAuth.Name = "lobbyAuth";
            this.lobbyAuth.Size = new System.Drawing.Size(181, 24);
            this.lobbyAuth.TabIndex = 0;
            this.lobbyAuth.Text = "lobbyAuth";
            this.lobbyAuth.UseVisualStyleBackColor = true;
            this.lobbyAuth.Click += new System.EventHandler(this.lobbyAuth_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(540, 594);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "GameHub";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 646);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "TestGUI";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button lobbyAuth;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button lobbyReAuth;
        private System.Windows.Forms.TextBox lobbyAuthUsername;
        private System.Windows.Forms.Button lobbyGetAllLobbies;
        private System.Windows.Forms.TextBox lobbyCreateLobbyPassword;
        private System.Windows.Forms.TextBox lobbyCreateLobbyName;
        private System.Windows.Forms.Button lobbyCreateLobby;
        private System.Windows.Forms.TextBox lobbyRemoveLobbyId;
        private System.Windows.Forms.Button lobbyRemoveLobby;
        private System.Windows.Forms.Button lobbyChangeGameModeSettings;
        private System.Windows.Forms.Button lobbyChangeLobbyGeneralSettings;
        private System.Windows.Forms.Button lobbyPlayerLeft;
        private System.Windows.Forms.TextBox lobbyPlayerJoinPassword;
        private System.Windows.Forms.TextBox lobbyPlayerJoinLobbyId;
        private System.Windows.Forms.Button lobbyPlayerJoin;
        private System.Windows.Forms.Button lobbyConnect;
        private System.Windows.Forms.TextBox lobbyCreateGameLobbyId;
        private System.Windows.Forms.Button lobbyCreateGame;
        private System.Windows.Forms.TextBox lobbyPlayerLeftLobbyId;
        private System.Windows.Forms.CheckBox logHearbeat;
        private System.Windows.Forms.TextBox lobbyReAuthSecret;
        private System.Windows.Forms.Button lobbyDisconnect;
    }
}

