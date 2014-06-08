﻿using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using CupCake.Chat;
using CupCake.Command;
using CupCake.Command.Source;
using CupCake.Core;
using CupCake.Core.Events;
using CupCake.Host;
using CupCake.HostAPI.IO;
using CupCake.HostAPI.Title;
using CupCake.Messages.Receive;
using CupCake.Protocol;
using CupCake.Server.StorageProviders;
using CupCake.Server.SyntaxProviders;
using PlayerIOClient;

namespace CupCake.Server
{
    public class CupCakeClientHost
    {
        public const string GameId = "everybody-edits-su9rn58o40itdbnw69plyw";
        private CupCakeClient _client;
        private string _cs;
        private DatabaseType _dbType;
        private EventsPlatform _eventsPlatform;

        public event Action<string> Output;

        protected virtual void OnOutput(string e)
        {
            Action<string> handler = this.Output;
            if (handler != null) handler(e);
        }

        public event Action<string> Title;

        protected virtual void OnTitle(string obj)
        {
            Action<string> handler = this.Title;
            if (handler != null) handler(obj);
        }

        public void Input(string input)
        {
            this._eventsPlatform.Event<InputEvent>().Raise(this, new InputEvent(input));
        }

        private void PlatformLoader_EnableComplete(object sender, EventArgs e)
        {
            // Change the default storage source
            var storagePlatform = this._client.PlatformLoader.Get<StoragePlatform>();
            if (this._dbType == DatabaseType.MySql)
                storagePlatform.StorageProvider = new MySqlStorageProvider(this._cs);
            else
                storagePlatform.StorageProvider = new SQLiteStorageProvider(this._cs);

            // Listen to HostAPI events
            _eventsPlatform = this._client.PlatformLoader.Get<EventsPlatform>();
            _eventsPlatform.Event<OutputEvent>().Bind(this.OnOutput, EventPriority.Lowest);
            _eventsPlatform.Event<ChangeTitleEvent>().Bind(this.OnChangeTitle, EventPriority.Lowest);
        }
        private void ServiceLoader_EnableComplete(object sender, EventArgs e)
        {
            // Change the default chat and output formats
            this._client.ServiceLoader.Get<ChatService>().SyntaxProvider = new CupCakeChatSyntaxProvider();
            this._client.ServiceLoader.Get<IOService>().SyntaxProvider = new CupCakeIOSyntaxProvider();
        }

        private void OnOutput(object sender, OutputEvent e)
        {
            this.OnOutput(e.Message);
        }

        private void OnChangeTitle(object sender, ChangeTitleEvent e)
        {
            this.OnTitle(e.NewTitle);
        }

        private void connection_OnDisconnect(object sender, string message)
        {
            this._client.Dispose();
            this.LogMessage("Disconnected from Everybody Edits");
            Environment.Exit(1);
        }

        private void LogMessage(string str)
        {
            this.OnOutput(String.Format("*** {0}", str));
        }

        public void Start(AccountType accType, string email, string password, string roomId, string[] directories,
            DatabaseType dbType, string cs)
        {
            this._dbType = dbType;
            this._cs = cs;

            this.LogMessage("Logging in...");
            // Connect to playerIO
            Client playerioclient = accType == AccountType.Regular
                ? PlayerIO.QuickConnect.SimpleConnect(GameId, email, password)
                : PlayerIO.QuickConnect.FacebookOAuthConnect(GameId, email, String.Empty);

            this.LogMessage("Login successful. Getting room version...");
            int version = RoomHelper.GetVersion();
            string roomType = RoomHelper.GetRoomType(roomId, version);


            this.LogMessage("Done. Joining room...");
            Connection connection = playerioclient.Multiplayer.CreateJoinRoom(roomId, roomType, true, null, null);
            connection.OnDisconnect += this.connection_OnDisconnect;

            this._client = new CupCakeClient(connection, new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            this.LogMessage("Join complete. Loading plugin dlls...");
            foreach (string dir in directories)
            {
                this._client.AggregateCatalog.Catalogs.Add(new DirectoryCatalog(dir));
            }

            this._client.PlatformLoader.EnableComplete += this.PlatformLoader_EnableComplete;
            this._client.ServiceLoader.EnableComplete += this.ServiceLoader_EnableComplete;

            this.LogMessage("Getting stuff ready...");
            this._client.Start();
            this.LogMessage(String.Format("Welcome to CupCake! (API version: {0})", this.GetVersion()));
        }

        private string GetVersion()
        {
            var attribute =
                (AssemblyInformationalVersionAttribute)Assembly.GetAssembly(typeof(CupCakeClient))
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();

            if (attribute != null)
                return attribute.InformationalVersion;
            return "Unknown!";
        }
    }
}