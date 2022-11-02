using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OYan.Core.Registry
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public class ConsulRegisterService : IHostedService
    {
        /// <summary>
        /// ConsulClient
        /// </summary>
        IConsulClient _consulClient;

        /// <summary>
        /// 服务id
        /// </summary>
        string _id = String.Intern(String.Empty);

        /// <summary>
        /// 服务名
        /// </summary>
        string _name = String.Intern(String.Empty);

        /// <summary>
        /// IP地址
        /// </summary>
        public string _ip = String.Intern(String.Empty);

        /// <summary>
        /// 端口
        /// </summary>
        int _port;

        /// <summary>
        /// 健康检查地址
        /// </summary>
        string _healthCheck = String.Intern(String.Empty);


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="consulClient">ConsulClient</param>
        public ConsulRegisterService(IConfiguration config, IConsulClient consulClient)
        {
            var sc = config.GetSection("ServiceInfo");
            _name = sc["name"];
            _id = $"{_name}{new Random().Next()}";
            _ip = config["ip"];
            _port = int.Parse(config["port"]);
            _healthCheck = sc["healthCheck"];
            _consulClient = consulClient;
        }

        /// <summary>
        /// 启动（注册服务到consul）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"start to register service {_id} to consul client ...");
            //await _consulClient.Agent.ServiceDeregister(_id, cancellationToken);
            await _consulClient.Agent.ServiceRegister(new AgentServiceRegistration
            {
                ID = _id,
                Name = _name,
                Address = _ip,
                Port = _port,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(2),
                    Interval = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{_ip}:{_port}/{_healthCheck}",
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });
            var addr = $"http://{_ip}:{_port}/{_healthCheck}";
            Console.WriteLine($"healthCheck:{addr}");
            Console.WriteLine("register service info to consul client Successful ...");
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _consulClient.Agent.ServiceDeregister(_id, cancellationToken);
            Console.WriteLine($"Deregister service {_id} from consul client Successful ...");
        }
    }
}