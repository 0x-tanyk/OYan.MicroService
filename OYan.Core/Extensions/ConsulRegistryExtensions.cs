using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OYan.Core.Registry;

namespace OYan.Core
{
    public static class ConsulRegistryExtensions
    {
        /// <summary>
        /// 添加服务注册、发现
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConsulRegistry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient>(new ConsulClient(x => x.Address = new Uri(configuration["consul:clientAddress"])));
            //注册ConsulRegisterService 这个servcie在app启动的时候会自动注册服务信息
            services.AddHostedService<ConsulRegisterService>();
        }
    }
}
