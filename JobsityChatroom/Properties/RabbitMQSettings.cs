using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChatroom.Properties
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; }
        public string CommandQueueName { get; set;  }
        public string ResponseQueueName { get; set; }
    }
}
