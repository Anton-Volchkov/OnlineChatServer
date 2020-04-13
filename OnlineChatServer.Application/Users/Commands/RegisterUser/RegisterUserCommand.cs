using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using OnlineChatServer.Domain.Models;

namespace OnlineChatServer.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<object>
    {
        public ApplicationUserModel User { get; set; }
    }
}
