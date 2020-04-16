using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineChatServer.Application.Messages.Queries;
using OnlineChatServer.Domain;
using OnlineChatServer.Hubs.ChatHub.Models;

namespace OnlineChatServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Authorize]
        [Route("GetChatHistory/{recipientID}")]
        public async Task<List<Message>> GetHistory(string recipientID)
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var messages = await _mediator.Send(new GetMessagesQuery()
            {
                RecipientID = recipientID,
                SenderID = userID
            });

            return messages.Select(x => new Message()
            {
                SenderID = x.SenderID,
                RecipientID = x.RecipientID,
                TextMessage = x.TextMessage,
                DispatchTime = x.DispatchTime,
                FullNameSender = x.FullNameSender
            }).ToList();
        }
}
}