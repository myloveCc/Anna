using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ServiceConfig
{
    /// <summary>
    /// Consul 服务发现配置
    /// </summary>
    public class ServiceDiscoveryConfig
    {
        public List<ServiceDiscovery> Services { get; set; }
    }
}
