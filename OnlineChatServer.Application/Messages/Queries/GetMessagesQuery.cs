using System.Collections.Generic;
using MediatR;
using OnlineChatServer.Domain;

namespace OnlineChatServer.Application.Messages.Queries
{
    public class GetMessagesQuery : IRequest<List<ChatMessage>>
    {
        public string SenderID { get; set; }
        public string RecipientID { get; set; }
    }
}