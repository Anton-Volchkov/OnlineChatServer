using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appSettings = appSettings.Value;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(ApplicationUserModel model)
        {
            model.Role = "User";
            try
            {
                var user = new ApplicationUser(model.Login,model.FirstName,model.LastName,model.Email);
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(result);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("123")]
        [Authorize(Roles = "User")]
        public string Test()
        {
            return "1234";
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
                    var role = await _userManager.GetRolesAsync(user);
                    IdentityOptions options = new IdentityOptions();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserID", user.Id),
                            new Claim(options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault()), 
                           
                        }),
                        Expires = DateTime.Now.AddHours(4),
                        SigningCredentials =
                            new
                                SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SecretJWTKey)),
                                                   SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok(new {token});
                }
                else
                    return BadRequest(new {message = "Incorrect user or password"});
            }
            catch (Exception e)
            {

                return BadRequest(new {message = "Incorrect Data"});
            }
        }
    }
}
