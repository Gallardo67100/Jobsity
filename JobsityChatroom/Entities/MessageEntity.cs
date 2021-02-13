using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChatroom.Entities
{
    public class MessageEntity
    {
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string MessageText { get; set; }
    }
}
