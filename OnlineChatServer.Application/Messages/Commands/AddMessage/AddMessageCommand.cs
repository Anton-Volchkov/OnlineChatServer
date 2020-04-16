using MediatR;

namespace OnlineChatServer.Application.Messages.Commands.AddMessage
{
    public class AddMessageCommand : IRequest<Unit>
    {
        public string SenderID { get; set; }
        public string RecipientID { get; set; }
        public string TextMessage { get; set; }
    }
}