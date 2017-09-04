using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ServiceConfig
{
    /// <summary>
    /// 服务发现配置信息
    /// </summary>
    public class ServiceDiscovery:BasicServiceInfo
    {
        public string ConsulAddress { get; set; }
    }
}
