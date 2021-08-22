using System;
using Microsoft.Extensions.DependencyInjection;

namespace MaestroMediaCenter.Common
{
    public class Loader
    {
        public Loader()
        {
        }

        public void LoadDefaults(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserProvider, UserProvider>();
            serviceCollection.AddSingleton<SampleChain, SampleChain>();
        }
    }
}
