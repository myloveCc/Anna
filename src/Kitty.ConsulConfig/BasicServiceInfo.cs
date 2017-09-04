using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ServiceConfig
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public class BasicServiceInfo
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务标签
        /// </summary>
        public string[] Tags { get; set; }

    }
}
