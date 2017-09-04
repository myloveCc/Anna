using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Kitty.ServiceConfig;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kitty.ServicesRegister
{
    public class KittyHostedService:IHostedService
    {
        private CancellationTokenSource _cts;
        private readonly IConsulClient _consulClient;
        private readonly IOptions<ServiceRegisterConfig> _consulConfig;
        private readonly ILogger<KittyHostedService> _logger;
        private readonly IServer _server;
        private string _registrationID;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="consulClient"></param>
        /// <param name="consulConfig"></param>
        /// <param name="logger"></param>
        /// <param name="server"></param>
        public KittyHostedService(IConsulClient consulClient, IOptions<ServiceRegisterConfig> consulConfig, ILogger<KittyHostedService> logger, IServer server)
        {
            _server = server;
            _logger = logger;
            _consulConfig = consulConfig;
            _consulClient = consulClient;
        }

        /// <summary>
        /// 应用启动注册服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var registration = AgentServiceRegistrationBuilder();

            _logger.LogInformation($"Registering {_registrationID} in consul ");

            await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);

            try
            {
                await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Registering {_registrationID} failed");
            }
        }

        /// <summary>
        /// 应用关闭注销服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation($"Deregistering {_registrationID} from Consul");
            try
            {
                await _consulClient.Agent.ServiceDeregister(_registrationID, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Deregisteration {_registrationID} failed");
            }
        }

        /// <summary>
        /// AgentServiceRegistration builder
        /// </summary>
        /// <returns></returns>
        private AgentServiceRegistration AgentServiceRegistrationBuilder()
        {
            //获取服务地址信息
            var features = _server.Features;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            var uri = new Uri(address);

            var service = _consulConfig.Value.Service;

            _registrationID = $"{service.ServiceId}-{uri.Port}";

            var registration = new AgentServiceRegistration()
            {
                ID = _registrationID,
                Name = service.ServiceName,
                Address = $"{uri.Scheme}://{uri.Host}",
                Port = uri.Port,
                Tags = service.Tags
            };

            //健康检查
            var checks = new List<AgentServiceCheck>();
            if (service.HealthChecks != null && service.HealthChecks.Any())
            {
                var host = $"{uri.Scheme}://{uri.Host}:{uri.Port}";

                foreach (var check in service.HealthChecks)
                {
                    checks.Add(new AgentServiceCheck()
                    {
                        HTTP = $"{host}/{check.HttpApi}",
                        Interval = TimeSpan.FromSeconds(check.Interval),
                        Timeout = TimeSpan.FromSeconds(check.Timeout)
                    });
                }
            }

            registration.Checks = checks.ToArray();

            return registration;
        }
    }
}
