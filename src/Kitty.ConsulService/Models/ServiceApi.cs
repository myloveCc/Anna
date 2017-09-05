using System;
using System.Collections.Generic;
using System.Text;

namespace Kitty.ConsulService.Models
{
    public class ServiceApi
    {
        public string ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string[] Tags { get; set; }


        public string ApiHost { get; set; }

        public int ApiPort { get; set; }


        public override string ToString()
        {
            return $"{ApiHost}:{ApiPort}/";
        }
    }
}
