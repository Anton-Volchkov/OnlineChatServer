using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineChatServer.DataAccess;
using OnlineChatServer.Domain;

namespace OnlineChatServer.Application.Messages.Queries
{
    public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, List<ChatMessage>>
    {
        private readonly ApplicationDbContext _db;    
        public GetMessagesHandler(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task<List<ChatMessage>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_db.Messages
                .Where(x => x.SenderID == request.SenderID && x.RecipientID == request.RecipientID ||
                            x.SenderID == request.RecipientID && x.RecipientID == request.SenderID)
                .ToList());
        }
    }
}