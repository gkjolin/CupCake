﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using CupCake.Core.Events;
using CupCake.Core.Log;
using CupCake.Core.Metadata;
using CupCake.Core.Storage;
using MuffinFramework.Services;

namespace CupCake.Core
{
    public abstract class CupCakeServicePart<TProtocol> : ServicePart<TProtocol>
    {
        private readonly Lazy<ConnectionPlatform> _connectionPlatform;
        private readonly Lazy<EventManager> _events;
        private readonly Lazy<Logger> _logger;
        private readonly Lazy<string> _name;
        private readonly Lazy<StoragePlatform> _storagePlatform;
        private readonly Lazy<SynchronizePlatform> _synchronizePlatofrm;
        private readonly Lazy<MetadataPlatform> _metadataPlatform;

        protected CupCakeServicePart()
        {
            this._name = new Lazy<string>(this.FindName);

            this._connectionPlatform = new Lazy<ConnectionPlatform>(() => this.PlatformLoader.Get<ConnectionPlatform>());
            this._synchronizePlatofrm =
                new Lazy<SynchronizePlatform>(() => this.PlatformLoader.Get<SynchronizePlatform>());
            this._storagePlatform = new Lazy<StoragePlatform>(() => this.PlatformLoader.Get<StoragePlatform>());
            this._metadataPlatform = new Lazy<MetadataPlatform>(() => this.PlatformLoader.Get<MetadataPlatform>());

            this._logger = new Lazy<Logger>(() =>
            {
                var logService = this.PlatformLoader.Get<LogPlatform>();
                string name = this.GetName();
                return new Logger(logService, name);
            });

            this._events = new Lazy<EventManager>(() =>
            {
                var eventsPlatform = this.PlatformLoader.Get<EventsPlatform>();
                return new EventManager(eventsPlatform, this);
            });
        }

        protected Logger Logger
        {
            get { return this._logger.Value; }
        }

        protected EventManager Events
        {
            get { return this._events.Value; }
        }

        protected ConnectionPlatform ConnectionPlatform
        {
            get { return this._connectionPlatform.Value; }
        }

        protected SynchronizePlatform SynchronizePlatform
        {
            get { return this._synchronizePlatofrm.Value; }
        }

        protected StoragePlatform StoragePlatform
        {
            get { return this._storagePlatform.Value; }
        }

        protected MetadataPlatform MetadataPlatform
        {
            get { return this._metadataPlatform.Value; }
        }

        private string FindName()
        {
            var pluginName =
                (PluginNameAttribute)
                    Assembly.GetAssembly(this.GetType())
                        .GetCustomAttributes(typeof(PluginNameAttribute), false)
                        .FirstOrDefault();

            if (pluginName != null)
                return pluginName.Name;

            return this.GetType().Namespace;
        }

        protected virtual string GetName()
        {
            return this._name.Value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._events.IsValueCreated)
                {
                    this._events.Value.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}