using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChatroom.Services
{
    public interface IRabbitMQService
    {
        public void SendToQueue(string message);
    }
}
