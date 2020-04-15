using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OnlineChatServer.DataAccess;
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
            var messages = _service.GenerateMessage(userID, recipientUserID, textMessage);
            foreach (var msg in messages) await Clients.Client(msg.RecipientConnectionID).SendAsync("NewMessage", msg);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userID = Context.User.Claims.First(x => x.Type == "UserID").Value;
            _service.Disconnect(userID, Context.ConnectionId);
            await Clients.All.SendAsync("OnlineUsers", _service.GetOnlineUsers());
        }
    }
}