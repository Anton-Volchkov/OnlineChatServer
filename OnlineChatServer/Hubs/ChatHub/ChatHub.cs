using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OnlineChatServer.Application.Messages.Commands.AddMessage;
using OnlineChatServer.DataAccess;
using OnlineChatServer.Domain;
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
            await Clients.All.SendAsync("OnlineUsers", _service.GetOnlineUsers());
        }


        public async Task SendMessage(string recipientUserID, string textMessage)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;
           
            var message = _service.GenerateMessage(userID, recipientUserID, textMessage);
            var connections = _service.GetAllUserConnections(recipientUserID);
            foreach (var connection in connections) await Clients.Client(connection).SendAsync("NewMessage", message);
            
            await _mediator.Send(new AddMessageCommand()
            {
                RecipientID = recipientUserID,
                SenderID = userID,
                TextMessage = textMessage
            });

        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;
            _service.Disconnect(userID, Context.ConnectionId);
            await Clients.All.SendAsync("OnlineUsers", _service.GetOnlineUsers());
        }
    }
}