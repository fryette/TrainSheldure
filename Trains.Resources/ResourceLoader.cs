using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Trains.Resources
{
    public sealed class ResourceLoader
    {
        private static volatile ResourceLoader instance;
        private static object syncRoot = new Object();

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
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ResourceLoader();
                    }
                }
                return instance;
            }
        }
    }
}
