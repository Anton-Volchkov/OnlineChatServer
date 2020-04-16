using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineChatServer.DataAccess;
using OnlineChatServer.Domain;

namespace OnlineChatServer.Application.Messages.Commands.AddMessage
{
    public class AddMessageHandler : IRequestHandler<AddMessageCommand, Unit>
    {
        private readonly ApplicationDbContext _db;

        public AddMessageHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var sender = await _db.Users.FirstOrDefaultAsync(x => x.Id == request.SenderID);

            var recipient = await _db.Users.FirstOrDefaultAsync(x => x.Id == request.RecipientID);

            if (recipient == null || sender == null) return Unit.Value;

            var msg = new ChatMessage
            {
                DispatchTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                FullNameSender = $"{sender.FirstName} {sender.LastName}",
                RecipientID = request.RecipientID,
                SenderID = request.SenderID,
                TextMessage = request.TextMessage
            };
            
            _db.Messages.Add(msg);
           
            await _db.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}