using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ServiceConfig
{
    public class ConsulConfig
    {
        public ServiceRegisterConfig ServiceRegisterConfig
        {
            get; set;
        }

        public ServiceDiscoveryConfig ServiceDiscoveryConfig
        {
            get; set;
        }
    }
}
