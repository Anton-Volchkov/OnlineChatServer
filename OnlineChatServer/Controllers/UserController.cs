using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineChatServer.Domain;
using OnlineChatServer.Domain.Models;
using OnlineChatServer.Models;

namespace OnlineChatServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(ApplicationUserModel model)
        {
            model.Role = "User";
            try
            {
                const string roleName = "User";
                var user = new ApplicationUser(model.Login, model.FirstName, model.LastName, model.Email);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(role);
                }


                await _userManager.AddToRoleAsync(user, roleName);
                return Ok(result);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Login);

                if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var claims = await GetUserRolesClaims(user);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddHours(4),
                        SigningCredentials =
                            new
                                SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SecretJWTKey)),
                                                   SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok(new { token });
                }

                return BadRequest(new { message = "Incorrect user or password" });
            }
            catch(Exception e)
            {
                return BadRequest(new { message = "Incorrect Data" });
            }
        }

        private async Task<List<Claim>> GetUserRolesClaims(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var options = new IdentityOptions();

            var claims = new List<Claim>();

            claims.Add(new Claim("UserID", user.Id));

            foreach(var role in roles) claims.Add(new Claim(options.ClaimsIdentity.RoleClaimType, role));

            return claims;
        }
    }
}