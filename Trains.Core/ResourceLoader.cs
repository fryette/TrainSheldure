using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Trains.Core.Service;
using Trains.Core;
using Cirrious.CrossCore;
using Trains.Core.Interfaces;

namespace Trains.Core
{
    public sealed class ResourceLoader
    {
        private static volatile ResourceLoader _instance;
        private static readonly object SyncRoot = new Object();

        public Dictionary<string, string> Resource;

        private ResourceLoader()
        {
            Init();
        }

        private async void Init()
        {
            Resource = Mvx.Resolve<ISerializableService>().Desserialize<Dictionary<string, string>>(Constants.ResourceLoader);
        }

        public static ResourceLoader Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ResourceLoader();
                    }
                }
                return _instance;
            }
        }
    }
}
