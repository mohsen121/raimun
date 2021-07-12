using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raimun.Api.Configurations
{
    public class RabbitMQConfig
    {
        public string Hostname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
