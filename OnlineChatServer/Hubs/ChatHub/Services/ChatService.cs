﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using OnlineChatServer.Hubs.ChatHub.Models;

namespace OnlineChatServer.Hubs.ChatHub.Services
{
    public class ChatService
    {
        private readonly List<ChatUser> Users = new List<ChatUser>();

        public void Connect(string userID, string connectionID, string fullName,string imagePath)
        {
            Users.Add(new ChatUser
            {
                UserID = userID,
                ConnectionID = connectionID,
                FullName = fullName,
                ImagePath = imagePath
            });
        }

        public void Disconnect(string userID, string connectionID)
        {
            var user = Users.FirstOrDefault(x => x.UserID == userID && x.ConnectionID == connectionID);
            if(user != null)
            {
                Users.Remove(user);
            }
        }

        public List<Message> GenerateMessage(string senderUserID, string recipientUserID, string textMessage)
        {
            var messages = new List<Message>();
            var sender = Users.FirstOrDefault(x => x.UserID == senderUserID);
            var nameSender = "Неизвестный пользователь";
            var date = DateTime.Now.ToString("dd.mm.yyyy HH:mm");
            if(sender != null)
            {
                nameSender = sender.FullName;
            }
            var allUserConnections = Users.Where(x => x.UserID == recipientUserID);

            foreach(var connection in allUserConnections)
                messages.Add(new Message
                {
                    DispatchTime = date,
                    RecipientID = recipientUserID,
                    SenderID = senderUserID,
                    TextMessage = textMessage,
                    FullNameSender = nameSender,
                    RecipientConnectionID = connection.ConnectionID
                });

            return messages;
        }

        public List<ChatUser> GetOnlineUsers()
        {
            return Users.DistinctBy(x => x.UserID).ToList();
        }

        public string GetConnectionID(string userID)
        {
            var user = Users.FirstOrDefault(x => x.UserID == userID);

            if(user != null)
            {
                return user.ConnectionID;
            }

            return "";
        }
    }
}