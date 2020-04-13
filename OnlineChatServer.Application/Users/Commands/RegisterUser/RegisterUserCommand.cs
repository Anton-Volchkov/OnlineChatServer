using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineChatServer.Domain.Models;

namespace OnlineChatServer.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public ApplicationUserModel User { get; set; }
    }
}
