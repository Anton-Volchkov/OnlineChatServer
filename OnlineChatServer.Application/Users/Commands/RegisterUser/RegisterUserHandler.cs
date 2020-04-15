using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineChatServer.Domain;

namespace OnlineChatServer.Application.Users.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            const string roleName = "User";
            var user = new ApplicationUser(request.User.Login, request.User.FirstName, request.User.LastName,
                request.User.Email, request.User.ImagePath);
            var result = await _userManager.CreateAsync(user, request.User.Password);
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }


            await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }
    }
}