using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Kitty.ConsulService;
using Kitty.ConsulService.ServiceProvider;
using Kitty.ServiceConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Conusl 服务注册扩展
    /// </summary>
    public static class ServiceRegisterExtensions
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKitty(this IServiceCollection services, IConfiguration Configuration)
        {
            // Add consul client
            services.Configure<ConsulConfig>(Configuration.GetSection("ConsulConfig"));
            services.AddSingleton<IConsulClient, ConsulClient>(m => new ConsulClient(config =>
            {
                var addrss = Configuration["ConsulConfig:ServiceRegisterConfig:ConsulAddress"] ?? "localhost:8500";
                config.Address = new Uri(addrss);
            }));

            services.TryAddSingleton<IHostedService, KittyHostedService>();

            services.TryAddScoped<IKittyServiceProvider, KittyServiceProvider>();

            return services;
        }
    }
}
