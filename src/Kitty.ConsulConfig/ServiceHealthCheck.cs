using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ServiceConfig
{
    /// <summary>
    /// 服务健康检查
    /// </summary>
    public class ServiceHealthCheck
    {
        /// <summary>
        /// 健康检查 http api 地址
        /// </summary>
        public string HttpApi { get; set; }

        /// <summary>
        /// 健康检查间隔时间（单位s）
        /// </summary>
        public int Interval { get; set; } = 5;

        /// <summary>
        /// 健康检查过期时间（单位s）
        /// </summary>
        public int Timeout { get; set; } = 3;
    }
}
