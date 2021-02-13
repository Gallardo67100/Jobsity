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

        public MessageService(JobsityChatroomContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public Task Send(string userName, string message)
        {
            var newMessage = new MessageEntity() { UserName = userName, MessageText = message };

            dbcontext.Messages.Add(newMessage);
            return dbcontext.SaveChangesAsync();
        }
    }
}
