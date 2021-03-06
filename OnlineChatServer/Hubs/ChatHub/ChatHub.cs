﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OnlineChatServer.Application.Messages.Commands.AddMessage;
using OnlineChatServer.DataAccess;
using OnlineChatServer.Domain;
using OnlineChatServer.Domain.Entities;
using OnlineChatServer.Domain.Models;
using OnlineChatServer.Hubs.ChatHub.Services;

namespace OnlineChatServer.Hubs.ChatHub
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        private readonly IMediator _mediator;
        private readonly ChatService _service;

        public ChatHub(IMediator mediator, ChatService service, ApplicationDbContext db)
        {
            _db = db;
            _mediator = mediator;
            _service = service;
        }

        public async Task Join(string fullName)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;
            var user = _db.Users.FirstOrDefault(x => x.Id == userID);
            _service.Connect(userID, Context.ConnectionId, fullName, user.ImagePath);

            var onlineUsers = _service.GetOnlineUsers();

            await Clients.All.SendAsync("OnlineUsers", onlineUsers);
        }

        public async Task RemoveMessage(int msgId)
        {
            var msg = _db.Messages.FirstOrDefault(x => x.Id == msgId);

            if(msg == null)
            {
                return;
            }

            _db.Messages.Remove(msg);

            await _db.SaveChangesAsync();

            await Clients.All.SendAsync("MessageDeleted", msgId);
        }

        public async Task SendMessage(string recipientUserID, string textMessage)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;

            var message = _service.GenerateMessage(userID, recipientUserID, textMessage);
            var connections = _service.GetAllUserConnections(recipientUserID);
            connections.AddRange(_service.GetAllUserConnections(userID));

            if (_db.UnreadDialogs.FirstOrDefault(x => x.SenderID == userID && x.UserID == recipientUserID) == null)
            {
                await _db.UnreadDialogs.AddAsync(new UnreadDialog
                {
                    SenderID = userID,
                    UserID = recipientUserID
                });

                await _db.SaveChangesAsync();
            }

            var msgId = await _mediator.Send(new AddMessageCommand
            {
                RecipientID = recipientUserID,
                SenderID = userID,
                TextMessage = textMessage
            });

           message.Id = msgId;
            foreach (var connection in connections) await Clients.Client(connection).SendAsync("NewMessage", message);
        }

        public async Task ReadDialog(string senderID)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;
            var dialog = _db.UnreadDialogs.FirstOrDefault(x => x.SenderID == senderID && x.UserID == userID);
            if (dialog != null)
            {
                _db.UnreadDialogs.Remove(dialog);
                await _db.SaveChangesAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;
            _service.Disconnect(userID, Context.ConnectionId);
            await Clients.All.SendAsync("OnlineUsers", _service.GetOnlineUsers());
        }

        public async Task BlockUser(string blockUserId)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;

            if(_db.BlackList.Any(x => x.UserId == userID && x.BlockUserId == blockUserId))
            {
                return;
            }

            await _db.BlackList.AddAsync(new BlackList()
            {
                BlockUserId = blockUserId,
                UserId =  userID
            });
          
            await _db.SaveChangesAsync();

            await Clients.All.SendAsync("BlockUser", new BlackListDataModel()
            { 
                BlockUserId = blockUserId,
                UserId =  userID
            });

        }

        public async Task UnblockUser(string unblockUserId)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;


            var unblockUsers = _db.BlackList.Where(x => x.UserId == userID && x.BlockUserId == unblockUserId);

           
            _db.BlackList.RemoveRange(unblockUsers);
            
            await _db.SaveChangesAsync();

            await Clients.All.SendAsync("UnblockUser", new BlackListDataModel()
            {
                BlockUserId = unblockUserId,
                UserId = userID
            });

        }
    }
}