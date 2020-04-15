using MediatR;
using OnlineChatServer.Domain.Models;

namespace OnlineChatServer.Application.Users.Queries.Login
{
    public class LoginCommand : IRequest<string>
    {
        public LoginModel Model { get; set; }
        public string JWTSecretKey { get; set; }
    }
}