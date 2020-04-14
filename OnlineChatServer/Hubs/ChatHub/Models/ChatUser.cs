﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatServer.Hubs.ChatHub.Models
{
    public class ChatUser
    {
        public string ConnectionID { get; set; }
        public string UserID { get; set; }  
        public string FullName { get; set; }
        public string ImagePath { get; set; }
    }
}
