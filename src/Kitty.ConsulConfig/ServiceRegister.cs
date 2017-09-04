using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ServiceConfig
{
    /// <summary>
    /// 服务注册配置信息
    /// </summary>
    public class ServiceRegister:BasicServiceInfo
    {
        /// <summary>
        /// 服务健康检查
        /// </summary>
        public List<ServiceHealthCheck> HealthChecks { get; set; }
    }
}
