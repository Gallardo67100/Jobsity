using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChatroom.Bot.DTOs
{
    public class MessageDTO
    {
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string MessageText { get; set; }
    }
}
