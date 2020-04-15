using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnlineChatServer.Domain;

namespace OnlineChatServer.Application.Users.Queries.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;


        public LoginHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Model.Login);

                if (user != null && await _userManager.CheckPasswordAsync(user, request.Model.Password))
                {
                    var claims = await GetUserRolesClaims(user);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddHours(4),
                        SigningCredentials =
                            new
                                SigningCredentials(
                                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.JWTSecretKey)),
                                    SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return token;
                }

                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private async Task<List<Claim>> GetUserRolesClaims(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var options = new IdentityOptions();

            var claims = new List<Claim>();

            claims.Add(new Claim("UserID", user.Id));

            foreach (var role in roles) claims.Add(new Claim(options.ClaimsIdentity.RoleClaimType, role));

            return claims;
        }
    }
}