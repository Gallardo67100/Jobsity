using JobsityChatroom.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobsityChatroom.Services
{
    public interface IMessageService
    {
        public Task Send(string userName, string message);
        public List<MessageEntity> GetAll();
    }
}
