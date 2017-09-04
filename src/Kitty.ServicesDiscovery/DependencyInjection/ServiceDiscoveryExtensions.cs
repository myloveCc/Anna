using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Kitty.ServiceConfig;
using Kitty.ServicesRegister;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Consul 服务发现 扩展
    /// </summary>
    public static class ServiceDiscoveryExtensions
    {
        /// <summary>
        /// 发现服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKittyDisconvery(this IServiceCollection services, IConfiguration Configuration)
        {
            // Add consul client
            services.Configure<ServiceRegisterConfig>(Configuration.GetSection("ServiceRegisterConfig"));
            services.AddSingleton<IConsulClient, ConsulClient>(m => new ConsulClient(config =>
            {
                var addrss = Configuration["ServiceRegisterConfig:ConsulAddress"] ?? "localhost:8500";
                config.Address = new Uri(addrss);
            }));

            services.TryAddSingleton<IHostedService, KittyHostedService>();
            return services;
        }
    }
}
