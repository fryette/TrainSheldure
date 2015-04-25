using System;
using System.Collections.Generic;

namespace Trains.Resources
{
    public sealed class ResourceLoader
    {
        private static volatile ResourceLoader _instance;
        private static readonly object SyncRoot = new Object();

        public Dictionary<string,string> Resource;

        private ResourceLoader()
        {
            Init();
        }

        private async void Init()
        {
            LocalData lk = new LocalData();
            Resource = await lk.GetData<Dictionary<string, string>>("Resource.json");
        }

        public static ResourceLoader Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ResourceLoader();
                }
                return _instance;
            }
        }
    }
}
