using KubeClient;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Kubernetes;
using Ocelot.ServiceDiscovery;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;

namespace OYan.Gateway.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class OcelotBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly ServiceDiscoveryFinderDelegate FixedKubernetesProviderFactoryGet = (provider, config, reroute) =>
        {
            var serviceDiscoveryProvider = KubernetesProviderFactory.Get(provider, config, reroute);
            if (serviceDiscoveryProvider is KubernetesServiceDiscoveryProvider)
            {
                serviceDiscoveryProvider = new Kube(serviceDiscoveryProvider);
            }
            else if (serviceDiscoveryProvider is PollKubernetes)
            {
                serviceDiscoveryProvider = new PollKube(serviceDiscoveryProvider);
            }
            return serviceDiscoveryProvider;
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="usePodServiceAccount"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddKubernetesFixed(this IOcelotBuilder builder, bool usePodServiceAccount = true)
        {
            builder.Services.AddSingleton(FixedKubernetesProviderFactoryGet);
            builder.Services.AddKubeClient(usePodServiceAccount);
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Kube : IServiceDiscoveryProvider
        {
            private readonly IServiceDiscoveryProvider serviceDiscoveryProvider;
            public Kube(IServiceDiscoveryProvider serviceDiscoveryProvider)
            {
                this.serviceDiscoveryProvider = serviceDiscoveryProvider;
            }
            public Task<List<Service>> Get()
            {
                return this.serviceDiscoveryProvider.Get();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class PollKube : IServiceDiscoveryProvider
        {
            private readonly IServiceDiscoveryProvider serviceDiscoveryProvider;
            public PollKube(IServiceDiscoveryProvider serviceDiscoveryProvider)
            {
                this.serviceDiscoveryProvider = serviceDiscoveryProvider;
            }
            public Task<List<Service>> Get()
            {
                return this.serviceDiscoveryProvider.Get();
            }
        }
    }
}
