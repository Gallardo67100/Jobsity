using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChatroom.Services
{
    public interface IMessageService
    {
        public Task Send(string userName, string message);
    }
}
