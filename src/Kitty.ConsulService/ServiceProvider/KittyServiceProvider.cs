using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Kitty.ServiceConfig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Consul;
using Kitty.ConsulService.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Kitty.ConsulService.ServiceProvider
{
    public class KittyServiceProvider:IKittyServiceProvider
    {
        private readonly IOptions<ConsulConfig> _consulConfig;
        private readonly ILogger<KittyServiceProvider> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="consulConfig"></param>
        /// <param name="logger"></param>
        public KittyServiceProvider(IOptions<ConsulConfig> consulConfig, ILogger<KittyServiceProvider> logger)
        {
            _consulConfig = consulConfig;
            _logger = logger;
        }

        /// <summary>
        /// 获取健康Api集合
        /// </summary>
        /// <param name="config">服务发现配置信息</param>
        /// <returns></returns>
        public async Task<List<ServiceApi>> HealthApisAsync(ServiceDiscovery config)
        {
            if (config == null)
            {
                _logger.LogWarning($"{nameof(config)} is null");
                return null;
            }

            _logger.LogInformation($"Discovering Services from consul on address {config.ConsulAddress}.");
            using (ConsulClient consulClient = new ConsulClient(c =>
             {
                 var uri = new Uri(config.ConsulAddress);
                 c.Address = uri;
             }))
            {
                QueryResult<ServiceEntry[]> result = await consulClient.Health.Service(config.ServiceName, null, true);

                ServiceEntry[] services = result.Response;

                if (services != null && services.Any())
                {
                    var apis = new List<ServiceApi>();

                    foreach (var item in services)
                    {
                        apis.Add(new ServiceApi()
                        {
                            ServiceId = item.Service.ID,
                            ServiceName = item.Service.Service,
                            ApiHost = item.Service.Address,
                            ApiPort = item.Service.Port,
                            Tags = item.Service.Tags
                        });
                    }
                    _logger.LogInformation($"{apis.Count} api endpoints found.");
                    return apis;
                }
            }
            return null;
        }
    }
}
