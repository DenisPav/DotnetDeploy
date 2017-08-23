using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotnetDeploy.Config
{
    public static class ServiceCollectionBuilder
    {
        private static IServiceCollection Services { get; set; }

        public static IServiceCollection CreateWith(Action<IServiceCollection> action)
        {
            Services = new ServiceCollection();
            action(Services);

            return Services;
        }
    }
}
