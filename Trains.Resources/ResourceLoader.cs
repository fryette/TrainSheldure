using System;
using System.Reflection;
using System.Resources;

namespace Trains.Resources
{
    public sealed class ResourceLoader
    {
        private static volatile ResourceLoader _instance;
        private static readonly object SyncRoot = new Object();

        public ResourceManager Resource;

        private ResourceLoader()
        {
            var assembly = typeof(Constants).GetTypeInfo().Assembly;
            Resource = new ResourceManager(assembly.GetManifestResourceNames()[0].Replace(".resources", String.Empty), assembly);
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
