using JobsityChatroom.Data;
using JobsityChatroom.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChatroom.Services
{
    public class MessageService : IMessageService
    {
        private readonly JobsityChatroomContext dbcontext;
        private readonly ChatroomHub hub;

        public MessageService(JobsityChatroomContext dbcontext, ChatroomHub hub)
        {
            this.dbcontext = dbcontext;
            this.hub = hub;
        }

        public List<MessageEntity> GetAll()
        {
            return dbcontext.Messages.ToList().OrderBy(m => m.Timestamp).TakeLast(5).ToList();
        }

        public async Task Send(string userName, string message)
        {
            var newMessage = new MessageEntity() { UserName = userName, MessageText = message };

            dbcontext.Messages.Add(newMessage);
            await hub.SendMessage(userName, message, DateTime.Now);
            await dbcontext.SaveChangesAsync();
        }
    }
}
