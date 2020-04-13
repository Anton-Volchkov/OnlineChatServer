using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineChatServer.Domain;
using OnlineChatServer.Domain.Models;

namespace OnlineChatServer.Application.Users.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterUserHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
          
            try
            {
                const string roleName = "User";
                var user = new ApplicationUser(request.User.Login, request.User.FirstName, request.User.LastName, request.User.Email);
                var result = await _userManager.CreateAsync(user, request.User.Password);
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(role);
                }


                await _userManager.AddToRoleAsync(user, roleName);
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
    
}
