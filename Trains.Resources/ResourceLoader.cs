using System;
using System.Collections.Generic;
using System.Globalization;

namespace Trains.Resources
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
            Resource = await new LocalData().GetData<Dictionary<string, string>>("Resource.json", CultureInfo.CurrentCulture.Name);
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
