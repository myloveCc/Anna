using System;

namespace Kitty.ServiceConfig
{
    /// <summary>
    /// Consul 服务注册配置
    /// </summary>
    public class ServiceRegisterConfig
    {
        /// <summary>
        /// Consul 地址信息
        /// </summary>
        public string ConsulAddress { get; set; }

        /// <summary>
        /// 注册服务信息
        /// </summary>
        public ServiceRegister Service { get; set; }
    }
}
