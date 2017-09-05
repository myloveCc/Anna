using System.Collections.Generic;
using System.Threading.Tasks;
using Kitty.ConsulService.Models;
using Kitty.ServiceConfig;

namespace Kitty.ConsulService.ServiceProvider
{
    public interface IKittyServiceProvider
    {
        Task<List<ServiceApi>> HealthApisAsync(ServiceDiscovery config);
    }
}
